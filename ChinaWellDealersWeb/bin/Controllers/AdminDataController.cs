using BLL;
using Common;
using IBLL;
using Models;
using Models.MapModel;
using Models.Params;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data.Entity;
using System.Text;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class AdminDataController : BaseController//Controller//   
    {
        #region 属性注入
        public IAdvertisementBll AdvertisementBll { get; set; }
        public IBlackListIPBll BlackListIPBll { get; set; }
        public IHomePageBll HomePageBll { get; set; }
        public ISeeNewsBll SeeNewsBll { get; set; }
        public ISeoBll SeoBll { get; set; }
        public IWonderfulTxtImgBll WonderfulTxtImgBll { get; set; }
        public IUserInfoBll UserInfoBll { get; set; }
        public INewsBll NewsBll { get; set; }
        public ICompanyBll CompanyBll { get; set; }
        public IPackageBll PackageBll { get; set; }//礼包
        public IPackageCardBll PackageCardBll { get; set; }
        public IRechargeBll RechargeBll { get; set; }//厂商充值明细
        public ILeaveMsgBll LeaveMsgBll { get; set; }
        public IPersonalUserBll PersonalUserBll { get; set; }
        public IUserMessageBll UserMessageBll { get; set; }//用户充值及资料完善
        public IOpenServiceBll OpenServiceBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IDemoUserBll DemoUserBll { get; set; }
        public IGameDemoRechargeBll GameDemoRechargeBll { get; set; }
        IAuditLogBll AuditLogBll { get; set; }
        #endregion
        #region 后台管理默认页面
        public ActionResult Default()
        {

            ViewData.Model = GetCurrentAdmin();
            return View();
        }
        #endregion
        #region 菜单权限处理
        public ActionResult TestMenu()
        {
            List<ActionInfo> data = null;
            Guid  actionKey=Guid.NewGuid();
            object oj = Common.CacheHelper.Get(actionKey.ToString());
           if (oj == null)
           {
               var loginAdmin = GetCurrentAdmin();
               short delNormal = (short)Models.Enum.DelFlagEnum.Normal;
               //根据当前登录用户的id查出当前用户的信息
               var user = UserInfoBll.LoadEntities(u => u.Id == loginAdmin.Id && u.State == "1")
                   .FirstOrDefault();
               if (user == null)
               {
                   Response.Redirect("/Home/Error");
               }
               short menuType = (short)Models.Enum.ActionInfoTypeEnum.MenuAction;
               //拿到用户的所有的权限
               //通过角色拿到所有 的权限
               var temp = (from r in user.RoleInfo
                           from a in r.ActionInfo
                           where a.ActionType == menuType
                           where a.DelFlag == delNormal
                           select a).ToList();

               //通过特殊中间表拿到权限数据  
               var userTemp = (from r in user.R_UserInfo_ActionInfo
                               where r.IsPass == true
                               select r.ActionInfo).ToList();
               //把所有角色权限里面的特殊拒绝的权限清除掉
               temp.AddRange(userTemp);
               //TO移除拒绝的
               var userNoPass = (from r in user.R_UserInfo_ActionInfo
                                 where r.IsPass == false
                                 select r.ActionInfo.Id).ToList();
               data = (from t in temp
                           where !userNoPass.Contains(t.Id)
                           where t.ActionType == menuType
                       select new ActionInfo { Id = t.Id, ActionName = t.ActionName, ActionType=t.ActionType,
                                               MenuName = t.MenuName,
                                               Url=t.Url
                       }).ToList();
               Common.CacheHelper.WriteCache(actionKey.ToString(), data, DateTime.Now.AddDays(1));
           }
           else { 
            data=oj  as  List<ActionInfo>;
           }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 添加新闻
        public ActionResult NewsAddShow(int id)
        {

            var cpy = CompanyBll.LoadEntities(n => n.Id == id).FirstOrDefault();
            ViewData.Model = cpy;
            return View();
        }
        [ValidateInput(false)]
        public ActionResult NewsAdd(FormCollection form)
        {
            int companyId = int.Parse(Request["companyId"].ToString());
            string type = Request["type"].ToString();
            try
            {
                if (form["title"] != "" && form["msg"] != "" && form["editor"] != "")
                {
                    News news = new News()
                    {
                        CompanyId = companyId,
                        Title = form["title"],
                        EditTitle = form["editTitle"],
                        Source = form["source"],
                        Editor = form["editor"],
                        Kewords = form["kewords"],
                        Memo = form["memo"],//摘要
                        Msg = form["msg"],
                        Type = type,
                        Game = form["game"],
                        InTime = DateTime.Now,
                        State = "2",
                        Rec_Forum_Index = "0",
                        Rec_Forum_Time = DateTime.Now,
                        Rec_Hot_Index = "0",
                        Rec_Hot_Time = DateTime.Now,
                        CheckName = null,
                        AddedBy = null,
                        Views = 0,

                        LeaveMsgId = 2

                    };
                    if (news != null)
                    {
                        NewsBll.Add(news);
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("empty");
                }

            }
            catch (Exception)
            {

                return Content("ex");
            }
        }
        #endregion
        #region 添加礼包
        public ActionResult AddPackageShow(int id)
        {
            ViewData.Model = CompanyBll.LoadEntities(p => p.Id == id).FirstOrDefault();
            return View();
        }
        public ActionResult AddPackage(int companyId, FormCollection form)
        {
            string type = form["type"].ToString();
            string state;
            if (type == "3")//独家礼包
            {
                state = "2";
            }
            else
            {
                state = "3";//无需审核
            }
            Package package = new Package()
            {
                CompanyId = companyId,
                GameName = form["game"],
                ServerName = form["servername"],//servername
                Type = type,//礼包类型
                GiftName = form["giftName"],
                StartDate = DateTime.Parse(form["startDay"].ToString()),
                EndDate = DateTime.Parse(form["endDay"].ToString()),
                Url = form["url"],
                InTime = DateTime.Now,
                Msg = form["msg"],
                Memo = form["memo"],
                State = state,
                Rec = "0",
                Rec_Hot = "0",
                Rec_Index = "0",
                Rec_Hot_Time = DateTime.Now,
                Rec_Time = DateTime.Now,
                Rec_Index_Time = DateTime.Now,
            };
            if (package != null)
            {
                if (PackageBll.Add(package) != null)
                {
                    string code = form["code"];
                    string[] splitCode = code.Trim().Split('\n');
                    PackageCard card = null;
                    for (int i = 0; i < splitCode.Length; i++)
                    {
                        card = new PackageCard();
                        card.PackageId = package.Id;
                        card.UserId = 0;
                        card.InTime = DateTime.Now;
                        if (splitCode[i] == "" || splitCode[i] == null)
                        {
                            continue;
                        }
                        card.Code = splitCode[i];
                        PackageCardBll.Add(card);
                    }
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("empty");
            }
        }
        #endregion
        #region 厂商充值明细表及模糊查询
        public ActionResult Cpy_Recharge()
        {
            return View();
        }
        public ActionResult GerRechargeInfos(DateTime? StartDate, DateTime? EndDate)
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            string cpyName = Request["cpyName"];
            string comboName = Request["comboName"];
            var temp = RechargeBll.LoadEntities(r => r.Id > 0);
            var cpy = CompanyBll.LoadEntities(c => c.Id > 0).Select(c => new { c.Id, c.SystemName });
            var data = from t in temp
                       join c in cpy on t.CompanyId equals c.Id
                       select new { t.Id, t.ComboName, t.InTime, t.Money, t.Remark, t.Num, c.SystemName, t.Used, LeftNum = t.Num - t.Used };

            if (!string.IsNullOrEmpty(cpyName))
            {
                data = data.Where(d => d.SystemName.Contains(cpyName));
            }
            if (!string.IsNullOrEmpty(comboName))
            {
                data = data.Where(d => d.ComboName.Contains(comboName));
            }
            if (StartDate.HasValue || EndDate.HasValue)
            {
                EndDate = EndDate.Value.AddDays(1);
                data = data.Where(d => d.InTime >= StartDate && d.InTime <= EndDate);
            }
            int total = data.Count();

            var result = new { total = total, rows = data.OrderByDescending(d => d.InTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 新闻列表
        public ActionResult NewsPagingList()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }
        public ActionResult NewsPaging()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            string title = Request["title"];
            string cpyName = Request["cpyName"];
            string newsType = Request["newsType"];
            string checkState = Request["checkState"] == null ? "2" : Request["checkState"]; ;
            //三表关联拿到相应的id
            var allMsg = LeaveMsgBll.LoadEntities(l => l.Id > 0)
                                    .Select(l => new { l.NewsId });
            var allNews = NewsBll.LoadEntities(n => true)
                                 .Select(n => new { n.Id, n.InTime, n.CompanyId, n.Game, n.Title, n.State, n.Type, n.CheckName, n.Views });
            var cpy = CompanyBll.LoadEntities(c => c.Id > 0)
                                .Select(c => new { c.Id, c.SystemName });
            var data = (from n in allNews
                        join c in cpy on n.CompanyId equals c.Id
                        join m in allMsg on n.Id equals m.NewsId into nn
                        from nni in nn.DefaultIfEmpty()
                        let MsgNum = (from mm in allMsg where n.Id == mm.NewsId select mm.NewsId).Count()
                        select new
                        {
                            n.Id,
                            n.InTime,
                            n.Title,
                            n.Type,
                            n.Views,
                            n.State,
                            c.SystemName,
                            n.Game,
                            n.CheckName,
                            MsgNum = MsgNum != null ? MsgNum : 0
                        }).Distinct();
            if (!string.IsNullOrEmpty(title))
            {
                data = data.Where(d => d.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(cpyName))
            {
                data = data.Where(d => d.SystemName.Contains(cpyName));
            }
            if (!string.IsNullOrEmpty(newsType))
            {
                data = data.Where(d => d.Type.Contains(newsType));
            }
            if (!string.IsNullOrEmpty(checkState))
            {
                data = data.Where(d => d.State.Contains(checkState));
            }
            var allData = data.OrderByDescending(d => d.InTime)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            int total = data.Count();
            var result = new { total = total, rows = allData };

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 新闻单一审核状态
        public ActionResult CheckState(int id, string checkIsPass, string title, string gameName, string currentAdmin)
        {
            string reason = Request["reason"];
            // <option value="2">待审核</option>
            //<option value="">全部</option>
            //<option value="0">未通过</option>
            //<option value="1">已通过</option> 
            News news = new News() { Id = id };
            if (news != null)
            {
                news = NewsBll.LoadEntities(n => n.Id == id).FirstOrDefault();
                if (checkIsPass == "1")
                {
                    news.State = "1";
                    news.CheckName = currentAdmin;
                    NewsBll.Update(news);
                    return Content("ok");
                }
                else if (checkIsPass == "0")
                {
                    news.State = "0";
                    news.CheckName = currentAdmin;
                    AuditLog log = new AuditLog();
                    log.CompanyId = news.CompanyId;
                    log.Title = gameName + ' ' + title + "，新闻，未通过审核，原因:";
                    log.Type = 2;
                    log.TableId = id;
                    log.Reason = reason;
                    log.InTime = DateTime.Now;
                    AuditLogBll.Add(log);
                    NewsBll.Update(news);
                    return Content("ok");
                }
            }
            return Content("no");

        }
        #endregion
        #region 新闻批量审核
        public ActionResult MoreCheckNews(string ids, string checkIsPass, string currentAdmin)
        {

            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var item in allIds)
            {
                idList.Add(int.Parse(item));
            }
            if (NewsBll.MoreCheckNewsInfo(idList, checkIsPass, currentAdmin) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 新闻修改
        public ActionResult NewsEdit(int id)
        {
            ViewData.Model = NewsBll.LoadEntities(n => n.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsEdit(FormCollection form, int Id)
        {
            var news = NewsBll.LoadEntities(n => n.Id == Id).FirstOrDefault();
            news.Title = form["title"];
            news.EditTitle = form["editTitle"];
            news.Game = form["gameName"];
            news.Editor = form["editor"];
            news.Type = form["newsType"];
            news.Source = form["source"];
            news.Memo = form["memo"];
            news.Msg = form["msg"];
            news.InTime = news.State != "1" ? DateTime.Now : news.InTime;
            if (NewsBll.Update(news))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 新闻删除
        public ActionResult DeleteNews(int id)
        {
            News news = new News() { Id = id };
            if (news != null)
            {
                if (NewsBll.Delete(news))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            return Content("empty");
        }
        #endregion
        #region 个人用户表数据读取及模糊查询
        public ActionResult PersonalUser()
        {
            return View(PersonalUserBll.LoadEntities(p => p.Id > 0).ToList());
        }

        [HttpPost]
        public ActionResult GetPersonalUsers()
        {
            #region 连接查询求和多种尝试办法
            //var  result = from m in db.UserMessage
            //             group m by new { m.UserId, m.Pay, } into s
            //             select new { 
            //             UserId=s.Select(n=>n.UserId) ,                       
            //             Pay=s.Sum(n=>n.Pay),                                           
            //             };
            //var data = from r in result
            //            join p in db.PersonalUser on r.UserId.ToString() equals p.Id.ToString()
            //            select new {  r.UserId, r.Pay, p.InTime, p.QQ, p.WeChat };

            //var query = from m in db.UserMessage
            //            join p in db.PersonalUser on m.UserId equals p.Id
            //            where p.Id>0
            //            group m  by m.Pay into s
            //            select new {
            //                pay = s.Sum(y => y.Pay),
            //                userId=s.Select(y=>y.UserId), 
            //                Id=from u  in db.UserMessage
            //            };  
            #endregion

            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string uName = Request["uName"] as string;
            string mobile = Request["mobile"] as string;
            var json = new JsonResult();
            var allPerUser = PersonalUserBll.LoadEntities(r => r.Id > 0)
                                           .Select(p => new { p.Id, p.InTime, p.UName, p.QQ, p.WeChat, p.Email, p.Mobile, p.State });
            var allUserMsg = UserMessageBll.LoadEntities(c => c.Id > 0).Select(u => new { u.UserId, u.Pay });
            var data = from p in allPerUser
                       let paySum = (from m in allUserMsg where p.Id == m.UserId select (int?)m.Pay).Sum()
                       select new { p.Id, p.InTime, p.State, p.UName, p.QQ, p.WeChat, p.Mobile, p.Email, Pay = paySum };
            if (!string.IsNullOrEmpty(uName) || !string.IsNullOrEmpty(mobile))
            {
                data = data.Where(d => d.UName.Contains(uName) && d.Mobile.Contains(mobile));
                total = data.Count();
                json = Json(new { total = total, rows = data.OrderByDescending(d => d.InTime).Skip((pageIndex - 1) * pageSize).Take(pageSize) });
            }
            else
            {
                total = data.Count();
                var allData = data.OrderByDescending(d => d.InTime)
                   .Skip(pageSize * (pageIndex - 1))
                  .Take(pageSize).ToList();
                json = Json(new { total = total, rows = allData });
            }
            return json;
        }
        #endregion
        #region 个人用户系统充值
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult User_EarnCoolDetails(int id)
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int total = 0;
            var data = UserMessageBll.LoadPageEntities(pageSize, pageIndex, out total, um => um.PayType == "2" && um.UserId == id, false, um => um.InTime).ToList();
            ViewData["pageCount"] = total;
            ViewData["pageSize"] = pageSize;
            ViewData["pageIndex"] = pageIndex;
            ViewData["data"] = data;
            ViewBag.CurrentUserId = id;
            return View();
        }
        public ActionResult AddUserMessage(FormCollection form, int currentUserId)
        {

            UserMessage um = new UserMessage()
            {
                UserId = currentUserId,
                Title = "系统充值—恭喜您获得" + int.Parse(form["pay"]) + "爽币",
                Msg = "亲爱的爽赞会员恭喜您，" + int.Parse(form["pay"]) + "爽币已充值到账，可前往“我的爽币”—“充值记录”查看详情。爽币可充值试玩账号游戏币、兑换实体礼品",
                State = 0,
                IsDel = "0",
                PayType = "2",//充值
                Pay = int.Parse(form["pay"]),
                Memo = null,
                Memo1 = "0",//人民币
                Memo2 = "系统充值",//
                InTime = DateTime.Now,
                orderNo = form["remark"]
            };
            if (UserMessageBll.Add(um) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult DelUserMessage(int id, int currentUserId)
        {
            if (UserMessageBll.Delete(new UserMessage()
            {
                Id = id,
                UserId = currentUserId
            }))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        } 
        #endregion
        #region 个人充值记录后台列表
        public ActionResult Users_Recharges()
        {

            return View();
        }
        public ActionResult Users_GetRecharges(DateTime? startday, DateTime? endday)
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string uName = Request["uName"];
            string memo2 = Request["payType"];
            var allPerUser = PersonalUserBll.LoadEntities(r => r.Id > 0);
            var allUserMsg = UserMessageBll.LoadEntities(c => c.Id > 0);
            var data = from p in allPerUser
                       join m in allUserMsg on p.Id equals m.UserId
                       where m.PayType == "2"//充值
                       select new { p.Id, m.InTime, p.UName, m.Pay, m.Memo1, m.Memo2, m.orderNo, m.PayType };
            if (!string.IsNullOrEmpty(uName))
            {
                data = data.Where(d => d.UName.Contains(uName));
            }
            if (!string.IsNullOrEmpty(memo2))
            {
                data = data.Where(d => d.Memo2.Contains(memo2));
            }
            if (startday.HasValue || endday.HasValue)
            {
                data = data.Where(d => d.InTime >= startday && d.InTime <= endday);
            }
            total = data.Count();
            data = data.OrderByDescending(d => d.InTime)
                  .Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize);
            var result = new { total = total, rows = data.ToList() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  个人用户状态管理
        public ActionResult PersonalUserState(int id)
        {
            PersonalUser person = new PersonalUser()
            {
                Id = id,
            };
            if (person != null)
            {
                var personState = PersonalUserBll.LoadEntities(p => p.Id == id).FirstOrDefault();
                //当前用户状态等于0的改变为1
                if (personState.State == 0)
                {
                    personState.State = 1;

                    if (PersonalUserBll.Update(personState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
                else
                {
                    personState.State = 0;
                    if (PersonalUserBll.Update(personState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }

            }
            else
            {
                return Content("empty");
            }
        }
        #endregion
        #region 礼包审核管理
        public ActionResult Package()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }

        public ActionResult Getpackage()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string giftName = Request["giftName"] as string;
            string cpyName = Request["cpyName"] as string;
            string giftType = Request["giftType"];
            string checkState = Request["checkState"] == null ? "2" : Request["checkState"];
            var Package = PackageBll.LoadEntities(r => r.Id > 0)
                                           .Select(p => new { p.Id, p.InTime, p.GiftName, p.State, p.GameName, p.Type, p.ServerName, p.StartDate, p.EndDate, p.Msg, p.CompanyId, p.CheckName });
            var cpy = CompanyBll.LoadEntities(c => c.Id > 0).Select(c => new { c.Id, c.SystemName });
            var data = from p in Package
                       join c in cpy on p.CompanyId equals c.Id
                       select new
                       {
                           p.Id,
                           p.InTime,
                           p.GameName,
                           p.ServerName,
                           p.StartDate,
                           p.EndDate,
                           p.Msg,
                           p.Type,
                           p.GiftName,
                           p.State,
                           c.SystemName,
                           p.CheckName
                       };
            if (!string.IsNullOrEmpty(giftName))
            {
                data = data.Where(d => d.GiftName.Contains(giftName));
            }
            if (!string.IsNullOrEmpty(cpyName))
            {
                data = data.Where(d => d.SystemName.Contains(cpyName));
            }
            if (!string.IsNullOrEmpty(giftType))
            {
                data = data.Where(d => d.Type.Contains(giftType));
            }
            if (!string.IsNullOrEmpty(checkState))
            {
                data = data.Where(d => d.State.Contains(checkState));
            }
            total = data.Count();
            return Json(new { total = total, rows = data.ToList().OrderByDescending(d => d.InTime).Skip((pageIndex - 1) * pageSize).Take(pageSize) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 礼包单一审核
        public ActionResult PackageCheck(int id, string checkIsPass, string reason, string currentAdmin)
        {
            // string reason = Request["reason"];
            #region old
            //Package package = new Package() { Id = id };
            //if (package != null)
            //{
            //    package = PackageBll.LoadEntities(p => p.Id == id).FirstOrDefault();
            //    if (checkIsPass == "1")
            //    {
            //        package.State = "1";
            //        package.CheckName = currentAdmin;
            //        PackageBll.Update(package);
            //        return Content("ok");
            //    }
            //    else if (checkIsPass == "0")
            //    {
            //        package.State = "0";
            //        package.CheckName = currentAdmin;
            //        AuditLog log = new AuditLog();
            //        log.CompanyId = package.CompanyId;
            //        log.Title = gameName + ' ' + giftName + "，礼包，未通过审核，原因:";
            //        log.Type = 1;
            //        log.TableId = id;
            //        log.Reason = reason;
            //        log.InTime = DateTime.Now;
            //        AuditLogBll.Add(log);
            //        PackageBll.Update(package);
            //        return Content("ok");
            //    }
            //}
            //return Content("no"); 
            #endregion
            int i = PackageBll.PackageSingleCheck(id, checkIsPass, reason, currentAdmin);
            if (i > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 礼包批量审核
        public ActionResult PackageMoreCheck(string ids, string checkIsPass, string currentAdmin)
        {

            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var item in allIds)
            {
                idList.Add(int.Parse(item));
            }
            if (PackageBll.MoreCheckPachages(idList, checkIsPass, currentAdmin) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 礼包编辑
        public ActionResult PackageEdit(int id)
        {
            ViewData.Model = PackageBll.LoadEntities(p => p.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult PackageEdit(int Id, FormCollection form)
        {
            string type = form["type"].ToString();
            string state;
            if (type == "3")
            {
                state = "2";
            }
            else
            {
                state = "3";
            }
            var package = PackageBll.LoadEntities(n => n.Id == Id).FirstOrDefault();
            package.GameName = form["game"];
            package.ServerName = form["serversname"];
            package.Type = type;
            package.GiftName = form["giftName"];
            package.Url = form["url"];
            package.Memo = form["memo"];
            package.Msg = form["msg"];
            package.State = state;
            // package.InTime = DateTime.Now;
            if (PackageBll.Update(package))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 礼包删除
        public ActionResult packageDelete(int id)
        {
            Package p = new Models.Package { Id = id };
            if (p != null)
            {
                if (PackageBll.Delete(p))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            return Content("empty");
        }
        #endregion
        #region 开服审核管理
        public ActionResult OpenService()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }
        [HttpPost]
        public ActionResult GetOpenServices(DateTime? startday, DateTime? endday)
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string game = Request["game"];
            string cpyName = Request["cpyName"];
            string openType = Request["openType"];
            string checkState = Request["checkState"] == null ? "2" : Request["checkState"]; ;
            var param = new ServiceParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                GameName = game,
                CompanyName = cpyName,
                Type = openType,
                State = checkState,
                StartDay = startday,
                EndDay = endday
            };
            var data = OpenServiceBll.LoadOpenService(param).Select(d => new ServiceViewModel()
            {
                CompanyId = d.CompanyId,
                GameName = d.GameName,
                Id = d.Id,
                InTime = d.InTime,
                Type = d.Type,
                State = d.State,
                StartTime = d.StartTime,
                Url = d.Url,
                SystemName = d.SystemName,
                GiftName = d.GiftName,
                ServerName = d.ServerName,
                GiftType = d.GiftType,
                CheckName = d.CheckName
            }).ToList();
            return Json(new { rows = data, total = param.Total }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 开服信息删除
        public ActionResult OpenServiceDel(int id, int companyId)
        {
            OpenService op = new OpenService()
            {
                Id = id,
                CompanyId = companyId
            };
            //先删开服表
            if (OpenServiceBll.Delete(op))
            {
                //归还条数
                if (OpenServiceBll.ReturnServiceNum(id, companyId))
                {
                    return Content("ok,恭喜：删除成功！条数已归还！");
                }
                else
                {
                    return Content("no,提示：开服表删除成功！条数归还失败！");
                }
            }
            else
            {
                return Content("no,开服表删除失败！");
            }

        }
        #endregion
        #region 开服单一审核
        public ActionResult OpenServiceCheck(int id, int companyId, string checkIsPass, string serverName, string gameName, string currentAdmin)
        {
            string reason = Request["reason"];
            OpenService open = new OpenService() { Id = id };
            if (open != null)
            {
                open = OpenServiceBll.LoadEntities(p => p.Id == id).FirstOrDefault();
                //通过
                if (checkIsPass == "1")
                {
                    open.State = "1";
                    open.CheckName = currentAdmin;
                    OpenServiceBll.Update(open);
                    return Content("ok");
                }
                // 未通过
                else if (checkIsPass == "0")
                {
                    open.State = "0";
                    open.CheckName = currentAdmin;
                    AuditLog log = new AuditLog();
                    log.CompanyId = companyId;
                    log.Title = gameName + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "双线" + serverName + "服，开服，未通过审核，原因:";
                    log.Type = 0;
                    log.TableId = id;
                    log.Reason = reason;
                    log.InTime = DateTime.Now;
                    AuditLogBll.Add(log);
                    OpenServiceBll.Update(open);
                    return Content("ok");
                }
            }
            return Content("no");
        }
        #endregion
        #region 开服批量审核
        public ActionResult OpenServiceMoreCheck(string ids, string checkIsPass, string currentAdmin)
        {

            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var item in allIds)
            {
                idList.Add(int.Parse(item));
            }
            if (OpenServiceBll.MoreCheckOpenService(idList, checkIsPass, currentAdmin) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 攻略审核
        public ActionResult UserRaiders()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }
        [HttpPost]
        public ActionResult GetUserRaiders()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            //个人用户表（personuser）和UserRaiders关联
            var allUserRaider = UserRaidersBll.LoadEntities(ur => ur.Id > 0)
                .Select(ur => new { ur.State, ur.Id, ur.InTime, ur.Title, ur.UserId, ur.Rec_Hot, ur.GameName, ur.CheckName }).ToList();
            var allPersonUser = PersonalUserBll.LoadEntities(p => true).Select(p => new { p.Id, p.UName }).ToList();
            var userRaider_personUser = from ur in allUserRaider
                                        join u in allPersonUser on
                                        ur.UserId equals u.Id
                                        select new { ur.State, ur.Id, ur.InTime, ur.Title, ur.Rec_Hot, ur.GameName, ur.CheckName, u.UName };
            var json = new JsonResult();
            if (Request["uName"] == null)
            {
                int total = userRaider_personUser.Count();
                var allData = userRaider_personUser
                              .Where(d => d.State.Contains("2"))
                              .OrderByDescending(d => d.InTime)
                                            .Skip(pageSize * (pageIndex - 1))
                                            .Take(pageSize)
                                            .ToList();
                json = Json(new { total = total, rows = allData });
            }
            else
            {
                string uName = Request["uName"].Trim() as string;
                string game = Request["game"].Trim() as string;
                string title = Request["title"].ToString();
                string checkState = Request["checkState"].ToString();

                var searchData = from d in userRaider_personUser
                                 where d.GameName.Contains(game) && d.UName.Contains(uName) && d.Title.Contains
                                 (title)
                                 && d.State.Contains(checkState)
                                 select d;
                int total = searchData.Count();
                json = Json(new { total = total, rows = searchData.ToList().OrderByDescending(d => d.InTime).Skip((pageIndex - 1) * pageSize).Take(pageSize) });
            }
            return json;
        }
        #endregion
        #region 礼包删除
        public ActionResult UserRaidersDel(int id)
        {
            UserRaiders ur = new UserRaiders { Id = id };
            if (ur != null)
            {
                if (UserRaidersBll.Delete(ur))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            return Content("empty");
        }
        #endregion
        #region 攻略单一审核
        //TODO需要加事务处理、可以正常运行  后期加未通过的原因
        public ActionResult UserRaidersCheck(int id, string checkIsPass, string currentAdmin, string reason)
        {
            UserRaiders ur = new UserRaiders() { Id = id };
            if (ur != null)
            {
                ur = UserRaidersBll.LoadEntities(p => p.Id == id).FirstOrDefault();
                if (checkIsPass == "1")
                {
                    // 1、------先更新通过审核-----------------
                    ur.State = "1";
                    ur.CheckName = currentAdmin;
                    UserRaidersBll.Update(ur);
                    //2、----将审核通过的消息写入到userMessage--
                    UserMessage um = new UserMessage();
                    um.UserId = ur.UserId;
                    um.Title = "攻略投稿—通过审核<<" + ur.Title + ">>";
                    um.Msg = "亲爱的爽赞会员恭喜您，你投递的攻略《" + ur.Title + "》已审核通过啦，特此奖励10爽币。希望您在之后的日子里面，多多投稿，帮助到更多的玩家朋友哦";
                    um.State = 0;//消息未读
                    um.PayType = "1";//赚爽币记录
                    um.Pay = 10;
                    um.InTime = DateTime.Now;
                    um.IsDel = "0";
                    um.Memo = null;
                    um.Memo1 = null;
                    um.Memo2 = null;
                    um.orderNo = null;
                    UserMessageBll.Add(um);
                    return Content("ok");
                }
                else if (checkIsPass == "0")
                {
                    ur.State = "0";
                    ur.CheckName = currentAdmin;
                    UserRaidersBll.Update(ur);
                    //2、----将审核通过的消息写入到userMessage--
                    UserMessage um = new UserMessage();
                    um.UserId = ur.UserId;
                    um.Title = "攻略投稿—未通过审核<<" + ur.Title + ">>";
                    um.Msg = "亲爱的爽赞会员您好，非常抱歉，您投递的攻略《" + ur.Title + "》因'" + reason + "'的原因未通过审核，希望您针对我们提出的意见做一些修改之后再次提交。如有什么疑问，可直接联系在线客服哦";
                    um.State = 0;//消息未读
                    um.PayType = "1";//赚爽币记录
                    um.Pay = -10;
                    um.InTime = DateTime.Now;
                    um.IsDel = "0";
                    um.Memo = null;
                    um.Memo1 = null;
                    um.Memo2 = null;
                    um.orderNo = null;
                    UserMessageBll.Add(um);
                    return Content("ok");
                }

            }
            return Content("no");
        }
        //public ActionResult TestTransaction()
        //{
        //    using (var dbContext = new ShuangZanModelDBContainer())
        //    {
        //        using (var scope = dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (ids != null)
        //                {
        //                    foreach (var id in ids)
        //                    {
        //                        T t = dbContext.Find<T>(id);
        //                        assfeedback.IsDel = true;
        //                        dbContext.Update<T>(t);
        //                    }
        //                }
        //                scope.Commit();//正常完成就可以提交
        //                return 0;
        //            }
        //            catch (Exception ex)
        //            {
        //                scope.Rollback();//发生异常就回滚
        //                return -1;
        //            }
        //        }
        //    }
        //}
        #endregion
        #region 攻略批量审核
        public ActionResult UserRaidersMoreCheck(string ids, string checkIsPass, string currentAdmin, string reason)
        {

            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var item in allIds)
            {
                idList.Add(int.Parse(item));
            }
            if (UserRaidersBll.MoreCheckUserRaiders(idList, checkIsPass, currentAdmin, reason) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 攻略编辑
        public ActionResult UserRaidersEdit(int id)
        {
            ViewData.Model = UserRaidersBll.LoadEntities(ur => ur.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]//非法字符不要验证
        public ActionResult UserRaidersEdit(FormCollection form, int Id)
        {


            var ur = UserRaidersBll.LoadEntities(u => u.Id == Id).FirstOrDefault();
            ur.Title = form["Title"];
            ur.EditTitle = form["EditTitle"];
            ur.Editor = form["Editor"];
            ur.Key = form["Key"];
            ur.Source = form["Source"];
            ur.GameName = form["GameName"];
            ur.Memo = form["Memo"];
            ur.Msg = form["Msg"];
            ur.InTime = ur.State != "1" ? DateTime.Now : ur.InTime;
            if (UserRaidersBll.Update(ur))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 邀请链接管理
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult UsersInvite()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult GetUsersInvite(DateTime? startday, DateTime? endday)
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            string uName = Request["uName"];
            var allUsers = PersonalUserBll.LoadEntities(p => p.Id > 0).Select(p => new { p.Id, p.InTime, p.UName, p.RecommendNum, p.ReferrerId });
            var data = from p in allUsers
                       join u in allUsers on p.ReferrerId equals u.Id
                       where p.ReferrerId > 0
                       select new { p.Id, p.UName, p.InTime, p.RecommendNum, beInviteName = u.UName };
            if (!string.IsNullOrEmpty(uName))
            {
                data = data.Where(d => d.UName.Contains(uName));
            }
            if (startday.HasValue || endday.HasValue)
            {
                endday = endday.Value.AddDays(1);
                data = data.Where(d => d.InTime >= startday && d.InTime < endday);
            }
            int total = data.Count();
            data = data.OrderByDescending(d => d.InTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return Json(new { total = total, rows = data.ToList() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 游戏试玩截图审核
        public ActionResult DemoCutImgList()
        {
            // DemoUserBll.LoadDemoCutImgDetails(int userid);

            return View();
        }
        public ActionResult GetDemoCutImgList()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string type = Request["type"];
            string state = Request["state"] == null ? "2" : Request["state"];
            string gameName = Request["gameName"];
            string systemName = Request["systemName"];
            string uName = Request["uName"];
            var param = new DemoCutImgParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                Type = type,
                State = state,
                GameName = gameName,
                UName = uName,
                SystemName = systemName
            };
            var data = DemoUserBll.LoadDemoCutImgCheckData(param).Select(d => new DemoCutImgCheckViewModel()
                                    {
                                        Id = d.Id,//对应的玩家
                                        RequireId = d.RequireId,
                                        DemoId = d.DemoId,
                                        AccountId = d.AccountId,
                                        GameName = d.GameName,//试玩游戏表游戏名称
                                        SystemName = d.SystemName,//账号表
                                        Account = d.Account,//账号表的试玩账号
                                        UName = d.UName,//玩家用户
                                        AwardCoins = d.AwardCoins,//需求表的奖励爽币
                                        CoinsEquity = d.CoinsEquity,//需求表的奖励权益
                                        Demand = d.Demand,//需求表的要求
                                        Img = d.Img,//要求表的图片
                                        InTime = d.InTime,//要求表的时间
                                        Type = d.Type,//试玩表中试玩类型 
                                        State = d.State
                                    }).ToList();
            var result = new { rows = data, total = param.Total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 游戏试玩截图审核是否通过事件
        public ActionResult DemoCheckEvent(int accountId, int demoId, int userId, int requireId)
        {

            string state = Request["state"];
            string systemName = Request["systemName"];
            string gameName = Request["gameName"];
            string account = Request["account"];
            string demand = Request["demand"];
            string reason = Request["reason"];
            int awardCoins = int.Parse(Request["awardCoins"]);
            string type = Request["type"];
            type = allGameType(type);
            var demouser = DemoUserBll.LoadEntities(du => du.GameDemoId == demoId && du.AccountId == accountId && du.RequiredId == requireId && du.UserId == userId).FirstOrDefault();
            if (state == "1")
            {
                UserMessage um = new UserMessage();
                um.UserId = userId;
                um.Title = "爽币玩游戏—" + systemName + "《" + gameName + "》" + type + "" + account + "，" + demand + "—截图通过审核";
                um.Msg = "亲爱的爽赞会员恭喜您，" + systemName + "《" + gameName + "》" + type + "" + account + "，" + demand + "的截图已审核通过啦，奖励" + awardCoins + "爽币。可充值试玩账号游戏币、兑换实体礼品。";
                um.State = 0;//代表消息未读
                um.PayType = "1";//赚爽币
                um.Pay = awardCoins;
                um.IsDel = "0";
                um.Memo = null;
                um.Memo1 = null;
                um.Memo2 = null;
                um.InTime = DateTime.Now;
                demouser.State = "1";
                demouser.Reason = reason;
                DemoUserBll.Update(demouser);
                if (UserMessageBll.Add(um) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else if (state == "0")
            {
                UserMessage um = new UserMessage();
                um.UserId = userId;
                um.Title = "爽币玩游戏—" + systemName + "《" + gameName + "》" + type + "" + account + "，" + demand + "—截图审核未通过";
                um.Msg = "亲爱的爽赞会员您好，非常遗憾，由于" + reason + "的原因，您的" + systemName + "《" + gameName + "》" + type + "试玩账号" + account + "的截图未通过审核，请针对我们提出的意见做一些修改之后再次提交。如有什么疑问，可直接联系在线客服哦。";
                um.State = 0;//代表消息未读
                um.PayType = "1";//赚爽币
                um.Pay = 0;
                um.IsDel = "0";
                um.Memo = null;
                um.Memo1 = null;
                um.Memo2 = null;
                um.InTime = DateTime.Now;

                demouser.State = "0";
                demouser.Reason = reason;
                DemoUserBll.Update(demouser);
                if (UserMessageBll.Add(um) != null)
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
                return Content("empty");
            }

            #region 调用存储过程
            //   string state = Request["state"];
            //   string type = Request["type"];
            //   string reason = Request["reason"];
            //   type = allGameType(type);
            //var str=DemoUserBll.DemoCheckEvent(accountId, demoId, userId,requireId, state, type, reason);
            // return Json(str, JsonRequestBehavior.AllowGet); 
            #endregion
        }
        #endregion
        #region 判断游戏试玩的类型
        public string allGameType(string type)
        {
            string[] allType = { "最强王者", "超凡大师", "璀璨钻石", "华贵铂金", "荣耀黄金 ", "不屈白银", "英勇黄铜" };
            int n = int.Parse(type);
            return n >= 0 ? allType[n] : null;
        }
        #endregion
        #region 审查游戏试玩的详情
        public ActionResult CheckDemoDetails(int accountId, int demoId, int userId, int required)
        {
            ViewData.Model = DemoUserBll.LoadDemoCutImgDetails(demoId, accountId, userId).Select(d => new DemoCutImgCheckViewModel()
              {
                  UserId = d.UserId,
                  DemoId = d.DemoId,
                  AccountId = d.AccountId,
                  RequireId = d.RequireId,
                  GameName = d.GameName,//试玩游戏表游戏名称
                  SystemName = d.SystemName,//账号表
                  Account = d.Account,//账号表的试玩账号
                  UName = d.UName,//玩家用户
                  AwardCoins = d.AwardCoins,//需求表的奖励爽币
                  CoinsEquity = d.CoinsEquity,//需求表的奖励权益
                  Demand = d.Demand,//需求表的要求
                  Img = d.Img,//要求表的图片
                  InTime = d.InTime,//要求表的时间
                  Type = d.Type,//试玩表中试玩类型
                  State = d.State
              }).FirstOrDefault();
            var data = DemoUserBll.LoadDemoCutImgDetails(demoId, accountId, userId).Select(d => new DemoCutImgCheckViewModel()
               {
                   UserId = d.UserId,
                   DemoId = d.DemoId,
                   AccountId = d.AccountId,
                   RequireId = d.RequireId,
                   GameName = d.GameName,//试玩游戏表游戏名称
                   SystemName = d.SystemName,//账号表
                   Account = d.Account,//账号表的试玩账号
                   UName = d.UName,//玩家用户
                   AwardCoins = d.AwardCoins,//需求表的奖励爽币
                   CoinsEquity = d.CoinsEquity,//需求表的奖励权益
                   Demand = d.Demand,//需求表的要求
                   Img = d.Img,//要求表的图片
                   InTime = d.InTime,//要求表的时间
                   Type = d.Type,//试玩表中试玩类型
                   State = d.State
               }).ToList();
            ViewData["CheckDemoDetails"] = data;
            ViewData["required"] = required;
            return View();
        }
        #endregion
        #region 试玩充值审核
        public ActionResult DemoRechargecheckList()
        {
            return View();
        }
        public ActionResult GetDemoRechargecheckList()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string rechargeState = Request["state"];
            string gameName = Request["gameName"];
            string account = Request["account"];
            string uName = Request["uName"];
            string type = Request["type"];
            string systemName = Request["systemName"];
            var param = new DemoCutImgParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                RechargeState = rechargeState,
                Type = type,
                GameName = gameName,
                UName = uName,
                Account = account,
                SystemName = systemName
            };
            var data = DemoUserBll.LoadDemoRechargeCheckData(param).Select(d => new DemoCutImgCheckViewModel()
            {
                Id = d.Id,
                UserId = d.UserId,
                GameName = d.GameName,//试玩游戏表游戏名称
                SystemName = d.SystemName,//账号表
                Account = d.Account,//账号表的试玩账号
                UName = d.UName,//玩家用户
                Pwd = d.Pwd,
                ServerName = d.ServerName,  //多少服                                              
                InTime = d.InTime,//要求表的时间
                Type = d.Type,//试玩表中试玩类型
                RechargeState = d.RechargeState,
                Pay = d.Pay,
                CurrentPlayer = d.CurrentPlayer
            }).ToList();
            var result = new { rows = data, total = param.Total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 处理游戏试玩充值状态

        public ActionResult RechargeCheck(string gameName, string systemName, string Uname)
        {
            int userId = int.Parse(Request["userId"]);
            int id = int.Parse(Request["id"].ToString());
            string reason = Request["reason"];
            int pay = int.Parse(Request["pay"].ToString());
            int rechargeState = int.Parse(Request["rechargeState"].ToString());
            GameDemoRecharge demoRechage = GameDemoRechargeBll.LoadEntities(r => r.Id == id && r.State == 2).FirstOrDefault();
            if (rechargeState == 0)//退款
            {
                UserMessage um = new UserMessage();
                um.UserId = userId;
                um.Title = "爽币玩游戏—'," + gameName + "'《" + systemName + "》试玩账号'," + Uname + ",'充值失败";
                um.Msg = "亲爱的爽赞会员非常遗憾，您在'," + systemName + ",'《" + gameName + "》的试玩账号'," + Uname + ",'充值失败，消费的'," + pay + ",'爽币已经退回，请注意查看账户余额。更多活动，敬请关注淘福利版块哟~";
                um.State = 0;
                um.PayType = "3";//爽币消费记录
                um.Pay = pay;
                um.IsDel = "0";
                um.Memo = null;
                um.Memo1 = null;
                um.Memo2 = null;
                um.InTime = DateTime.Now;
                if (UserMessageBll.Add(um) != null)
                {
                    demoRechage.Reason = reason;
                    demoRechage.State = rechargeState;
                    demoRechage.InTime = DateTime.Now;
                    GameDemoRechargeBll.Update(demoRechage);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else if (rechargeState == 1)//处理完成
            {
                UserMessage um = new UserMessage();
                um.UserId = userId;
                um.Title = "爽币玩游戏—'," + gameName + "'《" + systemName + "》试玩账号'," + Uname + ",'充值成功";
                um.Msg = "亲爱的爽赞会员恭喜您，'," + systemName + ",'《" + gameName + "》的试玩账号'," + Uname + ",'的游戏元宝已到账，请注意前往游戏查看账户余额。更多活动，敬请关注“淘福利”版块哟~";
                um.State = 0;//消息未读
                um.PayType = "3";//爽币消费记录
                um.Pay = 0;//成功加分值为0
                um.IsDel = "0";
                um.Memo = null;
                um.Memo1 = null;
                um.Memo2 = null;
                um.InTime = DateTime.Now;
                if (UserMessageBll.Add(um) != null)
                {
                    demoRechage.Reason = reason;
                    demoRechage.State = rechargeState;
                    demoRechage.InTime = DateTime.Now;
                    //当前操作玩家，da表
                    GameDemoRechargeBll.Update(demoRechage);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {

                return Content("empty");
            }
        }
        #endregion
        #region 留言别表(异步分页)
        //public ActionResult MsgShow()
        //{
        //    return View();
        //}
        //public ActionResult LeavesMsgInfo()
        //{
        //    int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
        //    int pageSize = int.Parse(Request["pageSize"] ?? "10");
        //    int totalCount = 0;
        //    List<LeaveMsg> listMsg = LeaveMsgBll.LoadPageEntities(pageSize, pageIndex, out totalCount, l => true, false, l => l.Id).ToList();
        //    //ViewData["MsgInfo"] = listMsg;
        //    string strNav = LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
        //    var data = from m in listMsg select new { m.Id, m.Date, m.Name, m.Msg, m.Tel, m.ContactMan, m.Email, m.Remark };
        //    var result = new { Data = data, NavStr = strNav };
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}
        #endregion
        #region 留言删除

        //public ActionResult DelteMsg(int id)
        //{

        //    LeaveMsg msg = new LeaveMsg() { Id = id };
        //    if (LeaveMsgBll.Delete(msg) == false)
        //    {
        //        return Content("ok");
        //    }
        //    return Content("no");

        //}
        #endregion
        #region 多文件上传
        public JsonResult ImgUpload()
        {
            UploadImages img = new UploadImages();
            HttpPostedFileBase file = Request.Files["fileInput"];
            if (file != null)
            {
                string _imageExt = System.IO.Path.GetExtension(file.FileName).ToLower();
                string _imageName = Common.WebHelper.GetStreamMd5(file.InputStream) + _imageExt;
                string dir = "/Content/PublicImg/" + _imageName;
                string fullDir = System.Web.HttpContext.Current.Server.MapPath(dir);
                file.SaveAs(fullDir);
                img.State = 1;
                img.Name = _imageName;
                img.Path = dir;
            }
            else
            {
                img.State = 0;
            }
            return Json(img, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 拿到当前管理员的姓名
        public UserInfo GetCurrentAdmin()
        {
            //Done 根据guid拿到当前登录的用户。
            string guid = Request["mySessionId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var loginAdmin = Common.CacheHelper.Get(guid) as UserInfo;
            if (loginAdmin != null)
            {
                ViewData.Model = loginAdmin;
            }
            return ViewData.Model as UserInfo;
        }
        public ActionResult CurrrentAdmin()
        {
            var data = GetCurrentAdmin();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 前台新闻管理（看新闻首页统一管理）

        /// <summary>
        /// 上传幻灯轮播
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSlider()
        {
            return View();
        }
        // 7   幻灯轮播。
        // 8   红字头条。      
        // 9  灰字头条。 
        // 10   五条置顶区
        public ActionResult CreateSlider(FormCollection form)
        {

            SeeNews seenews = new SeeNews()
            {
                Title = form["title"],
                Type = form["type"],
                Url = form["url"],
                Intime = DateTime.Now,
                Image = form["logo"] == null ? null : form["logo"]
            };
            if (SeeNewsBll.Add(seenews) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        #endregion
        #region 看新闻推荐管理
        public ActionResult RecSeeNews()
        {
            return View();
        }
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult GetRecSeeNews()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string title = Request["title"];
            string type = Request["newsType"];
            var para = new RecSeeNews()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                Title = title,
                Type = type
            };
            var data = SeeNewsBll.LoadSeeNews(para).Select(t => new SeeNewsViewModel()
            {
                Title = t.Title,
                Type = t.Type,
                Image = t.Image,
                Id = t.Id,
                Url = t.Url,
                Intime = t.Intime,
                RedLight = t.RedLight,
                GreyHeadlines = t.GreyHeadlines,
                redHeadlines = t.redHeadlines,
                FiveStick = t.FiveStick,
                DirectSeeding = t.DirectSeeding,
                MobileGame = t.MobileGame,
                NewGame = t.NewGame,
                HotGame = t.HotGame,
                Industry = t.Industry,
            }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 看新闻编辑
        public ActionResult SeeNewsEdit(int id)
        {
            ViewData.Model = SeeNewsBll.LoadEntities(s => s.Id == id).FirstOrDefault();
            return View();
        }
        public ActionResult SeeNewsUpdate(FormCollection form, int id)
        {
            var seenews = SeeNewsBll.LoadEntities(s => s.Id == id).FirstOrDefault();
            seenews.Title = form["title"];
            seenews.Intime = DateTime.Now;
            seenews.Image = form["logo"] == "" ? null : form["logo"];
            seenews.Url = form["url"];
            seenews.Type = form["type"];
            if (SeeNewsBll.Update(seenews))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 看新闻的删除
        public ActionResult DelteSeeNews(int id)
        {
            SeeNews seenews = new SeeNews() { Id = id };
            if (SeeNewsBll.Delete(seenews))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 留言管理调用业务层方法
        private List<LeaveMsgViewModel> GetLeaveMsgInfo()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            string name = Request["userName"];
            string content = Request["content"];
            var param = new LeaveMsgParam()
             {
                 PageIndex = pageIndex,
                 PageSize = pageSize,
                 Total = total,
                 UserName = name,
                 Content = content
             };
            Guid cacheKey = Guid.NewGuid();
            object obj = Common.CacheHelper.Get(cacheKey.ToString());
            List<LeaveMsgViewModel> list = null;
            if (obj == null)
            {
                list = LeaveMsgBll.LoadLeaveMsg(param).Select(m => new LeaveMsgViewModel()
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    NewsTitle = m.NewsTitle,
                    RaidersTitle = m.RaidersTitle,
                    Type = m.Type,
                    City = m.City,
                    IP = m.IP,
                    Content = m.Content,
                    InTime = m.InTime
                }).ToList();
                if (list != null)
                {
                    Common.CacheHelper.WriteCache(cacheKey.ToString(), list, DateTime.Now.AddMinutes(5));
                }
            }
            else
            {
                list = obj as List<LeaveMsgViewModel>;

            }
            return list;
        }
        #endregion
        #region 留言管理
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult LeaveMsgPage()
        {
            return View();
        }
        public ActionResult MsgList()
        {

            int pageSize = int.Parse(Request["rows"] ?? "20");
            int pageIndex = int.Parse(Request["page"] ?? "1");
            int total = 0;
            string name = Request["userName"];
            string content = Request["content"];
            var param = new LeaveMsgParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                UserName = name,
                Content = content
            };

            var list1 = LeaveMsgBll.LoadLeaveMsg(param).Select(m => new LeaveMsgViewModel()
              {
                  Id = m.Id,
                  UserName = m.UserName,
                  NewsTitle = m.NewsTitle,
                  // EditTitle = m.EditTitle,
                  RaidersTitle = m.RaidersTitle,
                  GirlTitle = m.GirlTitle,
                  Type = m.Type,
                  City = m.City,
                  IP = m.IP,
                  Content = m.Content,
                  InTime = m.InTime
              }).ToList();
            var result = new { total = param.Total, rows = list1 };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 留言管理的删除
        public ActionResult LeaveMsgDel(int id)
        {
            LeaveMsg msg = new LeaveMsg() { Id = id };
            if (LeaveMsgBll.Delete(msg))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 留言黑名单添加
        public ActionResult AddBlackListIp(FormCollection form)
        {
            BlackListIP ip = new BlackListIP()
            {
                Id = Guid.NewGuid(),
                IP = form["ip"],
                InTime = DateTime.Now
            };
            if (BlackListIPBll.Add(ip) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        //------------------首页推荐--------------------
        #region 首页各类型数据添加
        public ActionResult HomeIndex()
        {
            return View();
        }
        public ActionResult AddHomePage(FormCollection form)
        {
            HomePage page = new HomePage()
            {
                Name = form["name"],
                Url = form["url"],
                Img = form["logo"] == null ? null : form["logo"],
                Type = form["type"],
                SmallImg = form["smalllogo"],
                InTime = DateTime.Now
            };
            if (HomePageBll.Add(page) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 首页各类型数据编辑
        [HttpGet]
        public ActionResult HomePageEdit(int id)
        {
            ViewData.Model = HomePageBll.LoadEntities(h => h.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult HomePageEdit(int id, FormCollection form)
        {
            HomePage page = HomePageBll.LoadEntities(h => h.Id == id).FirstOrDefault();
            page.Id = int.Parse(Request["id"].ToString());
            page.Name = form["name"];
            page.Type = form["type"];
            page.Url = form["url"];
            page.Img = form["logo"] == null ? null : form["logo"];
            page.SmallImg = form["smalllogo"];
            page.InTime = DateTime.Now;
            if (HomePageBll.Update(page))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 首页图片利用Md5添加
        public ActionResult IndexUpload()
        {
            string msg = string.Empty;
            string filePath = "/Content/PublicImg/";
            HttpPostedFileBase fileLogo = Request.Files["logoImgs"];
            HttpPostedFileBase fileIcons = Request.Files["smallIcons"];
            // string str="";
            if (fileLogo != null)
            {
                string str = Common.UploadImgs.CommonUploadImg(fileLogo, out msg, filePath);
                return Content(str);
            }

            string str2 = Common.UploadImgs.CommonUploadImg(fileIcons, out msg, filePath);
            return Content(str2);



        }
        #endregion
        #region 首页管理
        public ActionResult HomeList()
        {
            return View();
        }
        public ActionResult HomeListData()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string name = Request["name"];
            string type = Request["type"];
            var para = new HomePageParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                Title = name,
                Type = type
            };
            var data = HomePageBll.GetAllHomePageData(para).Select(t => new HomePageViewModel()
            {
                Name = t.Name,
                Type = t.Type,
                Image = t.Image,
                Id = t.Id,
                Url = t.Url,
                Intime = t.Intime,
                RecGame = t.RecGame,
                RedLight = t.RedLight,
                redHeadlines = t.redHeadlines,
                GreyHeadlines = t.GreyHeadlines,
                FiveStick = t.FiveStick,
                DirectSeeding = t.DirectSeeding,
                MobileGame = t.MobileGame,
                JoinCompany = t.JoinCompany,
                NewestHotGame = t.NewestHotGame,

            }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HomePageDel(int id)
        {
            HomePage page = new HomePage() { Id = id };
            if (HomePageBll.Delete(page))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        //-------------------Seo推荐---------------------------
        #region seo专用推荐
        public ActionResult SeoList()
        {
            return View();
        }
        public ActionResult GetSeoList()
        {

            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var data = SeoBll.LoadPageEntities(pageSize, pageIndex, out total, s => s.Id > 0, false, s => s.InTime).ToList();
            var result = new { rows = data, total = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSeoInfo(FormCollection form)
        {
            Seo seo = new Seo()
            {
                Name = form["name"],
                Url = form["url"],
                Type = form["type"],
                InTime = DateTime.Now
            };
            if (SeoBll.Add(seo) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        [HttpGet]
        public ActionResult SeoEdit(int id)
        {
            ViewData.Model = SeoBll.LoadEntities(s => s.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult SeoEdit(int id, FormCollection form)
        {
            Seo seo = SeoBll.LoadEntities(s => s.Id == id).FirstOrDefault();
            seo.Name = form["name"];
            seo.Url = form["url"];
            seo.Type = form["type"];
            seo.InTime = DateTime.Now;
            if (SeoBll.Update(seo))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult SeoDel(int id)
        {
            Seo seo = new Seo()
            {
                Id = id
            };
            if (SeoBll.Delete(seo))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        //-------------精彩图文推荐-----------------------------------
        #region 精彩图文推荐
        public ActionResult WonderFulTxtImgList()
        {
            return View();
        }
        public ActionResult GetWonderFulTxtImgList()
        {
            int pageSize = int.Parse(Request["rows"] ?? "20");
            int pageIndex = int.Parse(Request["page"] ?? "1");
            int total = 0;
            var data = WonderfulTxtImgBll.LoadPageEntities(pageSize, pageIndex, out total, s => s.Id > 0, false, s => s.InTime).Select(w => new { w.Id, w.Image, w.InTime, w.Title, w.Url }).ToList();
            var result = new { rows = data, total = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadTxtImg()
        {
            string msg = string.Empty;
            //精彩图文推荐上传文件夹
            string filePath = "/Content/PublicImg/";
            HttpPostedFileBase file = Request.Files["logoImgs"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            return Content(str);

        }
        public ActionResult AddTxtImg(FormCollection form)
        {
            WonderfulTxtImg txtImg = new WonderfulTxtImg()
            {
                Title = form["title"],
                Url = form["url"],
                Image = form["image"],
                InTime = DateTime.Now
            };
            if (WonderfulTxtImgBll.Add(txtImg) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        [HttpGet]
        public ActionResult TxtImgEdit(int id)
        {
            ViewData.Model = WonderfulTxtImgBll.LoadEntities(w => w.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult TxtImgEdit(int id, FormCollection form)
        {
            WonderfulTxtImg txtImg = WonderfulTxtImgBll.LoadEntities(w => w.Id == id).FirstOrDefault();
            txtImg.Title = form["title"];
            txtImg.Image = form["image"];
            txtImg.Url = form["url"];
            // txtImg.InTime = DateTime.Now;
            if (WonderfulTxtImgBll.Update(txtImg))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult TxtImgDel(int id)
        {
            WonderfulTxtImg txtImg = new WonderfulTxtImg()
            {
                Id = id
            };

            if (WonderfulTxtImgBll.Delete(txtImg))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 广告管理
        public ActionResult AdvertList()
        {
            return View();
        }
        public ActionResult GetAdvertList()
        {
            int pageSize = int.Parse(Request["rows"] ?? "20");
            int pageIndex = int.Parse(Request["page"] ?? "1");
            int total = 0;
            string type = Request["type"];
            var param = new AdvertParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Type = type
            };
            var data =
                AdvertisementBll.LoadAdvertisementData(param).Select(a => new { a.Id, a.Title, a.Type, a.Url, a.State, a.Path, a.StartTime, a.EndTime, a.InTime }).ToList();
            var result = new { rows = data, total = param.Total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdvertDel(int id)
        {
            Advertisement ad = new Advertisement() { Id = id };
            if (AdvertisementBll.Delete(ad))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 添加广告

        public ActionResult AddAdvert()
        {
            return View();
        }

        public ActionResult AddAdvertInfo()
        {
            string title = Request["title"].ToString();
            string type = Request["type"].ToString();
            string path = Request["path"].ToString();
            DateTime startTime = DateTime.Parse(Request["startTime"].ToString());
            DateTime endTime = DateTime.Parse(Request["endTime"].ToString());
            int state = int.Parse(Request["state"].ToString());
            string url = Request["url"];
            Advertisement ad = new Advertisement()
            {
                Title = title,
                Type = type,
                Path = path,
                StartTime = startTime,
                EndTime = endTime,
                State = state,
                InTime = DateTime.Now,
                Url = url
            };
            if (AdvertisementBll.Add(ad) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 编辑广告
        [HttpGet]
        public ActionResult AdvertEdit(int id)
        {
            var d = AdvertisementBll.LoadEntities(a => a.Id == id).FirstOrDefault();
            ViewData.Model = d;
            // StringBuilder sb = new StringBuilder();
            // string path = string.Empty;
            // if (d.Path.Split('.')[1] == "swf")
            // {
            //   //path=  string.Format("<embed class='hide' menu='false' wmode='transparent' type='application/x-shockwave-flash' pluginspage='{0}' quality='high' src='{1}' width='300' height='200'>", d.Url, d.Path);
            //     sb.Append("<embed class='hide' menu='false' wmode='transparent' type='application/x-shockwave-flash' pluginspage='"+d.Url+"' quality='high' src='"+d.Path+"' width='300' height='200'>");
            // }
            // else {
            // // path=  string.Format("<img src='{0}' class='file-preview-image' />", d.Path);
            //     sb.Append("<img src='"+d.Path+"' class='file-preview-image' />");
            // }
            //ViewBag.Path = sb.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AdvertEdit(int id, FormCollection form)
        {
            var ad = AdvertisementBll.LoadEntities(a => a.Id == id).FirstOrDefault();
            ad.Id = id;
            ad.Title = form["title"];
            ad.State = int.Parse(form["state"]);
            ad.Type = form["type"];
            ad.Url = form["url"];
            ad.Path = form["path"];
            ad.StartTime = DateTime.Parse(form["startTime"]);
            ad.EndTime = DateTime.Parse(form["endTime"]);
            ad.InTime = DateTime.Now;
            if (AdvertisementBll.Update(ad))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
    }
    #region 上传图片用到的类
    public class UploadImages
    {
        public int State { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    #endregion
}
