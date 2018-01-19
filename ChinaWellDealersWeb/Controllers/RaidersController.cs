using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models;
using Models.Enum;
using System.Data.Entity;
using Models.MapModel;
using Common;
namespace ShuangZan.Web.Controllers
{
    /// <summary>
    /// 用户攻略控制器
    /// </summary>
     [LoginCheckFilterAttribute(IsCheck = false)]
    public class RaidersController : Controller
    {
        #region 属性注入
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        ILeaveMsgBll LeaveMsgBll { get; set; }
        IPersonalUserBll PersonalUserBll { get; set; }
        IForbiddenListBll ForbiddenListBll { get; set; }
        IBlackListIPBll BlackListIPBll { get; set; }
        public INewsBll NewsBll { get; set; }
        ISeoBll SeoBll { get; set; } 
        #endregion
        #region 最新攻略  
         [OutputCache(CacheProfile="ItemConfigCache")]  
         [ValidateInput(false)]
        public ActionResult RaidersIndex()
        {                   
           
            ViewBag.RecSeoGame = SeoBll.GetSeoData(3, "3");  
            return View();
        }
        #endregion
        #region 热游攻略接口
         public ActionResult GetHotRaiders()
         {
             var data = UserRaidersBll.GetHotRaiders();
             return Json(data, JsonRequestBehavior.AllowGet);
         } 
         #endregion
        #region 最新攻略列表json
         [HttpPost]
         public ActionResult GetRaidersList()
         {

             int pageSize = Request["pageSize"] == null ? 50 : int.Parse(Request["pageSize"]);

             int pageIndex = int.Parse(Request["pageIndex"] ?? "1");

             int total = 0;
             int state = (short)CheckState.Pass;
             string t = state.ToString();
             var list = UserRaidersBll.LoadPageEntities(pageSize, pageIndex, out total, r => r.State == t, false, r => r.InTime).Select(r => new { r.Id, r.InTime, r.EditTitle, r.Title,r.GameName }).ToList();
             //Guid guid = Guid.NewGuid();
             //object obj = Common.CacheHelper.Get(guid.ToString());          
           //  List<UserRaiders> list = null;
             //if (obj == null)
             //{
              
             //    Common.CacheHelper.WriteCache(guid.ToString(), list, DateTime.Now.AddMinutes(10));
             //}
             //else
             //{
             //    list = obj as List<UserRaiders>;
             //}
             var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, total);
             var result = new { Data = list, NavStr = NavStr };
             return Json(result, JsonRequestBehavior.AllowGet);

         }

