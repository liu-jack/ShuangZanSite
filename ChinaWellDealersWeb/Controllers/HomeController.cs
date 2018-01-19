using BLL;
using Common;
using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using ShuangZan.Web.Alipay;
using ShuangZan.Web.WxPayAPI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ThoughtWorks.QRCode.Codec;
using WxPayAPI;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class HomeController : Controller
    {
        #region 根据属性去创造对象
        public IAdvertisementBll AdvertisementBll { get; set; }
        public IHomePageBll HomePageBll { get; set; }
        public INewsBll NewsBll { get; set; }
        public ITestBll TestBll { get; set; }
        public IPackageBll PackageBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        public IBeautifulGirlsBll BeautifulGirlsBll { get; set; }
        public IGameBll GameBll { get; set; }
        public IOpenServiceBll OpenServiceBll { get; set; }
        public ICompanyBll CompanyBll { get; set; }
        public ISeoBll SeoBll { get; set; }
        IGiftBll GiftBll { get; set; }
        IGameDemoRequiresBll GameDemoRequiresBll { get; set; }
        IPersonalUserSignBll PersonalUserSignBll { get; set; }
        IPersonalUserSignDetailBll PersonalUserSignDetailBll { get; set; }
        IDemoUserBll DemoUserBll { get; set; }
        IUserMessageBll UserMessageBll { get; set; }
        IFeedbackBll FeedbackBll { get; set; }
        #endregion
        #region 得到当前用户的信息
        public ActionResult GetCurrentUserInfo()
        {
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int noReadmsg = 0;
            if (user == null)
            {
                user = null;
            }
            else
            {
                //状态未读，未删除的
                noReadmsg = UserMessageBll.LoadEntities(um => um.State == 0 && um.UserId == user.Id && (um.IsDel == "0" || um.IsDel == null)).Select(um => um.Id).Count();
            }
            //未读消息
            // var noReadmsg = UserMessageBll.LoadEntities(um => um.State == 0&&um.UserId==user.Id).Select(um => um.Id).Count();
            return Json(new { User = user, NoReadMsg = noReadmsg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 网站默认首页数据展示
        /// <summary>
        /// 网站默认首页数据展示
        /// </summary>
        /// <returns></returns>
        /// 
        [OutputCache(Duration = 65)]

        public ActionResult Index()
        {
            ViewData["smallImg"] = AdvertisementBll.GetAllTypeAdvert("2", 9);
            ViewData["advert"] = AdvertisementBll.GetAllTypeAdvert("3", 2);
            ViewBag.IndexBigImg = AdvertisementBll.GetAllTypeAdvert("1", 1);
            ViewBag.SiYe = AdvertisementBll.GetAllTypeAdvert("4", 1);
            ViewData["recGame"] = HomePageBll.GetAllTypeHomePage("11", 11);
            ViewData["joinCpy"] = HomePageBll.GetAllTypeHomePage("12", 13);
            ViewData["SlideShow"] = HomePageBll.GetAllTypeHomePage("7", 10);
            ViewData["FiveTopArea"] = HomePageBll.GetAllTypeHomePage("10", 5);

            //网站首页所有新闻类型
            ViewData["allTypeNews"] = NewsBll.GetAllNewsIndex();
            //开测
            ViewData["TestInfo"] = TestBll.TestDataTen();
            //最爽礼包
            ViewData["NewestCoolPackage"] = PackageBll.NewestCoolPackage();
            //最热游戏
            ViewData["NewestHotGame"] = HomePageBll.GetAllTypeHomePage("13", 5);
            // 最强福利
            ViewBag.NewsetGameDemo = GameDemoBll.GetNewestGameDemo();
            //直播热点
            ViewData["DirectHot"] = HomePageBll.GetAllTypeHomePage("5", 1);
            ViewData["threeDirectHot"] = NewsBll.AccondingTypeGetNews("5");
            //手游
            ViewData["MobileGame"] = HomePageBll.GetAllTypeHomePage("6", 1);
            ViewData["threeMobileGame"] = NewsBll.AccondingTypeGetNews("6");
            //最赞攻略结合游戏库拿游戏的logo图
            ViewData["MostGreatRaiders"] = UserRaidersBll.GetMostGreatRaiders(10);
            //福利美图
            ViewData["NewestIndexRecGirls"] = BeautifulGirlsBll.NewestIndexRecGirls();
            //热游排行
            ViewBag.InLikeNumHotGame = GameBll.InLikeNumHotGame();
            //开服
            ViewBag.sevenData = OpenServiceBll.WillSevenDayService();
            //资讯排行
            ViewBag.LikeNumNews = NewsBll.InLikeNumNews();
            ViewBag.RecCpy = CompanyBll.RecCompany();
            return View();
        }
        public ActionResult rightServer()
        {
            var serviceController = DependencyResolver.Current.GetService<OPenServicesController>();
            var topService = serviceController._RightTopService();
            var allDayService = serviceController.AllDayService();
            return Json(new { topService = topService, allDayService = allDayService }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 红字头条和灰字头条接口
        public ActionResult RedAndGrayHomePage()
        {
            var redData = HomePageBll.GetAllTypeHomePage("8", 2);
            var grayData = HomePageBll.GetAllTypeHomePage("9", 6);
            var result = new { redData = redData, grayData = grayData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 厂商列表接口

        public ActionResult GetCompanyList()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            int total = 0;
            string key = Request["key"];
            string py = Request["py"];
            var data = CompanyBll.CompanyList(pageIndex, pageSize, out total, key, py);
            var NavStr = Common.LaomaPager.ShowPageNavigate(pageSize, pageIndex, total);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 网站公用顶部
        public ActionResult _TwoShuangzanHeader()
        {
            ViewBag.RichMedia = AdvertisementBll.GetAllTypeAdvert("5", 2);
            return PartialView("_TwoShuangzanHeader");
        }
        #endregion
        #region 厂商列表
        public ActionResult Manufacturer()
        {
            ViewBag.RecCompany = CompanyBll.ForumRecCompany();
            ViewBag.SeoCpy = SeoBll.GetSeoData(3, "1");
            ViewData["advert"] = AdvertisementBll.GetAllTypeAdvert("3", 2);
            return View();
        }
        #endregion
        #region 厂商详情
        /// <summary>
        /// 厂商详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CpyDetails(int id)
        {
            int gameCount = 0;
            ViewData.Model = CompanyBll.GetCpyDetails(id, out gameCount);
            ViewBag.gameCount = gameCount;
            //厂商相关的新游、热游、手游新闻
            ViewBag.CurrentCpyNews = CompanyBll.CurrentCpyNews(id);
            //当前厂商相关开服
            ViewBag.CurrentCpyService = OpenServiceBll.CurrentCpyService(id);
            //当前厂商礼包
            ViewBag.Package = CompanyBll.CurrnetCpyPackage(id);
            ViewData["advert"] = AdvertisementBll.GetAllTypeAdvert("3", 2);
            return View();
        }
        #endregion
        #region 该厂商下的联运等游戏
        /// <summary>
        /// 该厂商下的联运等游戏
        /// </summary>
        /// <returns></returns>
        public ActionResult JoinGame()
        {
            int companyId = int.Parse(Request["id"]);
            string type = Request["type"];
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "12");
            int total = 0;
            var param = new GameParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Type = type,
                CompanyId = companyId
            };
            var data = CompanyBll.currentCpyGame(param).ToList();
            string strNav = LaomaPager.ShowPageNavigate(param.PageSize, param.PageIndex, param.Total);
            var result = new { Data = data, NavStr = strNav };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 淘福利首页
        public ActionResult AmoyWelFare()
        {
            List<GameDemoViewModel> list = GameDemoBll.GetRecForumDemo();
            ViewData["GameDemo"] = list;
            List<Gift> giftlist = GiftBll.NewestPublishGift();
            ViewBag.NewestGift = giftlist;
            return View();
        }
        #endregion
        #region 前台游戏试玩列表
        public ActionResult DemoList()
        {

            return View();
        }
        public ActionResult GetDemoList()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "15");
            string gameName = Request["gameName"];
            var param = new DemoParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                GameName = gameName
            };
            var data = GameDemoBll.GetGameDemo(param);
            int total = GameDemoBll.GetGameDemoCount();
            var NavStr = Common.LaomaPager.ShowPageNavigate(param.PageSize, param.PageIndex, total);
            var result = new { Data = data, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 淘福利试玩详情
        //试玩第一步
        [HttpGet]
        public ActionResult DemoDetails()
        {
            return View();
        }
        //试玩详情第一步
        [HttpPost]
        public ActionResult DemoDetails(int id)
        {
            var data = GameDemoBll.DemoDetailsFirst(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //试玩第二步
        public ActionResult DemoDetail_Step2()
        {
            int userId = int.Parse(Request["userId"]);
            int gamedemoId = int.Parse(Request["gamedemoId"]);
            var data = GameDemoBll.DemoDetail_Step2(userId, gamedemoId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //试玩第三步
        public ActionResult DemoDetail_Step3()
        {
            string guid = Request["userId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }
            int gamedemoId = int.Parse(Request["gamedemoId"]);
            int accountId = int.Parse(Request["accountId"]);
            var data = GameDemoBll.DemoDetail_Step3(userId, gamedemoId, accountId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 淘福利_个人领的全部帐号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DemoAllAccount()
        {
            List<DemoViewModel> data = null;
            if (!string.IsNullOrEmpty( Request["userId"]))
            {
                string guid = Request["userId"];
                var user = Common.CacheHelper.Get(guid) as PersonalUser;
              data= GameDemoBll.GetAlreadyNeckAllDemoAccount(user.Id).ToList();
            }          
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 淘福利-最大充值数
        public ActionResult DemoMaxRecharge()
        {
            int userId = int.Parse(Request["userId"]);
            int gamedemoId = int.Parse(Request["gamedemoId"]);
            int accountId = int.Parse(Request["accountId"]);
            var data = GameDemoBll.DemoMaxRecharge(userId, gamedemoId, accountId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 淘福利-领取帐号
        public ActionResult DemoGetAccount()
        {                   
            string systemName = Request["systemName"];
            string area = "", city = "", realIP = "";
            Sql_DemoModel data=null;
            try
            {
                HttpWebRequest reqHttpWeb = (HttpWebRequest)WebRequest.Create(string.Format("http://ip.ws.126.net/ipquery?ip=", realIP));
                StreamReader responseReader = new StreamReader(reqHttpWeb.GetResponse().GetResponseStream(), System.Text.Encoding.Default);
                string json = responseReader.ReadToEnd();
                Match m = Regex.Match(json, @"var lo=""(.*?)"", lc=""(.*?)""(.*)", RegexOptions.IgnoreCase | RegexOptions.Singleline); //[\s]*
                if (m.Success)
                {
                    area = m.Result("$1");
                    city = m.Result("$2");
                }
            }
            catch { }
            if (!string.IsNullOrEmpty(Request["userId"]) && !string.IsNullOrEmpty(Request["gamedemoId"]))
            {
                int userId = int.Parse(Request["userId"]);
                int gamedemoId = int.Parse(Request["gamedemoId"]);
                data = GameDemoBll.DemoGetAccount(gamedemoId, userId, systemName, area, city);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 淘福利-充值
        public ActionResult DemoRecharge(int gameDemoid, int userId, int accountId, int pay, string memo)
        {

            var data = GameDemoBll.DemoRecharge(gameDemoid, userId, accountId, pay, memo);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 签到
        /// <summary>
        /// 获取按月签到的信息
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="day">传递的时间</param>
        /// <returns></returns>
        public ActionResult UserSignDetail(int userId, string day)
        {
           
            var data = PersonalUserSignDetailBll.InMonthlySign(day, userId);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 去签到
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult Sign(int userId)
        {
            var data = PersonalUserSignBll.Sign(userId, 1);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //获取本月已经签到的信息
        public ActionResult GetSign()
        {           
            List<SignViewModel> data=null;
            if (!String.IsNullOrEmpty(Request["userId"]))
            {
                string guid = Request["userId"];
                var user = Common.CacheHelper.Get(guid) as PersonalUser;
                 data = PersonalUserSignBll.Sign(user.Id, 0);              
            }
            data = data != null ? data : null;
            return Json(data, JsonRequestBehavior.AllowGet);  
        }
        #endregion
        #region 前台淘福利试玩截图上传
        public ActionResult GameCutImgUpload()
        {
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int gamedemoId = int.Parse(Request["gamedemoId"]);
            int accountId = int.Parse(Request["accountId"]);
            int requiredId = int.Parse(Request["requiredId"]);

            string msg = string.Empty;
            string filePath = "/Content/UserImg/";
            HttpPostedFileBase file = Request.Files["fileToUpload"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            if (str != "empty" && str != "typeError")
            {
                DemoUserBll.DemoGameCutImgUpload(gamedemoId, accountId, requiredId, user.Id, str);

                //  var model = DemoUserBll.LoadEntities(d => d.GameDemoId == gamedemoId && d.AccountId == accountId && d.RequiredId == requiredId && d.UserId == user.Id).FirstOrDefault();
                //  model.Img = str;
                //  model.State = "2";
                ////  model.InTime = DateTime.Now;
                //  DemoUserBll.Update(model);
            }

            return Content(str);


        }
        #endregion
        #region 意见反馈
        [HttpGet]
        public ActionResult FeedBack()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FeedBack(string msg, string mobile)
        {
            Feedback fb = new Feedback()
            {
                Mobile = mobile,
                Msg = msg,
                InTime = DateTime.Now
            };
            if (FeedbackBll.Add(fb) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 网站底部相关页面
        public ActionResult DemoDetails_personal()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Sbsm()
        {
            return View();
        }

        public ActionResult Sbhd()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult SiteMap()
        {
            return View();
        }
        #endregion
        #region 微信支付
        public ActionResult wxPay(string total_fee, string sb)
        {
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            if (user == null)
            {
                return Json(new { success = "false", str = "您还没登陆，请先登陆后操作！" }, JsonRequestBehavior.AllowGet);
            }
            string xml = null;
            string trade_no = WxPayApi.GenerateOutTradeNo();
            try
            {
                xml = new WXPay()
                {
                    body = "爽赞网（爽币）充值",    //费用名称，这会显示
                    detail = "爽赞网", //微信名称
                    attach = string.Format("{0},{1}", user.Id, sb), //自定义数据，这里给充值数量和用户ID
                    out_trade_no = trade_no, //商户订单号
                    total_fee = int.Parse(total_fee) * 100
                    // total_fee=1  用户测试数据
                }.pay();
                if (xml.IsNullOrEmpty())
                {
                    return Json(new { success = "false", str = "微信下单超时失败！" }, JsonRequestBehavior.AllowGet);
                }

                XElement xe = XElement.Parse(xml);
                if (xe.Element("code_url") == null)
                {
                    LogHelper.WriteLog(xml);
                    return Json(new { success = "false", str = "微信统一下单失败！" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = "true", trade_no = trade_no, url = xe.Element("code_url").Value }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                LogHelper.WriteLog(xml + "\r\n" + e.ToString());
                return Json(new { success = "false", str = "微信下单失败！" }, JsonRequestBehavior.AllowGet);
            }
        }
        //根据订单号查询订单号是否已经存在支付成功。
        public ActionResult IsPaidSuccess(string trade_no)
        {
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            var userMsg = UserMessageBll.LoadEntities(um => trade_no.Equals(um.orderNo) && um.UserId == user.Id).FirstOrDefault();
            if (userMsg.orderNo != null && userMsg.orderNo != "")
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public ActionResult PaySuccess(string num)
        {
            ViewBag.sbNum = num;
            return View();
        }
        #endregion
        #region 微信支付二维码生成
        public ActionResult WxPayPage(string data, string dd, string d)
        {
            ViewBag.QRCode = "/Home/MakeQRCode?data=" + HttpUtility.UrlEncode(data);
            ViewBag.out_trade_no = dd;
            ViewBag.sbNum = d;
            return View();
        }

        public FileResult MakeQRCode(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("data");

            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(data, Encoding.Default);
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            // image.Dispose();
            return File(ms.ToArray(), "image/jpeg");
        }

        #endregion
        #region 支付宝支付
        public ActionResult AliPay()
        {
            return View();
        }
        public string SubmitAlipay(string total_fee, string recharge)
        {
           
            string guid = Request["userId"];
            var user = Common.CacheHelper.Get(guid) as PersonalUser;

            //if (user == null)
            //{
            //    return Json(new { success = "false", str = "您还没登陆，请先登陆后操作！" }, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("service", Config.service);
                sParaTemp.Add("partner", Config.partner);
                sParaTemp.Add("seller_id", Config.seller_id);
                sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
                sParaTemp.Add("payment_type", Config.payment_type);
                sParaTemp.Add("notify_url", Config.notify_url);
                sParaTemp.Add("return_url", Config.return_url);
                sParaTemp.Add("anti_phishing_key", Config.anti_phishing_key);
                sParaTemp.Add("exter_invoke_ip", Config.exter_invoke_ip);
                sParaTemp.Add("out_trade_no", DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(9999));
                sParaTemp.Add("subject", "爽赞网（爽币）充值"); //商品名称，必填
                sParaTemp.Add("total_fee", total_fee);
                sParaTemp.Add("body", string.Format("{0},{1}", user.Id, recharge)); //商品描述，可空，这里给充值数量和用户ID
                //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
                //如sParaTemp.Add("参数名","参数值");
                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                return sHtmlText;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.ToString());
                throw new Exception("获取支付宝数据失败");
            }
        }
        #endregion
        #region 支付宝同步调用，只发生一次
        public ActionResult Return_Url()
        {
            string str = string.Empty;
            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);
                if (verifyResult)//验证成功
                {
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表
                    //商户订单号
                    string out_trade_no = Request.QueryString["out_trade_no"];
                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];
                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];
                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        string[] x = Request.QueryString["body"].Split(',');
                        ViewBag.Sb = x[1];
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        str = "支付成功";
                    }
                    else
                    {
                        //Response.Write("trade_status=" + Request.QueryString["trade_status"]);
                        str = "支付失败";
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——                  
                }
                else//验证失败
                {
                    str = "验证失败";
                }
            }
            else
            {
                str = "无返回参数";
            }
            return View();
        }
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;           
            coll = Request.QueryString;         
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        } 
        #endregion
        #region 异步调用，支付宝会在24小时内多次调用，直到成功为止
        public ActionResult Notify_Url()
        {

            SortedDictionary<string, string> sPara = GetRequestPost();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];
                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    if (Request.Form["trade_status"] == "TRADE_FINISHED")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                    }
                    else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                        //如果有做过处理，不执行商户的业务程序
                        //注意：
                        //付款完成后，支付宝系统发送该交易状态通知
                        try
                        {
                            List<string> list = new List<string>();
                            for (int i = 0; i < Request.Form.Count; i++)
                            {
                                list.Add(string.Format("key:{0},value:{1}", Request.Form.Keys[i], Request.Form[i]));
                            }
                            LogHelper.WriteLog("通知：TRADE_SUCCESS:" + string.Join("\r\n", list));
                            string[] x = Request.Form["body"].Split(',');
                            UserMessageBll.UserRechargeCoolCoins(x[0], Request.Form["out_trade_no"], Request.Form["total_fee"], x[1], "支付宝支付");
                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteLog(e.ToString());
                        }
                    }
                    else
                    {
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    Response.Write("success");  //请不要修改或删除                  
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
            return View();
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        } 
        #endregion
       
    }
}
