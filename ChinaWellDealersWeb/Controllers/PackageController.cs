using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class PackageController : Controller
    {
        public IPackageBll PackageBll { get; set; }
        public IOpenServiceBll  OpenServiceBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        // GET: /Package/

        [OutputCache(Duration = 20)]
        public ActionResult PackageIndex()
        {
            //独家礼包
            ViewData["alonePackage"] = PackageBll.PackageRecommend(6,3);
            //开服礼包最新4条         
            ViewData["OpenServicePackage"] = PackageBll.PackageRecommend(10,1);       
            ViewData["ActivityCodePackage"] = PackageBll.PackageRecommend(10,2);                 
            return View();
        }
        #region 礼包的右侧数据
        public ActionResult _RightPackage()
        {
            //最赞攻略
            ViewData["MostGreatRaiders"] = UserRaidersBll.GetMostGreatRaiders();
            //右侧数据展示
            var otherController = DependencyResolver.Current.GetService<NewsInfoController>();
          
            ViewBag.NewsetGameDemo = GameDemoBll.GetNewestGameDemo();
            return PartialView("_RightPackage");
        } 
        #endregion
        #region 热游礼包接口
        public ActionResult PackageHot()
        {
            var data = PackageBll.PackageHot();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 礼包搜索及列表局部刷新数据
        public ActionResult PackageSearch()
        {           
            return View();
        }         
        [OutputCache(CacheProfile="ItemConfigCache")]
        public JsonResult PackageList()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "30");
            int total = 0;        
            string type = Request["type"];
            string key = Request["key"];
             string systemname = Request["systemname"];
            PackageParam param = new PackageParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Type = type,
                Key = key,
                Systemname = systemname
            };                        
                 var  list = PackageBll.LoadPackageData(param).Select(p => new PackageViewModel()
                    {
                        Id = p.Id,
                        GameName = p.GameName,
                        SystemName = p.SystemName,
                        GiftName = p.GiftName,
                        InTime = p.InTime,
                        StartTime = p.StartTime,
                        EndTime = p.EndTime,
                        Used = p.Used,
                        Type=p.Type
                    }).ToList();
                 var NavStr = Common.LaomaPager.ShowPageNavigate(param.PageSize, param.PageIndex, param.Total);
             var result = new { Data = list, NavStr = NavStr };                         
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion      
        #region 礼包详情
        public ActionResult PackageDetails(int id)
        {
            //礼包详情
            var data = PackageBll.GetPackageDetails(id);
            ViewData.Model = data as PackageViewModel2;
            //礼包游戏同名的今天开服
            ViewData["SameGameService"] = OpenServiceBll.RelatedGameService(id);
            ViewData["SameGamePackage"] = PackageBll.SameGamePackage(id);
            //同游戏的礼包
            return View();
        } 
        #endregion
        #region 礼包领号接口
        public ActionResult GetpackageNumber(int packageId, int userId)
        {
            var data = PackageBll.GetPackageNumber(packageId, userId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetpackageNumberTwo(int packageId, int userId)
        {
            Sql_PackageCode code;
            var result = PackageBll.GetPackageNumber(packageId, userId, out code);
            var data = new { data = result, code = code };
            return Json(data, JsonRequestBehavior.AllowGet);
        } 
        #endregion
    }
}