         #endregion      
        #region 搜索结果页
        public ActionResult RaidersSearch(string key)
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "50");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;          
            int state = (short)CheckState.Pass;
            string t = state.ToString();
            var list = UserRaidersBll.LoadPageEntities(pageSize, pageIndex, out total, r => r.State == t && r.GameName.Contains(key.Trim()), false, r => r.InTime).ToList();
            TempData["SerarchGameName"] = key;
            ViewData["SearchRaidersResult"] = list;
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;        
            return View();
        }
        #endregion
        #region 攻略详情
        public ActionResult RaidersDetail(int id)
        {
            ViewData.Model = GetRaidersDetails(id);
            //当前月份排行榜最高的十条数据        
            #region 上一篇、下一篇
            //-上一篇、下一篇
            //用pre和next变量分别存放上一篇文章和下一篇文章的id号
            int pre = 0, next = 0, i = 0, j;
            //计算总记录数
            int num = UserRaidersBll.LoadEntities(n => true).Count();
            int[] a = new int[num];
            var query = UserRaidersBll.LoadEntities(n => true).Select(n => n.Id).ToArray();
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
                ViewBag.preTitle = "没有了";
                ViewBag.pre = id;
            }
            else
            {
                ViewBag.preTitle = UserRaidersBll.LoadEntities(n => n.Id == pre).Single().Title;
                ViewBag.pre = pre;
            }
            //获取下一篇文章的标题
            if (next == 0)
            {
                ViewBag.nextTitle = "没有了";
                ViewBag.next = id;
            }
            else
            {
                ViewBag.nextTitle = UserRaidersBll.LoadEntities(n => n.Id == next).Single().Title;
                ViewBag.next = next;
            }
            #endregion
            //相关推荐
            ViewBag.RelatedRaiders = UserRaidersBll.RelatedRaiders(6,id);          
            return View();
        }
        #endregion
        #region 调用业务层攻略详情
        public UserRaidersViewModel GetRaidersDetails(int id)
        {
           
                UserRaidersViewModel raiders = UserRaidersBll.GetRaidersDetails(id).Select(r => new UserRaidersViewModel()
                {
                    Id = r.Id,
                    InTime = r.InTime,
                    ViewsNum = r.ViewsNum,
                    Source = r.Source,
                    Title = r.Title,
                    Editor = r.Editor,
                    Msg = r.Msg,
                    MsgNum = r.MsgNum,
                    EditTitle = r.EditTitle,
                    Memo=r.Memo,
                    KeyWords = r.KeyWords
                }).FirstOrDefault();                   
            return raiders;

        }

        #endregion        
        #region 攻略的右侧数据
        public ActionResult _RightRaiders()
        {
            //当前月份排行榜最高的十条数据
            ViewData["CurrentMonthTopTen"] = UserRaidersBll.GetNewestRaiders();          
            //右侧数据
            var otherController = DependencyResolver.Current.GetService<NewsInfoController>();           
            ViewData["ViewTopNews"] = NewsBll.GetAllPageViewTopNews();          
            ViewBag.NewsetGameDemo = GameDemoBll.GetNewestGameDemo();         
            return PartialView("_RightRaiders");
        }
        #endregion
        #region 攻略评论分部视图
        public ActionResult _RaidersLeaveMsg()
        {

            return PartialView("_RaidersLeaveMsg");
        }          
        #endregion
        #region 攻略评论
        public ActionResult AddRaiderContent()
        {
            string msg = Request["msg"];
            string ip = Request["ip"];
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            if (ForbiddenListBll.CheckBannd(msg))
            {
                return Content("banned,提示：您所输入的信息包含禁用词！");
            }
            if (BlackListIPBll.CheckBlackListIp(ip))
            {
                return Content("ipNo,提示：您所在地IP已被受限！暂不能发表评论！如需操作,联系爽赞网管理人员!");
            }
            else
            {
                LeaveMsg levaemsg = new LeaveMsg()
                {
                    PersonalUserId = userId,
                    IP = ip,
                    City = Request["city"],
                    Msg = msg,
                    MsgNum = null,
                    InTime = DateTime.Now,
                    ReplyId = 0,
                    UserRaidersId = int.Parse(Request["raidersId"])
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
        #region 攻略评论回复
        public ActionResult AddNewsReplyMsg()
        {
            string ip = Request["ip"];
            string msg = Request["msg"];
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            if (ForbiddenListBll.CheckBannd(msg))
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
                    Msg = msg,
                    MsgNum = null,
                    InTime = DateTime.Now,
                    ReplyId = int.Parse(Request["replyId"]),//变化点
                    UserRaidersId = int.Parse(Request["raidersId"])//变化点
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
        #region 用户攻略的评论数据加载
        /// <summary>
        /// 用户攻略的评论
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadRaiderLeaveMsg()
        {
            int raidersId = int.Parse(Request["raidersId"]);
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int total = 0;
            var msg = LeaveMsgBll.LoadEntities(m => true);
            var raiders = UserRaidersBll.LoadEntities(n => n.Id > 0).Select(n => new { n.Id });
            var user = PersonalUserBll.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id, u.Head });
            //-------------请求发布的评论---------------------------------- 
            var data = from m in msg
                       join u in user on m.PersonalUserId equals u.Id into um
                       from umi in um.DefaultIfEmpty()
                       join n in raiders on m.UserRaidersId equals n.Id
                       where m.UserRaidersId == raidersId
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
            var replyData = LeaveMsgBll.GetRaidersLeaveMsgData(raidersId).Select(n => new Reply
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
        #region 攻略浏览
        public ActionResult Views(int id)
        {
            var raiders = UserRaidersBll.LoadEntities(n => n.Id == id).FirstOrDefault();
            if (raiders != null)
            {
                raiders.Views = raiders.Views == null ? 1 : raiders.Views + 1;
                if (UserRaidersBll.Update(raiders))
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
