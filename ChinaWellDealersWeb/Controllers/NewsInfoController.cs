 
using BLL;
using Common;
using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Data.Entity;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class NewsInfoController : Controller
    {
        #region 根据属性创建对象
        public ISeeNewsBll SeeNewsBll { get; set; }
        public INewsBll NewsBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        public ILeaveMsgBll LeaveMsgBll { get; set; }
        public IForbiddenListBll ForbiddenListBll { get; set; }
        public IPersonalUserBll PersonalUserBll { get; set; }
        IBlackListIPBll BlackListIPBll { get; set; }
        IAdvertisementBll AdvertisementBll { get; set; }
        IWonderfulTxtImgBll WonderfulTxtImgBll { get; set; }
        #endregion
        #region 新闻主页面的数据读取
        [OutputCache(Duration = 60)]
        public ActionResult News()
        {
            //--------------轮播区数据5条最新数据-----------
            ViewBag.RedLight = SeeNewsBll.LoadEntities(n => n.Type == "7").AsNoTracking().OrderByDescending(n => n.Intime)
                                         .Take(5).ToList();                    
            //------------------5条置顶区------------------------
            ViewBag.FiveStick = SeeNewsBll.LoadEntities(n => n.Type == "10").AsNoTracking().OrderByDescending(n => n.Intime)
                                                 .Take(6).ToList();
            ViewBag.NewGame = GetAllNews("1");
            ViewBag.HotGame = GetAllNews("2");
            ViewBag.Industry = GetAllNews("3");
            
            ViewBag.SeeNewsNewGame = SeeNewsBll.LoadEntities(n => n.Type == "1").AsNoTracking().OrderByDescending(n => n.Intime).Take(1).ToList();
            ViewBag.SeeNewsHotGame = SeeNewsBll.LoadEntities(n => n.Type == "2").AsNoTracking().OrderByDescending(n => n.Intime).Take(1).ToList();
            ViewBag.SeeNewsIndustry = SeeNewsBll.LoadEntities(n => n.Type == "3").AsNoTracking().OrderByDescending(n => n.Intime).Take(1).ToList();
            ViewBag.NewsNewGameMsg = NewsBll.GetNewAllMsgImg("1");//1:新游在线(看新闻6条中是最后一条的一天数据)
            ViewBag.NewsHotGameMsg = NewsBll.GetNewAllMsgImg("2");//2热游动态
            ViewBag.NewsIndustryMsg = NewsBll.GetNewAllMsgImg("3");//2热游动态
            ViewBag.NewestNewsPC = GetNewestNewsPC();//获取最新的资讯15条(pc主机)           
            ViewBag.NewsPcMsgImg = NewsBll.GetAllNewsPcMsg("1");//板块推荐  留言数
            ViewBag.DirectSeeding = TwoSeeNewsImg("5");//2、2张带图的推荐位---直播热点
            ViewBag.MobileGame = TwoSeeNewsImg("6");//2、2张带图的推荐位---手游
            ViewBag.DirectRec = NewsBll.GetAllTypeNews("1", "5", 5);//直播版块最新5条数据
            ViewBag.MobileRec = NewsBll.GetAllTypeNews("1", "6", 5);//手游版块最新5条数据
            //-------------------娱乐八卦读取--------------------------
            ViewBag.HappNews = NewsBll.GetAllHappyNews();         
            return View();
        }
        #endregion
        #region 看新闻红字头条和灰字头条接口
        public ActionResult RedAndGrayHomePage()
        {
            var redData = SeeNewsBll.GetSeeNews("8", 2);
            var grayData = SeeNewsBll.GetSeeNews("9", 4);
            var result = new { redData = redData, grayData = grayData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion       
        #region 获取最新的5条数据
        //新游在线  热游、产业：1，2,3
        private List<FrontNews> GetAllNews(string type)
        {
            List<FrontNews> list = NewsBll.LoadEntities(n => n.Type == type&&n.State=="1").Select(n => new FrontNews()
            {
                Id = n.Id,
                Title = n.Title,
                EditTitle = n.EditTitle,
                InTime = n.InTime
            }).AsNoTracking().OrderByDescending(n => n.InTime).Take(6).ToList();                                               
            return list;
        }
        #endregion  
        #region 获取最新的资讯15条(pc主机)
        public List<FrontNews> GetNewestNewsPC()
        {
            List<FrontNews> list = NewsBll.LoadEntities(n => n.Type == "4"&&n.State=="1")
                                               .Select(n => new FrontNews()
                                               {
                                                   Id = n.Id,
                                                    Title = n.Title,
                                                   EditTitle = n.EditTitle,
                                                   InTime=n.InTime
                                               }).OrderByDescending(n => n.InTime).Take(15).AsNoTracking().ToList();
            return list;
        }
        #endregion
        #region 2张带图的推荐位---直播热点--手游
        /// <summary>
        /// 2张带图的推荐位---直播热点--手游
        /// </summary>
        /// <returns></returns>
        private List<SeeNews> TwoSeeNewsImg(string type)
        {
           var list = SeeNewsBll.LoadEntities(n => n.Type == type).OrderByDescending(n => n.Intime).Take(2).AsNoTracking().ToList();
                                                      
            return list;
        }
        #endregion
        #region 爽赞头部
        public ActionResult ShuangzanHeader()
        {
            ViewData["smallImg"] = AdvertisementBll.GetAllTypeAdvert("2", 9);
            ViewData["advert"] = AdvertisementBll.GetAllTypeAdvert("3", 2);
            return PartialView("ShuangzanHeader");
        }
        #endregion  
        #region 新闻各种类型的详情   
        public ActionResult NewsDetail(int id)
        {
            ViewData.Model = GetNewsDetails(id);                           
            #region 上一篇、下一篇


            //-上一篇、下一篇
            //用pre和next变量分别存放上一篇文章和下一篇文章的id号
            int pre = 0, next = 0, i = 0, j;
            //计算总记录数
            int num = NewsBll.LoadEntities(n =>n.Id>0).Count();
            int[] a = new int[num];
            var query = NewsBll.LoadEntities(n => true).Select(n => n.Id).ToArray();
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
                ViewBag.preTitle =NewsBll.LoadEntities(n =>n.Id==pre).Single().Title;
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
                ViewBag.nextTitle = NewsBll.LoadEntities(n => n.Id == next).Single().Title;
                ViewBag.next = next;
            }
            #endregion
            ViewBag.SameKeys = NewsBll.TheSameKeyWords(id);           
            return View();
        }
        #endregion
        #region 右侧新闻
           [OutputCache(Duration = 30)]
        public ActionResult _RightNews()
        {
            //---------------资讯排行---------------------
            ViewData["ViewTopNews"] = NewsBll.GetAllPageViewTopNews();                  
            //---------------精彩图文------------------------       
          ViewBag.WonderfulSeeNews = WonderfulTxtImgBll.LoadEntities(n =>true).OrderByDescending(n =>n.InTime)
                   .Take(4).AsNoTracking().ToList();
           //----------------最赞攻略------------------------
          ViewBag.NewestRaiders = UserRaidersBll.GetMostGreatRaiders();
            //----------------最新福利--------------
          ViewBag.NewsetGameDemo = GameDemoBll.GetNewestGameDemo();
         
            return PartialView("_RightNews");
        }
        #endregion       
      
        #region 新闻详情
        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private NewsViewModel GetNewsDetails(int id)
        {
            return NewsBll.GetNewsDetails(id).Select(n => new NewsViewModel
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum =n.ViewNum,//浏览量
                Source = n.Source,
                Title = n.Title,
                 Editor=n.Editor,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                EditTitle = n.EditTitle,               
                Memo=n.Memo,
                Kewords = n.Kewords
            }).FirstOrDefault();
        }
        #endregion
        #region 新游在线列表
        [OutputCache(Duration=60)]
        public ActionResult MoreNewGameNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.NewGameOnline;
            string t = type.ToString();
            ViewData["NewGame"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }
       
        #endregion
        #region 热游动态列表列表
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult MoreHotGameNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.HotGameDynamic;
            string t = type.ToString();
            ViewData["HotGame"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }  
        #endregion
        #region 产业资讯列表
        [OutputCache(CacheProfile="ItemConfigCache")]
        public ActionResult MoreIndustryNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.IndustryMsg;
            string t = type.ToString();
            ViewData["Industry"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }     
        #endregion
        #region PC主机列表   
        [OutputCache(Duration=60)]
        public ActionResult MorePcNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.PCHostNews;
            string t = type.ToString();
            ViewData["PCHostNews"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }       
        #endregion
        #region 直播热点列表
         [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult MoreDirectHotNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.DirectHot;
            string t = type.ToString();
            ViewData["DirectHot"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }
        
        #endregion
        #region 手游列表
         [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult MoreMobileGameNews()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            int type = (short)NewsType.MobileGame;
            string t = type.ToString();
            ViewData["MobileGame"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Type = n.Type,
                InTime = n.InTime,
                ViewNum = n.ViewNum,
                Title = n.Title,
                EditTitle = n.EditTitle,
                Msg = n.Msg,
                MsgNum = n.MsgNum,
                Memo = n.Memo
            }).ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return View();
        }        
        #endregion
        #region 娱乐八卦列表
        [OutputCache(Duration=60)]
         public ActionResult FunNews()
         {
             int pageSize = int.Parse(Request["pageSize"] ?? "30");
             int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
             int total = 0;
             int type = (short)NewsType.HappyNews;
             string t = type.ToString();

             ViewData["HappyNews"] = NewsBll.GetMoreNews(pageSize, pageIndex, out total, t).Select(n => new NewsViewModel()
             {
                 Id = n.Id,
                 Type = n.Type,
                 InTime = n.InTime,
                 ViewNum = n.ViewNum,
                 Title = n.Title,
                 EditTitle = n.EditTitle,
                 Msg = n.Msg,
                 MsgNum =n. MsgNum,
                 Memo = n.Memo
             }).ToList();
             ViewData["pageIndex"] = pageIndex;
             ViewData["pageSize"] = pageSize;
             ViewData["Total"] = total;        
             return View();
         }        
        #endregion    
        #region 娱乐八卦详情
         public ActionResult FunNewsDetail(int id)
         {
           
            var nn = GetNewsDetails(id);
            ViewData.Model = nn;                
             //-----相关推荐
             ViewData["recTopSixFunNews"] = NewsBll.GetRelatedFunNews(id, 6, "0");
             #region 上一篇、下一篇
             //-上一篇、下一篇
             //用pre和next变量分别存放上一篇文章和下一篇文章的id号
             int pre = 0, next = 0, i = 0, j;
             //计算总记录数
             int num = NewsBll.LoadEntities(n => true).Count();
             int[] a = new int[num];
             var query = NewsBll.LoadEntities(n => true).Select(n => n.Id).ToArray();
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
                 string edtitTitle = NewsBll.LoadEntities(n => n.Id == pre).Single().EditTitle;
                 //拿了原标题
                 if (edtitTitle != null)
                 {
                     ViewBag.preTitle = edtitTitle;
                 }
                 else {
                     ViewBag.preTitle = NewsBll.LoadEntities(n => n.Id == pre).Single().Title;
                 }                
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

                 string edtitTitle = NewsBll.LoadEntities(n => n.Id == next).Single().EditTitle;
                 //拿了原标题
                 if (edtitTitle != null)
                 {
                     ViewBag.nextTitle = edtitTitle;
                 }
                 else
                 {
                     ViewBag.nextTitle = NewsBll.LoadEntities(n => n.Id == next).Single().Title;
                 }                             
                 ViewBag.next = next;
             }
             #endregion
             return View();
         }
        #endregion
        #region 新闻列表搜索结果
         [OutputCache(Duration = 30)]
         public ActionResult NewsSearchResult(string key)
         {
             int pageSize = int.Parse(Request["pageSize"] ?? "15");
             int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
             int total = 0;
             var list = NewsBll.GetSearchNews(pageSize, pageIndex, out total, key).Select(n => new NewsViewModel()
             {
                 Id = n.Id,
                 Type = n.Type,
                 InTime = n.InTime,
                 ViewNum = n.ViewNum,//浏览量
                 EditTitle = n.EditTitle,
                 Title = n.Title,
                 Msg = n.Msg,
                 MsgNum = n.MsgNum,//留言数
                 Memo = n.Memo
             }).ToList();
             TempData["SearchTitle"] = key;
             ViewData["SearchResult"] = list;
             ViewData["pageIndex"] = pageIndex;
             ViewData["pageSize"] = pageSize;
             ViewData["Total"] = total;
             return View();
         }
         #endregion
        #region 评论部分页面
         public ActionResult _LeaveMsg()
         {

             return PartialView("_LeaveMsg");
         }
         public ActionResult AddLeaveMsg()
         {
             string guid = Request["userId"];
             string ip = Request["ip"];
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
                     IP =ip,
                     City = Request["city"],
                     Msg =  Request["msg"],
                     MsgNum = null,
                     InTime = DateTime.Now,
                     ReplyId = 0,
                     NewsId = int.Parse(Request["newsId"])
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
        #region 新闻评论回复
         public ActionResult AddNewsReplyMsg()
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
                 return Content("ipNo,提示：您所在地IP已被受限！暂不能发表评论！如需操作,联系爽赞网管理人员");
             }
             else
             {
                 LeaveMsg levaemsg = new LeaveMsg()
                 {
                     PersonalUserId = userId,
                     IP =ip ,
                     City = Request["city"],
                     Msg = msg,
                     MsgNum = null,
                     InTime = DateTime.Now,
                     ReplyId = int.Parse(Request["replyId"]),//变化点
                     NewsId = int.Parse(Request["newsId"])//变化点
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
        #region 新闻评论的数据加载
         /// <summary>
         /// 新闻评论的数据加载
         /// </summary>
         /// <returns></returns>
         public ActionResult LoadNewsLeaveMsg()
         {
             int newsId=int.Parse(Request["newsId"]);
             int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
             int pageSize = int.Parse(Request["pageSize"] ?? "20");
             int total = 0;
             var msg = LeaveMsgBll.LoadEntities(m => true);
             var news = NewsBll.LoadEntities(n => n.Id > 0).Select(n => new { n.Id});
             var user = PersonalUserBll.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id, u.Head });
             //-------------请求发布的评论---------------------------------- 
             var data = from m in msg
                        join u in user on m.PersonalUserId equals u.Id into um
                        from umi in um.DefaultIfEmpty()
                        join n in news on m.NewsId equals n.Id
                        where m.NewsId == newsId
                        where m.ReplyId == null || m.ReplyId == 0
                        select new Publish()
                        {
                            Id = m.Id,
                            Msg = m.Msg,
                            City = m.City,
                            UserName = umi != null ? umi.UName : "" + m.Id + "" + "爽赞网友",//用户名
                            UserNameImg = umi.Head != null ? ("/Content/Img/"+umi.Head) : null,//用户头像
                            InTime = m.InTime,
                            Tip = m.Tip == null ? 0 : m.Tip,
                            Stamp = m.Stamp == null ? 0 : m.Stamp,
                        };
           var  alldata=data.AsNoTracking().OrderByDescending(d => d.InTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
             total = data.Count();
             List<Publish> list = WebHelper.ToListTimeSpan(alldata);
             //-------------------------- //回复的数据---------------------------------------------
             string pubStrNav = LaomaPager.ShowPageNavigate(pageSize, pageIndex, total);

             //回复的数据
             var replyData = LeaveMsgBll.GetNewsLeaveMsgData(newsId).Select(n => new Reply
             {
                 SelfId = n.SelfId,
                 ReplyId = n.ReplyId,
                 ReplyContent = n.ReplyContent,
                 ReplyCity = n.ReplyCity,
                 ReplyName = n.ReplyName,
                 ReplyUserImg =  n.ReplyUserImg,//用户头像
                 ReplyInTime = n.ReplyInTime,
                 ReplyTip = n.ReplyTip,
                 ReplyStamp = n.ReplyStamp,
             }).ToList();
             List<Reply> replyList = WebHelper.ToReplyStrTimeSpan(replyData);
             var result = new { Data = list, replyData = replyList, PubStrNav = pubStrNav,Total=total };
             return Json(result, JsonRequestBehavior.AllowGet);
         }           
         #endregion
        #region 新闻浏览
         public ActionResult Views(int id)
         {
             var news = NewsBll.LoadEntities(n => n.Id == id).FirstOrDefault();
             if (news != null)
             {
                 news.Views = news.Views==null?1:news.Views+1;
                 if (NewsBll.Update(news))
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
        #region 留言点赞，踩
         public ActionResult LeaveMsgPlusOne(int id, string falg)
         {
             var msg = LeaveMsgBll.LoadEntities(m => m.Id == id).FirstOrDefault();
             if (msg != null)
             {
                 if (falg == "tip")
                 {
                     msg.Tip = msg.Tip==null?1:msg.Tip + 1;
                 }
                 if (falg == "stamp")
                 {
                     msg.Stamp = msg.Stamp==null?1:msg.Stamp + 1;
                 }
                 if (LeaveMsgBll.Update(msg))
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
