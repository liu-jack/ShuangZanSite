using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models.MapModel;
using Models.Enum;
using Models.SqlMapModel;
using System.Text;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class OPenServicesController : Controller
    {
        //注：开测表和开服表的前台数据全在了该控制器下
        // GET: /OPenServices/
        public IOpenServiceBll OpenServiceBll { get; set; }
        public ITestBll TestBll { get; set; }
        #region 开服主页
     
        public ActionResult ServiceIndex()
        {
            //当天开服总数
            ViewBag.AllServiceCount = OpenServiceBll.allCount();
             ViewBag.AllService = OpenServiceBll.GetAllServiceInfo(1, "2");
             ViewBag.TopService = OpenServiceBll.GetAllServiceInfo(200, "1");    
            return View();
        }      
        #endregion
        #region 全天开服
        public List<Sql_OpenServer> AllDayService()
        {
           var  allDayservice= OpenServiceBll.IndexCurrntService("2", 50);  
            return allDayservice==null?null:allDayservice;
        }
        #endregion     
        #region 置顶开服           
        public List<Sql_OpenServer> _RightTopService()
        {          
           var  topService = OpenServiceBll.IndexCurrntService("1", 200);          
            return topService;
        }
        #endregion
     
     //----------------------开测-----------------------
        #region 开测表主页
        [OutputCache(CacheProfile = "ItemconfigCache")]
        public ActionResult TestIndex()
        {
            ViewBag.TodayTestCount = TestBll.TodayTestCount();
           ViewBag.WillTestCount=TestBll.WillTestCount();
           ViewBag.HistoryTestCount = TestBll.HistoryTestCount();
            //今日开测
            ViewBag.TodayTest = TestBll.TodayTestInfo();
            //将要开测
            ViewBag.WillTest = TestBll.WillTestInfo();
            ViewBag.HistoryTest = TestBll.HistoryTestInfo();
            return View();
        }
        #endregion
        #region 测试日历
        public ActionResult TestCalendar()
        {
            return View();
        }
        public ActionResult GetTestDate(string date)
        {
            var data = TestBll.TestDate(date);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 开测表搜索结果页
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult TestSearch(string key)
        {
            TempData["Key"] = key;
            Guid guidKey = Guid.NewGuid();
            object obj = Common.CacheHelper.Get(guidKey.ToString());
            List<TestViewModel> list = null;
            if (obj == null)
            {
                list = TestBll.GetSearchTestResult(key).Select(t => new TestViewModel()
                {
                    Id = t.Id,
                    GameName = t.GameName,
                    Url = t.Url,
                    PackageUrl = t.PackageUrl,
                    StartTime = t.StartTime,
                    State = t.State,
                    Play = t.Play,
                    Theme = t.Theme,
                    Company = t.Company,
                    Type = t.Type,
                    SmallImg = t.SmallImg
                }).ToList();
                Common.CacheHelper.WriteCache(guidKey.ToString(), list, DateTime.Now.AddMinutes(5));
            }
            else
            {
                list = obj as List<TestViewModel>;
            }
            ViewBag.TestSearch = list;
            return View();
        }
        #endregion
        #region 开服表搜索结果页
        public ActionResult ServiceSearch(string key)
        {
            TempData["Key"] = key;
            Guid serviceKey = Guid.NewGuid();
            object obj = Common.CacheHelper.Get(serviceKey.ToString());
            List<ServiceViewModel> list = null;
            if (obj == null)
            {
                list = OpenServiceBll.GetServiceSearchData(key).Select(s => new ServiceViewModel()
                {
                    Id = s.Id,
                    GameName = s.GameName,
                    ServerName = s.ServerName,
                    StartTime = s.StartTime,
                    GiftName = s.GiftName,
                    SystemName = s.SystemName,
                    Url = s.Url,
                    Type = s.Type,
                    PackageCardCount = s.PackageCardCount
                }).ToList();
                if (list != null)
                {
                    Common.CacheHelper.WriteCache(serviceKey.ToString(), list, DateTime.Now.AddMinutes(10));
                }
            }
            else
            {

                list = obj as List<ServiceViewModel>;

            }       
            ViewBag.ServiceFind = list;
            return View();
        }
        #endregion              
    }
}
