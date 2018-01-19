using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models.MapModel;
using Models.Params;
using BLL;
using Common;
using Models;
using System.Data.Entity;
namespace ShuangZan.Web.Controllers
{
     [LoginCheckFilterAttribute(IsCheck = false)]
    public class BeautyGirlController : Controller
    {
         public IBeautifulGirlsBll BeautifulGirlsBll { get; set; }
         public ILeaveMsgBll LeaveMsgBll { get; set; }
         public IPersonalUserBll PersonalUserBll { get; set; }
         IForbiddenListBll ForbiddenListBll { get; set; }
         IBlackListIPBll BlackListIPBll { get; set; }
         IAdvertisementBll AdvertisementBll { get; set; }
       
        public ActionResult GirlIndex()
        {
            ViewData["advert"] = AdvertisementBll.GetAllTypeAdvert("3", 2);
            return View();
        }
        #region 美图局部分类搜索数据
        /// <summary>
        /// 美图局部分类搜索数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGirlsData()
        {
            string type = Request["type"];
            string game = Request["game"];
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "10");           
           var  list = BeautifulGirlsBll.GetAllGirlsInfo(type, game,pageIndex,pageSize).Select(g=>new {
               g.Id,
               g.Imgs,             
               g.Tags,
               g.Title
           }).ToList();                                      
            var ressult = new { Data = list };
            return Json(ressult, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 美图详情    
        public ActionResult GirlDetails(int id)
        {
            var data = BeautifulGirlsBll.GetGirlDetails(id).Select(t => new GirlsViewModel()
              {
                  Id = t.Id,
                  Title = t.Title,
                  Tags = t.Tags,
                  LeaveMsgCount = t.LeaveMsgCount,
                  LeadTxt = t.LeadTxt,
                  Image = t.Image,
                  InTime = t.InTime,
                  Views = t.Views,
              }).FirstOrDefault();
            ViewData.Model = data as GirlsViewModel;
            string[] arryImg = data.Image.Split(',');
            List<GirlsViewModel> list = new List<GirlsViewModel>();
            GirlsViewModel model = null;
            for (int n = 0; n < arryImg.Length; n++)
            {
                model = new GirlsViewModel();
                model.Image = arryImg[n];
                list.Add(model);
            }
            ViewBag.arryImg = list;
            //根据标签推荐
            ViewBag.InTags = BeautifulGirlsBll.LoadEntities(g => g.Tags.Contains(data.Tags)).Select(g => new GirlsViewModel
            {
                Id = g.Id,
                Tags = g.Tags,
                Title = g.Title,
                InTime = g.InTime
            }).AsNoTracking().OrderByDescending(g => g.InTime).Take(6).ToList(); ;
            #region 上一篇、下一篇
            //-上一篇、下一篇
            //用pre和next变量分别存放上一篇文章和下一篇文章的id号
            int pre = 0, next = 0, i = 0, j;
            //计算总记录数
            int num = BeautifulGirlsBll.LoadEntities(n => true).Count();
            int[] a = new int[num];
            var query = BeautifulGirlsBll.LoadEntities(n => true).Select(n => n.Id).ToArray();
            //将所有的文章id号全部放入一个数组中
            foreach (var item in query)
            {
                a[i] = Convert.ToInt32(item);
                i++;
            }
            //循环，获取上一篇和下一篇文章的ID号，分别放入变量pre和next中
            for (j = 0; j < num; j++)
            {
                if (a[j] == id)
                {
                    if (j != 0) pre = a[j - 1];//上一篇id
                    if (j != num - 1) next = a[j + 1];//下一篇文章id
                }
            }
            //获取上一篇文章的标题
            if (pre == 0)
            {
                ViewBag.preTags = "没有了";
                ViewBag.pre = id;
            }
            else
            {
                ViewBag.preTags = BeautifulGirlsBll.LoadEntities(n => n.Id == pre).Single().Tags;
                ViewBag.pre = pre;
            }
            //获取下一篇文章的标题
            if (next == 0)
            {
                ViewBag.nextTags = "没有了";
                ViewBag.next = id;
            }
            else
            {
                ViewBag.nextTags = BeautifulGirlsBll.LoadEntities(n => n.Id == next).Single().Tags;
                ViewBag.next = next;
            }
            #endregion
            return View();
        }
        
        #endregion   
        #region 添加美图的评论
        public ActionResult _GirlLeaveMsg()
        {
            return PartialView("_GirlLeaveMsg");
        }
        /// <summary>
        /// 添加美图的评论
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGirlLeaveMsg()
        {
            string ip = Request["ip"];
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            if (ForbiddenListBll.CheckBannd(Request["msg"]))
            {
                return Content("banned,提示：您所输入的信息包含禁用词！");
            }
            if (BlackListIPBll.CheckBlackListIp(ip))
            {
                return Content("ipNo,提示：您所在地IP已被受限！暂不能发表评论！如需操作,联系爽赞网管理人员");
            }
            else
            {
                LeaveMsg levaemsg = new LeaveMsg()
                {
                    PersonalUserId = userId,
                    IP = ip,
                    City = Request["city"],
                    Msg = Request["msg"],
                    MsgNum = null,
                    InTime = DateTime.Now,
                    ReplyId = 0,
                    GirlId = int.Parse(Request["girlId"])
                };
                if (LeaveMsgBll.Add(levaemsg) != null)
                {
                    return Content("ok,恭喜:评论提交成功");
                }
                else
                {
                    return Content("no,提示:评论提交失败！");
                }
            }
        } 
        #endregion
        #region 美图回复留言
        //美图回复留言
        public ActionResult AddGirlReplyMsg()
        {
            string ip = Request["ip"];
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            if (ForbiddenListBll.CheckBannd(Request["msg"]))
            {
                return Content("banned,提示：您所输入的信息包含禁用词！");
            }
            if (BlackListIPBll.CheckBlackListIp(ip))
            {
                return Content("ipNo,提示：您所在地IP已被受限！暂不能发表评论！如需操作,联系爽赞网管理人员");
            }
            else
            {
                LeaveMsg levaemsg = new LeaveMsg()
                {
                    PersonalUserId = userId,
                    IP = ip,
                    City = Request["city"],
                    Msg = Request["msg"],
                    MsgNum = null,
                    InTime = DateTime.Now,
                    ReplyId = int.Parse(Request["replyId"]),//变化点
                    GirlId = int.Parse(Request["girlId"])
                };
                if (LeaveMsgBll.Add(levaemsg) != null)
                {
                    return Content("ok,恭喜:评论提交成功");
                }
                else
                {
                    return Content("no,提示:评论提交失败！");
                }
            }
        } 
        #endregion
        #region 美图的评论数据加载
        /// <summary>
        /// 美图的评论数据加载
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadGirleaveMsg()
        {
            int girlsId = int.Parse(Request["girlId"]);
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int total = 0;
            var msg = LeaveMsgBll.LoadEntities(m => true);
            var girl = BeautifulGirlsBll.LoadEntities(n => n.Id > 0).Select(n => new { n.Id});
            var user = PersonalUserBll.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id, u.Head });
            //-------------请求发布的评论---------------------------------- 
            var data = from m in msg
                       join u in user on m.PersonalUserId equals u.Id into um
                       from umi in um.DefaultIfEmpty()
                       join n in girl on m.GirlId equals n.Id
                       where m.GirlId == girlsId
                       where m.ReplyId == null || m.ReplyId == 0
                       select new Publish()
                       {
                           Id = m.Id,
                           Msg = m.Msg,
                           City = m.City,
                           UserName = umi != null ? umi.UName : "" + m.Id + "" + "爽赞网友",//用户名
                           UserNameImg = umi.Head != null ? ("/Content/Img/" + umi.Head) : null,//用户头像
                           InTime = m.InTime,
                           Tip = m.Tip == null ? 0 : m.Tip,
                           Stamp = m.Stamp == null ? 0 : m.Stamp,
                       };
            total = data.Count();
            var alldata = data.AsNoTracking().OrderByDescending(d => d.InTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            List<Publish> list = WebHelper.ToListTimeSpan(alldata);
            //-------------------------- //回复的数据---------------------------------------------
            string pubStrNav = LaomaPager.ShowPageNavigate(pageSize, pageIndex, total);
            //回复的数据
            var replyData = LeaveMsgBll.GetGirlLeaveMsgData(girlsId).Select(n => new Reply
            {
                SelfId = n.SelfId,
                ReplyId = n.ReplyId,
                ReplyContent = n.ReplyContent,
                ReplyCity = n.ReplyCity,
                ReplyName = n.ReplyName,
                ReplyUserImg = n.ReplyUserImg,//用户头像
                ReplyInTime = n.ReplyInTime,
                ReplyTip = n.ReplyTip,
                ReplyStamp = n.ReplyStamp,
            }).ToList();
            List<Reply> replyList = WebHelper.ToReplyStrTimeSpan(replyData);
            var result = new { Data = list, replyData = replyList, PubStrNav = pubStrNav, Total = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region 福利美图浏览量
        public ActionResult Views(int id)
        {
            var girl = BeautifulGirlsBll.LoadEntities(n => n.Id == id).FirstOrDefault();
            if (girl != null)
            {
                girl.Views = girl.Views + 1;
                if (BeautifulGirlsBll.Update(girl))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("noExist");
            }
        }  
        #endregion
    }
}
