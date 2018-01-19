using Common;
using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = true)]
    public class PersonalUserController : Controller
    {
        // GET: /PersonalUser/玩家个人中心控制器
        public IPersonalUserBll PersonalUserBll { get; set; }
        public IPackageBll PackageBll { get; set; }//礼包
        public IPackageCardBll PackageCardBll { get; set; }
        public IDemoUserBll DemoUserBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        public IUserMessageBll UserMessageBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IOrderBll OrderBll { get; set; }
        IPersonalUserSignBll PersonalUserSignBll { get; set; }
        IPersonalUserSignDetailBll PersonalUserSignDetailBll { get; set; }
        #region 玩家个人中心的头部数据展示
        public ActionResult UserTop()
        {
            ViewData.Model = GetCurrentUser();
            var user = ViewData.Model as PersonalUser;
            //-------------最新的五条消息-----------------------------
            var data = UserMessageBll.LoadEntities(um => um.UserId == user.Id&&(um.IsDel == "0" || um.IsDel == null)).ToList()
                            .OrderByDescending(um => um.InTime)
                           .Select(um => new UserMessage() { Title = um.Title, Id = um.Id, InTime = um.InTime }).Take(4).ToList();
            ViewBag.TopFiveMsg = data;
            //------------爽币余额--------------------------------------------
            ViewBag.CoinsUsed = UserMessageBll.CoolCoinsUsed(user);
            return PartialView("UserTop", ViewData.Model);
        }
        #endregion
        #region 玩家个人尾部数据展示
        public ActionResult _SignInDiv()
        {
            ViewData.Model = GetCurrentUser();
            return PartialView("_SignInDiv", ViewData.Model);
        }
        #endregion          
        #region 拿到当前个人用户
        public PersonalUser GetCurrentUser()
        {
            //Done 根据guid拿到当前登录的用户。
            string guid = Request["userId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            if (user != null)
            {
                ViewData.Model = user;
            }
            return ViewData.Model as PersonalUser;
        }
        #endregion
        #region 检验个人玩家用户名是否已经存在
        public ActionResult CheckUName(string uname)
        {
            var user = PersonalUserBll.LoadEntities(u => u.UName == uname).FirstOrDefault();
            if (user != null)
            {
                return Content("ok");
            }
            else
            {

                return Content("no");
            }
        }
        #endregion
        #region 检验个人和玩家注册时的手机号是否重复
        public ActionResult CheckMobile(string mobile)
        {
            var user = PersonalUserBll.LoadEntities(u => u.Mobile == mobile).Select(u => u.Mobile).FirstOrDefault();
            if (user != null)
            {
                return Content("ok");
            }
            else
            {

                return Content("no");
            }
        }
        #endregion
        #region 个人主页
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            ViewData.Model = PersonalUserBll.LoadEntities(u =>u.Id==user.Id).FirstOrDefault();
            
            //爽币余额
            ViewBag.CoinsUsed = UserMessageBll.CoolCoinsUsed(user);
           
            return View();
        }
        #endregion
        #region 修改手机号
        public ActionResult UpdateUserMobile(string mobile, int id)
        {
            int i = PersonalUserBll.UpdateUserMobile(mobile, id);
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
        #region  修改密码
        public ActionResult UpdateUserPassword(string pwd, int id)
        {
            int i = PersonalUserBll.UpdateUserPassword(pwd, id);
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
        #region 更新个人资料  
        public bool IsUserMsgInfoPrefect(PersonalUser user)
        {
            //资料完善、给邀请人奖励5爽币
            if (user.RecommendNum == 0 && user.QQ != null && user.WeChat != null && user.Email != null && user.Birthday != null && user.Contacts != null && user.ContactNum != null)
            {
                user.RecommendNum = 1;
                PersonalUserBll.Update(user);
                UserMessage um = new UserMessage()
                {
                    UserId = user.ReferrerId,
                    Title = "邀请小伙伴—'," + user.UName + ",'注册成功'",
                    Msg = "亲爱的爽赞会员恭喜您，您邀请的'," + user.UName + ",'已经成功加入爽赞大家庭了，特此奖励5爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块哟~",
                    State = 0,
                    PayType = "1",
                    Pay = 5,
                    InTime = DateTime.Now
                };
                UserMessageBll.Add(um);
                return true;
            }
            else {
                return false;
            }                                  
        }
        public ActionResult UpdateUserMsg(string email, string update)
        {
            var user = GetCurrentUser();
            user = PersonalUserBll.LoadEntities(u => u.Id == user.Id).FirstOrDefault();
            if (update == "email")
            {
                //首次资料更新奖励2爽币
                if (user.Email==null)
                {
                    UserMessage um = new UserMessage()
                    {
                        UserId = user.Id,
                        Title = "完善个人资料-邮箱",
                        Msg = "亲爱的爽赞会员恭喜您，您的邮箱资料已经完善，特此奖励2爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块。",
                        State = 0,
                        PayType = "1",
                        Pay = 2,
                        InTime=DateTime.Now
                    };
                    UserMessageBll.Add(um);  
                }
                user.Email = email;
                PersonalUserBll.Update(user);
                IsUserMsgInfoPrefect(user);
                return Content("ok");
            }
            if (update == "birthday")
            {

                if (user.Birthday == null)
                {
                    UserMessage um = new UserMessage()
                    {
                        UserId = user.Id,
                        Title = "完善个人资料-生日",
                        Msg = "亲爱的爽赞会员恭喜您，您的生日资料已经完善，特此奖励2爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块",
                        State = 0,
                        PayType = "1",
                        Pay = 2,
                        InTime = DateTime.Now
                    };
                    UserMessageBll.Add(um);
                }
                user.Birthday = Request["birthday"];
                PersonalUserBll.Update(user);
                IsUserMsgInfoPrefect(user);
                return Content("ok");
            }
            if (update == "weChat")
            {
                if (string.IsNullOrEmpty(user.WeChat))
                {
                    UserMessage um = new UserMessage()
                    {
                        UserId = user.Id,
                        Title = "完善个人资料-微信号",
                        Msg = "亲爱的爽赞会员恭喜您，您的微信号资料已经完善，特此奖励2爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块。",
                        State = 0,
                        PayType = "1",
                        Pay = 2,
                        InTime = DateTime.Now
                    };
                    UserMessageBll.Add(um);
                }
                user.WeChat = Request["wx"];
                PersonalUserBll.Update(user);
                IsUserMsgInfoPrefect(user);
                return Content("ok");
            }
            if (update == "qq")
            {
                if (string.IsNullOrEmpty(user.QQ))
                {
                    UserMessage um = new UserMessage()
                    {
                        UserId = user.Id,
                        Title = "完善个人资料-QQ",
                        Msg = "亲爱的爽赞会员恭喜您，您的QQ资料已经完善，特此奖励2爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块。",
                        State = 0,
                        PayType = "1",
                        Pay = 2,
                        InTime = DateTime.Now
                    };
                    UserMessageBll.Add(um);
                }
                user.QQ = Request["qq"];
                PersonalUserBll.Update(user);
                IsUserMsgInfoPrefect(user);
                return Content("ok");
            }
            if (update == "contactTel")
            {
                if (string.IsNullOrEmpty(user.Contacts) && string.IsNullOrEmpty(user.ContactNum))
                {
                    UserMessage um = new UserMessage()
                    {
                        UserId = user.Id,
                        Title = "完善个人资料-紧急联系人",
                        Msg = "亲爱的爽赞会员恭喜您，您的紧急联系人资料已经完善，特此奖励2爽币，请注意查收哦。想要更多的福利，请继续关注“淘福利”版块。",
                        State = 0,
                        PayType = "1",
                        Pay = 2,
                        InTime = DateTime.Now
                    };
                    UserMessageBll.Add(um);
                }
                user.Contacts = Request["name"];
                user.ContactNum = Request["tel"];
                PersonalUserBll.Update(user);
                IsUserMsgInfoPrefect(user);
                return Content("ok");
            }
            else {
                return Content("no");
            }                    
        }
      
        #endregion    
        #region 我的礼包箱
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult GiftBox()
        {
           
            return View();
        }
        public ActionResult GetGiftBox()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int totalCount = 0;
            string type = Request["type"];
            var user = GetCurrentUser();

            var data = PackageBll.GerMyPackage(user, type, pageIndex, pageSize, out totalCount)
                                .Select(p => new Package_Card()
                                {
                                    EndDate = p.EndDate,
                                    StartDate = p.StartDate,
                                    State = p.State,
                                    GameName = p.GameName,
                                    InTime = p.InTime,
                                    Id = p.Id,
                                    Url = p.Url,
                                    Card = p.Card,
                                    GiftType = p.GiftType,
                                     GiftName=p.GiftName,
                                })
                                .ToList();
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion       
        #region 我的试玩列表
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult GameDemo()
        {
           
            return View();
        }
        public ActionResult MyGameDemo()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int totalCount = 0;                  
            var data = GameDemoBll.GetMyGameDemo(GetCurrentUser(), pageIndex, pageSize, out totalCount)
                .Select(d => new MyGameDemo()
                {
                    GameDemoId=d.GameDemoId,
                    AccountId=d.AccountId,
                    GameName = d.GameName,
                    SystemName = d.SystemName,
                    Url = d.Url,                
                    Account = d.Account,
                    Pwd = d.Pwd,
                    Coins = d.Coins,
                    InTime = d.InTime,
                    Type = d.Type,
                    PassCheck=d.PassCheck,
                    Progress=d.Progress
                }).ToList();
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 我的爽币
      
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult CoolCoins()
        {
            var user = GetCurrentUser();
           
            //---------------赚爽币记录-------

            ViewBag.Pays = UserMessageBll.LoadEntities(um => um.PayType == "1" && um.UserId == user.Id).Sum(um => (int?)um.Pay);
            //------------充值记录-----------

            ViewData["payRecord"] = UserMessageBll.LoadEntities(um => um.PayType == "2" && um.UserId == user.Id && um.Pay != null).Sum(um => (int?)um.Pay);
          
            //------------消费记录-----------

            ViewData["expenseRecord"] = UserMessageBll.LoadEntities(um => um.PayType == "3" && um.UserId == user.Id).Sum(um => (int?)um.Pay);
            //-----------爽币余额-------
            ViewBag.CoolCoinsUsedCount = UserMessageBll.CoolCoinsUsed(user);
        
            return View();
        }
        /// <summary>
        /// 得到我的爽币记录
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">一夜多少条</param>
        /// <param name="total">总数量</param>
        /// <param name="payType">1-赚爽币记录；2-充值记录；3-消费记录</param>
        public ActionResult GetMyCoolCoins()
        {
            var user = GetCurrentUser();
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            string type = Request["type"];
            int totalCount = 0;
            var data = UserMessageBll.GetCoolCoins(user, pageIndex, pageSize, out totalCount, type).Select(um => new MyCoolCoins()
            {
                Id = um.Id,
                InTime = um.InTime,
                Pay = um.Pay,
                Msg = um.Msg,
                Title = um.Title,
                Memo2 = um.Memo2 == null ? "" : um.Memo2,
                Memo1 = um.Memo1==null?"":um.Memo1,
                Memo = um.Memo==null?"":um.Memo,
                OrderNo = um.OrderNo == null ? "" : um.OrderNo,
                Pays = um.Pays
            }).ToList();
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 我的推广链接
        public ActionResult ExtenLinks()
        {
            ViewData.Model = GetCurrentUser();
            return View();
        }
        #endregion       
        #region 获取我的投稿
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult Submission()
        {

            return View();
        }
        public ActionResult GetMySubmission()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int totalCount = 0;
            var user = GetCurrentUser();
            var data = UserRaidersBll.LoadPageEntities(pageSize, pageIndex, out totalCount, n => n.UserId == user.Id, false, n => n.InTime).ToList()
                                      .Select(n => new UserRaiders()
                                      {
                                          Id = n.Id,
                                          InTime = n.InTime,
                                          Title = n.Title,
                                          GameName = n.GameName,
                                          State = n.State,
                                          Rec_Hot = n.Rec_Hot
                                      }).ToList();
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion      
        #region 用户添加略投稿
        [ValidateInput(false)]
        public ActionResult Addmission(FormCollection form)
        {
            //TODO  后台用户投稿需要变动  推荐到网站首页（推荐管理），+10爽币，取消推荐后，-10爽币（可能客服不小心推荐错了，又取消了推送，之前奖励的爽币，要减掉。）
            UserRaiders raiders = new UserRaiders()
            {
                UserId = GetCurrentUser().Id,
                Title = form["title"],
                GameName = form["gamename"],
                Key = form["keyword"],
                Msg = form["area"],
                InTime = DateTime.Now,
                State = "2",
                EditTitle = null,
                Reason = null,
                Rec = "0",
                Rec_Time = DateTime.Now,
                Rec_Hot = "0",
                Rec_Hot_Time = null,
                CheckName = null,
                Source = null,
                Memo = null,
                Editor = null

            };
            if (UserRaidersBll.Add(raiders) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 获取投稿的详情
        [OutputCache(Duration=15,VaryByParam="id")]
        public ActionResult GetRaidersDetail(int id,int userId)
        {
            ViewData.Model = UserRaidersBll.LoadEntities(ur => ur.Id == id&&ur.UserId==userId).FirstOrDefault();
            return View();
        }
        #endregion  
        #region 批量删除投稿
        public ActionResult RaidersMoreDel(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty,您懂删除吗？请先选中数据！");
            }
            string[] idsList = ids.Split(',');
            List<int> allIds = new List<int>();
            foreach (var id in idsList)
            {
                allIds.Add(int.Parse(id));
            }
            if (UserRaidersBll.MoreDelteRaiders(allIds) > 0)
            {
                return Content("ok,恭喜:删除成功！");
            }
            else
            {

                return Content("no,提示：删除失败！");
            }
        }
        #endregion
        #region 获取我的礼品
        /// <summary>
        /// 全部订单
        /// </summary>
        /// <returns></returns>     
        public ActionResult OrderList()
        {
           var user = GetCurrentUser();
                ViewBag.WaitOrder = OrderBll.LoadEntities(o=>o.state=="0"&&o.UserId==user.Id).Select(o=>o.Id).Count(); //待发货的个数
                ViewBag.WaitAcceptOrder = OrderBll.LoadEntities(o => o.state == "1 " && o.UserId == user.Id).Select(o => o.Id).Count();                    
            return View();
        }
        public ActionResult MyOrderList()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "5");
            string  state=Request["state"];
            int totalCount = 0;
            var user = GetCurrentUser();
            List<Order> data = null;
            if (string.IsNullOrEmpty(state))
            {
              data= OrderBll.LoadPageEntities(pageSize, pageIndex, out totalCount, n => n.UserId == user.Id && n.IsDel == 0, false, n => n.InTime)
                           .ToList();
            }
            else {
                data = OrderBll.LoadPageEntities(pageSize, pageIndex, out totalCount, n => n.UserId == user.Id && n.IsDel == 0 && n.state == state, false, n => n.InTime)
                                .ToList();
            
            }          
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 批量删除订单
        public ActionResult OrderMoreDel(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty,提示：请选择要删除的数据!");
            }
            else 
            {
                string[] allIds = ids.Split(',');
                List<int> idList = new List<int>();
                foreach (var i in allIds)
                {
                    idList.Add(int.Parse(i));
                }
                if (OrderBll.MoreDelteOrder(idList) > 0)
                {
                    return Content("ok,恭喜：删除成功！");
                }
                else {
                    return Content("no,提示：删除失败！");
                }               
            }
        }     
        #endregion
        #region 订单信息及礼品信息
        public ActionResult TestCreatePage()
        {
            return View();
        }
        public ActionResult  OrderDetailUrl(DateTime time, int id)
        {
           // "/" + time.Day +
            //-----------详情链接----------------------------
             string  OrderDetailUrl = "/OrderDetail/" + time.Year + "/" + time.Month +  "/"+id+".html";
             return Content(OrderDetailUrl);
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        [OutputCache(Duration=15,VaryByParam="id")]
       public ActionResult OrderDetail (int id=3)
       {
            ViewData.Model=OrderBll.LoadEntities(o => o.Id == id).FirstOrDefault();
           return View();
       }
        /// <summary>
        /// 使用HTML模板静态化
        /// </summary>
        /// <returns></returns>
       // [HttpPost]
        public ActionResult UseHtmlTemplateStatic()
        {
            int id = 3;
            string strMessage = string.Empty;
            //静态页面模板路径
            string strTemplateFullPath = string.Format("{0}StaticTemplate/{1}", AppDomain.CurrentDomain.BaseDirectory, "OrderDetail.html");
            //保存静态页面的绝对路径
            string strStaticPageAbsolutePath = GetStaticPageAbsolutePath(id);
            //获取模板占位符数组
            string[] arrPlaceholder = new string[3];
            arrPlaceholder[0] = "@Model.Address_Name";
            arrPlaceholder[1] = "@Model.Address";
            arrPlaceholder[2] = "@Model.Phone";          
            //获取填充到模板中的占位符(自定义标识)所对应的数据数组
            Order entity = GetOrderModel(id);
            string[] arrReplaceContent = new string[3];
            arrReplaceContent[0] = entity.Address_Name;
            arrReplaceContent[1] = entity.Address;
            arrReplaceContent[2] = entity.Phone;         
            StaticPageHelper.GenerateStaticPage(strStaticPageAbsolutePath, strTemplateFullPath, arrPlaceholder, arrReplaceContent, out strMessage);
            return Content("使用HTML模板生成静态页面-----" + strMessage);
        }
        private Order GetOrderModel(int id)
        {
            ViewData.Model = OrderBll.LoadEntities(o => o.Id == id).FirstOrDefault();
            return ViewData.Model as Order;
        }
        /// <summary>
        /// 获取保存静态页面绝对路径
        /// </summary>        
        /// <returns></returns>
        /// 
        private string GetStaticPageAbsolutePath(int id)
        {
            //静态页面名称
            string strStaticPageName = string.Format("{0}.html", id);
            //静态页面相对路径
            string strStaticPageRelativePath = string.Format("OrderDetail\\{0}\\{1}", DateTime.Now.ToString("yyyy/MM").Replace('/', '\\'), strStaticPageName);
            //静态页面完整路径                                    
            string strStaticPageAbsolutePath = AppDomain.CurrentDomain.BaseDirectory + strStaticPageRelativePath;
            return strStaticPageAbsolutePath;
        }

        #endregion      
        #region 订单确认收货
        /// <summary>
        /// 0/待发货、1、待收货/2、已收货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AcceptOrderInfo(int id)
        {
            PersonalUser user = GetCurrentUser();
           var  order= OrderBll.LoadEntities(o => o.UserId == user.Id && o.state == "1" && o.Id == id).FirstOrDefault();
            order.state ="2";//确认收货
           order.ReceiveTime = DateTime.Now;
           if (OrderBll.Update(order))
           {
               return Content("ok");
           }
           else {
               return Content("no");
           }                           
        }
        #endregion
        #region 消息中心
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult MsgCenter()
        {                   
            return View();
        }
        public ActionResult MyMsgCenter()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            int totalCount = 0;
            var user = GetCurrentUser();
            var data = UserMessageBll.LoadPageEntities(pageSize, pageIndex, out totalCount, um => um.UserId == user.Id && (um.IsDel == "0"||um.IsDel==null), false, um => um.InTime)
                .Select(um => new
                {
                    um.Title,
                    um.Id,
                    um.Msg,
                    um.InTime,
                    um.State
                }).ToList();
            var msg = UserMessageBll.LoadEntities(um => um.UserId == user.Id &&(um.IsDel == "0" || um.IsDel == null)).Select(um => um.Id).Count();
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, totalCount);
            var result = new { Data = data, NavStr = NavStr, allCount = msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 玩家上传头像
         public ActionResult UploadUserImg(FormCollection form)
         {
             var bigFile =Request.Files["headImg"];//上传头像
             string imgPath = form["real_headimg"];
             var  user = GetCurrentUser();
             var userImg = PersonalUserBll.LoadEntities(c => c.Id == user.Id).FirstOrDefault();
             if (bigFile.ContentLength > 0)
             {
                 // var file = Request.Files[0];
                 //拿到文件的扩展名
                 string extName = Path.GetExtension(bigFile.FileName);
                 if (bigFile.ContentType == "image/jpeg" || bigFile.ContentType == "image/jpg" || bigFile.ContentType == "image/png"
                    )
                 {
                     Random r = new Random();
                     //给文件取新名字
                     string fileName = DateTime.Now.ToString("yyyy-MM-dd") + r.Next(100, 1000) + extName;
                     //用户文件夹的物理路径（绝对路径）                   
                     String virthPath = "/Content/UserImg/" + fileName;
                     String name = Server.MapPath(virthPath);
                     bigFile.SaveAs(name);                 
                     userImg.Head = fileName;
                     PersonalUserBll.Update(userImg);
                     return Content(fileName);
                 }
                 else
                 {
                     return Content("typeError");
                 }
             }
             else if (imgPath != "")
             {
                 userImg.Head = imgPath.Substring(13);
                 PersonalUserBll.Update(userImg);
                 return Content(imgPath.Substring(13));
             }
             else
             {
                 return Content("empty");
             }
         }
        #endregion
        #region 消息中心消息已读
         /// <summary>
         /// 消息中心消息已读,0标识未阅读。1标识已经阅读
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public ActionResult AlreadyReadMsg(string ids)
         {

             if (string.IsNullOrEmpty(ids))
             {
                 return Content("empty,提示：您会阅读吗？");
             }
             else
             {
                 string[] allIds = ids.Split(',');
                 List<int> idList = new List<int>();
                 foreach (var i in allIds)
                 {
                     idList.Add(int.Parse(i));
                 }
                 if (UserMessageBll.MoreMsgRead(idList) > 0)
                 {
                     return Content("ok,已阅读，状态已更新！");
                 }
                 else
                 {
                     return Content("no,提示：状态更新失败！");
                 }
             }

         }
         #endregion
        #region 批量删除消息
         public ActionResult MoreDelMsg(string ids)
         {
             if (string.IsNullOrEmpty(ids))
             {
                 return Content("empty,提示：请选择要删除的数据!");
             }
             else
             {
                 string[] allIds = ids.Split(',');
                 List<int> idList = new List<int>();
                 foreach (var i in allIds)
                 {
                     idList.Add(int.Parse(i));
                 }
                 if (UserMessageBll.MoreDelMsg(idList) > 0)
                 {
                     return Content("ok,恭喜：删除成功！");
                 }
                 else
                 {
                     return Content("no,提示：删除失败！");
                 }
             }
         }
         #endregion
        #region 攻略投稿修改
         public ActionResult MissionEdit(int id)
         {
             var user = GetCurrentUser();
             //--------拿取投稿的详情-------------------
             var data = UserRaidersBll.LoadEntities(ur => ur.Id == id && ur.UserId == user.Id).FirstOrDefault();

             return Json(data, JsonRequestBehavior.AllowGet);
         }
         [ValidateInput(false)]
         public ActionResult RaidersEdit(int id, FormCollection form)
         {
             var user = GetCurrentUser();
             var raiders = UserRaidersBll.LoadEntities(ur => ur.Id == id && ur.UserId == user.Id).FirstOrDefault();
             raiders.Title = form["title"];
             raiders.GameName = form["gamename"];
             raiders.Key = form["keyword"];
             raiders.Msg = form["area"];
             raiders.State = "2";
             if (UserRaidersBll.Update(raiders))
             {
                 return Content("ok:恭喜,更新成功!请您等待审核！");
             }
             else
             {
                 return Content("no:提示,更新失败！请稍候再试！");
             }

         }
         #endregion
        #region 写入充值记录
         /// <summary>
         /// 充值
         /// </summary>
         /// <param name="trade_no">订单号</param>
         /// <param name="fee">充值金额人民币（元）</param>
         /// <param name="memo">爽币条数</param>
         /// <param name="feetype">微信支付|支付宝支付</param>
         //public ActionResult Recharge(string userid, string trade_no, int fee, string memo, string feetype)
         //{

         //    int i = PersonalUserBll.Recharge(userid, trade_no, fee, memo, feetype);
         //    if (i > 0)
         //    {
         //        return Content("ok");
         //    }
         //    else
         //    {
         //        return Content("no");
         //    }

         //}
         #endregion
        #region 攻略删除
         public ActionResult RaidersDel(int id)
         {

             UserRaiders raiders = new UserRaiders() { Id = id };
             if (UserRaidersBll.Delete(raiders))
             {
                 return Content("ok");
             }
             else
             {
                 return Content("no");
             }
         } 
         #endregion
        
         public ActionResult Recharge()
         {
             return View();
         }

    }
}
