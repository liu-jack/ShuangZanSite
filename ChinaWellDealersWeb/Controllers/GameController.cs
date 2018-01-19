using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models;
using Models.Enum;
using Models.Params;
using Models.MapModel;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class GameController : Controller
    {
        public IGameBll GameBll { get; set; }
        ISeoBll SeoBll { get; set; }
        IKeyWordsBll KeyWordsBll { get; set; }
        #region 定义公共的分页属性
        public GameParam CommonPaging()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "25");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            var param = new GameParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total
            };
            return param;
        }
        #endregion
        #region 角色扮演
        public ActionResult RolePlay()
        {
            var param = CommonPaging();
            string rolePlay = GameType.RolePlay.ToString();
            ViewData["RolePlay"] = GameBll.GetAllTypeGame(rolePlay, param).ToList();
            ViewData["pageIndex"] = param.PageIndex;
            ViewData["pageSize"] = param.PageSize;
            ViewData["Total"] = param.Total;
            return View();
        }
        #endregion       
        #region 找游戏主页
        public ActionResult GameIndex()
        {
            //轮播推荐区：总后台游戏推荐管理，热门推荐。最多显示8个
            ViewBag.RecHotGame = GameBll.LoadEntities(g => g.State == "1" && g.Rec_Hot == "1").OrderByDescending(g=>g.Rec_Hot_Time).Select(g => new GameViewModel2() { 
                    Id=g.Id,
                    BigImg=g.BigImg            
                            }).Take(8).ToList();
            //seo推荐游戏
            ViewBag.RecSeoGame = SeoBll.GetSeoData(3, "2");
            return View();
        }
        #endregion
        #region 热门游戏  异步加载     
        public ActionResult  GameAllData()
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "25");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            string games = Request["games"];
            string type = Request["type"];
            string theme = Request["theme"];
            string play = Request["play"];
            string letter = Request["letter"];
            string key = Request["key"];
        
            GameParam param = new GameParam()
            {
                PageIndex = pageIndex,
                PageSize=pageSize,
                Total=total,
                GameName=key,
                Games=games,
                Theme=theme,
                Type=type,
                Play=play,
                Letter=letter,               
            };
         var    list = GameBll.GetAllGamesInfo(param).ToList();
                       
            var NavStr = Common.LaomaPager.ShowPageNavigate(param.PageSize,param.PageIndex, param.Total);
            var result = new { Data = list, NavStr = NavStr };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 游戏搜索
        public ActionResult GameSearch(string key)
        {
            int pageSize = int.Parse(Request["pageSize"] ?? "25");
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int total = 0;
            Guid cacheKey = Guid.NewGuid();
            List<Game> list = null;
            object obj = Common.CacheHelper.Get(cacheKey.ToString());
            if (obj == null)
            {
                list = GameBll.LoadPageEntities(pageSize, pageIndex, out total, g => g.Name.Contains(key) || g.Alias.Contains(key), false, g => g.InTime).ToList().Select(g => new Game()
                              {
                                  Name = g.Name,
                                  Id = g.Id,
                                  DescImg = g.DescImg
                              }).ToList();
                if (list != null)
                {
                    try
                    {
                        Common.CacheHelper.WriteCache(cacheKey.ToString(), list, DateTime.Now.AddMinutes(10));
                    }
                    catch (Exception)
                    {
                        throw new Exception("缓存异常,需要处理！");
                    }
                }
            }
            else
            {
                list = obj as List<Game>;
            }
            //搜索结果
            ViewData["GameSearch"] = list;
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageSize"] = pageSize;
            ViewData["Total"] = total;
            return PartialView("GameSearch");
        }
        #endregion
        #region 游戏详情
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult GameDetail(int id)
        {
            ViewData.Model = GameBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            ViewBag.GameRelatedNews = GameBll.GetGameRelatedNews(id);
            ViewBag.GameRelatedRaiders = GameBll.GetGameRelatedRaiders(id);
           
            //相关开服
            ViewBag.GameRelatedService = GameBll.GetGameRelatedService(id);
            ViewData["GameRelatedPackage"] = GameBll.GetGameRelatedPackage(id);
            //福利--游戏试玩
            ViewBag.GameRelatedDemo = GameBll.GetGameRelatedDemo(id);
            //游戏截图
            ViewBag.GameCutImg = GameBll.GetGameCutImgInfo(id);
            //该游戏相关推荐同类型游戏随机显示
            ViewBag.GameRelatedType = GameBll.GetGameSameTypeData(id);
            //联运
            ViewBag.CpyGame = GameBll.GetGameCpy(id);
            return View();
        }
        #endregion
        #region 入库的游戏
        public ActionResult GameNameListPartial()
        {
            var data = GameBll.LoadEntities(n => n.Id > 0).Select(n => new GameName { Name = n.Name }).ToList();
            ViewBag.GameName = data;
            return PartialView("GameNameListPartial");
        }   
        #endregion  
        public ActionResult GameLikePlusOne(int id)
        {
            var game = GameBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            if (game != null)
            {
                game.LikeNum = game.LikeNum==null?1:game.LikeNum + 1;
                if (GameBll.Update(game))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else {
                return Content("noExist");
            }
        }
        //入库的关键词
        public ActionResult _GameTerms()
        {
            var data = KeyWordsBll.LoadEntities(k => true).ToList();
            ViewBag.GameTerms = data;
            return PartialView("_GameTerms");
        }
        //入库的城市
        public ActionResult _City()
        {
           ViewBag.AllCity=GameBll.GetAllCity();
           return PartialView("_City");
        }
        public ActionResult _CityDataList()
        {
            ViewBag.AllCity = GameBll.GetAllCity();
            return PartialView("_CityDataList");
        }
    }
}
