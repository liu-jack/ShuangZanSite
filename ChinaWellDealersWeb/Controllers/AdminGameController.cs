using IBLL;
using Models;
using Models.MapModel;
using Models.Params;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class AdminGameController : BaseController//Controller//
    {
        #region  属性注入
        public IGameBll GameBll { get; set; }
        public IKeyWordsBll KeyWordsBll { get; set; }
        public INewsBll NewsBll { get; set; }
        public ILeaveMsgBll LeaveMsgBll { get; set; }
        public ITestBll TestBll { get; set; }
        public IBeautifulGirlsBll BeautifulGirlsBll { get; set; }
        public IOrderBll OrderBll { get; set; }
        public IPersonalUserBll PersonalUserBll { get; set; }
        public IFeedbackBll FeedbackBll { get; set; }
        public IGiftBll GiftBll { get; set; }
        public IGameDemoBll GameDemoBll { get; set; }
        public IGameDemoRechargeBll GameDemoRechargeBll { get; set; }
        public IGameDemoAccountsBll GameDemoAccountsBll { get; set; }
        public IGameDemoRequiresBll GameDemoRequiresBll { get; set; }
        public ICompanyBll CompanyBll { get; set; }
        public IPackageBll PackageBll { get; set; }
        public IUserRaidersBll UserRaidersBll { get; set; }
        public IUserMessageBll UserMessageBll { get; set; }    
        #endregion
        // GET: /AdminGame/后台管理控制器
        //==============================操作管理===============================================
        #region 游戏关键词管理
     
        public ActionResult GameKeyWords()
        {
            return View();
        }    
        public ActionResult GetGameKeyWords()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
           
            GameKeyWords para = new GameKeyWords()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                KeyWords = Request["keyWords"],
                IsLibrary = Request["isLibrary"],
            };
            var data = KeyWordsBll.LoadKeyWords(para).Select(g => new { g.Id, g.Key, g.IsLibrary, g.LibraryTime }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 关键词添加及判断是否存在
        public ActionResult AddkeyWords(FormCollection form)
        {
            string keys=form["keys"];                    
            var existKeyWords = KeyWordsBll.LoadEntities(g => g.Key == keys).FirstOrDefault();
                if (existKeyWords == null)
                {
                    KeyWords keyword = new KeyWords()
                    {
                        Key = form["keys"],
                        LibraryTime = DateTime.Now,
                        IsLibrary = "0",
                    };
                    KeyWordsBll.Add(keyword);
                    return Content("ok");
                }
                else
                {
                    return Content("exist");
                }

            
        }
        #endregion
        #region 删除游戏关键词
        public ActionResult KeyWordsDel(int ids)
        {
            KeyWords key = new KeyWords()
            {
                Id = ids,
            };
            if (key != null)
            {
                if (KeyWordsBll.Delete(key))
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
        }
        #endregion
        #region  游戏关键词修改
        public ActionResult GameEdit(int id)
        {
            ViewData.Model = KeyWordsBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult GameEdit(FormCollection form, int id)
        {
            var keyUpdate = KeyWordsBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            if (keyUpdate != null)
            {
                keyUpdate.Key = form["keyWords"] as string;
              //  keyUpdate.IsLibrary = form["isLibrary"] as string;

                if (KeyWordsBll.Update(keyUpdate))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            return Content("数据为空");


        }
        #endregion
        #region 游戏入库管理
        [OutputCache(Duration = 3)]
        public ActionResult GamePutLibrary()
        {
            return View();
        }
        [OutputCache(Duration = 5)]
        public ActionResult GetGamePutLibrary()
        {

            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string gameName = Request["gameName"];
            string alias = Request["alias"];
            string type = Request["type"];
            string theme = Request["theme"];
            string play = Request["play"];
            var para = new GameParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                GameName = gameName,
                Alias = alias,
                Type = type,
                Theme = theme,
                Play = play,
            };
            var data = GameBll.LoadGamePutLibrary(para).Select(gg => new { gg.Id, gg.Type, gg.Theme, gg.Play, gg.Name, gg.Alias, gg.InTime, gg.State }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 游戏入库删除
        public ActionResult GameDel(int ids)
        {
            Game game = new Game()
            {
                Id = ids,
            };
            if (game != null)
            {
                if (GameBll.Delete(game))
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
        }
        #endregion
        #region 游戏图片上传的方法
        public ActionResult BigImgUpload()
        {
            var bigFile = Request.Files["bigImgs"];
            if (bigFile != null)
            {
                // var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(bigFile.FileName);
                if (bigFile.ContentType == "image/jpeg" || bigFile.ContentType == "image/jpg" || bigFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = Common.WebHelper.GetStreamMd5(bigFile.InputStream) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/GameImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    bigFile.SaveAs(name);
                    return Content(fileName);
                }
                else
                {
                    return Content("typeError");
                }
            }
            else
            {
                return Content("empty");
            }
        }
        public ActionResult SmallImgUpload()
        {
            var smallFile = Request.Files["smallImgs"];
            if (smallFile != null)
            {
                var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(file.FileName);
                if (smallFile.ContentType == "image/jpeg" || smallFile.ContentType == "image/jpg" || smallFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = Common.WebHelper.GetStreamMd5(smallFile.InputStream) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/GameImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    smallFile.SaveAs(name);
                    return Content(fileName);
                }
                else
                {
                    return Content("typeError");
                }
            }
            else
            {
                return Content("empty");
            }
        }
        public ActionResult DescImgUpload()
        {
            var descFile = Request.Files["descImgs"];
            if (descFile != null)
            {
                //var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(descFile.FileName);
                if (descFile.ContentType == "image/jpeg" || descFile.ContentType == "image/jpg" || descFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = Common.WebHelper.GetStreamMd5(descFile.InputStream) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/GameImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    descFile.SaveAs(name);
                    return Content(fileName);
                }
                else
                {
                    return Content("typeError");
                }
            }
            else
            {
                return Content("empty");
            }
        }
        public ActionResult LogoImgUpload()
        {
            var logoFile = Request.Files["logoImgs"];
            if (logoFile != null)
            {
                //var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(logoFile.FileName);
                if (logoFile.ContentType == "image/jpeg" || logoFile.ContentType == "image/jpg" || logoFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = Common.WebHelper.GetStreamMd5(logoFile.InputStream) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/GameImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    logoFile.SaveAs(name);
                    return Content(fileName);
                }
                else
                {
                    return Content("typeError");
                }
            }
            else
            {
                return Content("empty");
            }

        }

        #endregion
        #region 游戏截图上传
        public ActionResult GameCutImgUpload()
        {
            string msg = string.Empty;
            string filePath = "/Content/GameImg/";
            HttpPostedFileBase file = Request.Files["fileToUpload"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            return Content(str);
        }
        #endregion
        #region 礼品图片上传
        public ActionResult UploadGiftImg()
        {
            var logoFile = Request.Files["logoImgs"];
            if (logoFile != null)
            {
                //var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(logoFile.FileName);
                if (logoFile.ContentType == "image/jpeg" || logoFile.ContentType == "image/jpg" || logoFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = Common.WebHelper.GetStreamMd5(logoFile.InputStream) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/GiftImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    logoFile.SaveAs(name);
                    return Content(fileName);
                }
                else
                {
                    return Content("typeError");
                }
            }
            else
            {
                return Content("empty");
            }
        }
        public ActionResult MoreUploadGiftImg()
        {
            string msg = string.Empty;
            string filePath = "/Content/GiftImg/";
            HttpPostedFileBase file = Request.Files["fileToUpload"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            return Content(str);
        } 
        #endregion
        #region 福利美图上传             
        public ActionResult ImgUpload()
        {
            string msg = string.Empty;
            string filePath = "/Content/GirlImg/";
            HttpPostedFileBase file = Request.Files["fileToUpload"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            return Content(str);
        }
        #endregion
        #region 游戏添加
        public ActionResult AddGameLibrary()
        {
            return View();
        }
        public ActionResult AddGame(FormCollection form)
        {
           var  terms= form["game"];
            Game game = new Game()
            {
                Name = terms,
                Alias = Request["alias"] == null ? "" : Request["alias"],
                Type = form["type"],
                Theme = form["theme"],
                Play = form["play"],
                Company = form["company"],
                Operator = form["operators"],
                Url = form["url"],
                BigImg = form["bigImg"],
                SmallImg = form["smallImg"],
                DescImg = form["descImg"],
                LoGo = form["logoImg"],
                CutImg = form["cutImg"],
                State = "1",
                InTime = DateTime.Now,
                Msg = form["msg"],
                Rec = "0",
                Rec_Time = DateTime.Now,
                Rec_Hot_Time = DateTime.Now,
                Rec_Index = "0",
                Rec_Hot = "0",
                Rec_Index_Time = DateTime.Now,
                LikeNum = 0
            };
            if (GameBll.Add(game) != null)
            {
                var key = KeyWordsBll.LoadEntities(k => k.Key.Contains(terms)).FirstOrDefault();
                if (key!=null)
                {
                    key.IsLibrary = "1";
                    KeyWordsBll.Update(key);  
                }
               
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 修改入库游戏

        public ActionResult GameLibraryEdit(int id)
        {
            Game game = GameBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            Game gameImg = null;
            List<Game> list = new List<Game>();
            string[] imgArry = game.CutImg.Split(',');
            for (int i = 0; i < imgArry.Length; i++)
            {
                gameImg = new Game();
                gameImg.CutImg = imgArry[i];
                list.Add(gameImg);
            }
            ViewData.Model = game;
            ViewBag.GameCutImg = list;
            return View();
        }
        public ActionResult GameLibEdit(int id)
        {
            string terms = Request["game"];
            Game game = GameBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            game.Id = int.Parse(Request["id"].ToString());
            game.Name = Request["game"];
            game.Alias = Request["alias"] == null ? "" : Request["alias"];
            game.Type = Request["type"];
            game.Theme = Request["theme"];
            game.Play = Request["play"];
            game.Company = Request["company"];
            game.Operator = Request["operators"];
            game.Url = Request["url"];
            game.BigImg = Request["bigImg"];
            game.SmallImg = Request["smallImg"];
            game.DescImg = Request["descImg"];
            game.LoGo = Request["logoImg"];
            game.CutImg = Request["cutImg"];
            game.State = "1";
            //game.InTime = DateTime.Now;
            game.Msg = Request["msg"];         
            if (GameBll.Update(game))
            {
                var key = KeyWordsBll.LoadEntities(k => k.Key.Contains(terms)).FirstOrDefault();
                if (key != null)
                {
                    key.IsLibrary = "1";
                    KeyWordsBll.Update(key);
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 资讯（娱乐八卦）列表及模糊搜索
        [OutputCache(Duration = 5)]
        public ActionResult HappyNewsList()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }
       
        public ActionResult GetHappyNews()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            var allMsg = LeaveMsgBll.LoadEntities(l => l.Id > 0)
                                    .Select(l => new { l.NewsId });
            var allNews = NewsBll.LoadEntities(n => n.Type != "1" && n.Type != "2" && n.Type != "3")
                                 .Select(n => new { n.Id, n.InTime, n.Title, n.State, n.Type, n.CheckName, n.Views, n.AddedBy });
            var data = (from n in allNews
                       join m in allMsg on n.Id equals m.NewsId into nci
                       from nc in nci.DefaultIfEmpty()
                       let MsgNum = (from mm in allMsg where mm.NewsId == n.Id select (int?)mm.NewsId).Count()
                       select new { n.Id, n.InTime, n.Title, n.State, n.Type, n.CheckName, n.Views, MsgNum = MsgNum, n.AddedBy }).Distinct();
            string title = Request["title"];
            string type = Request["newsType"];
            string addedBy = Request["addedBy"];
            if (!string.IsNullOrEmpty(title))
            {
                data = data.Where(d => d.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(d => d.Type.Contains(type));
            }
            if (!string.IsNullOrEmpty(addedBy))
            {
                data = data.Where(d => d.AddedBy.Contains(addedBy));
            }
            int total = data.Count();
            var allData = data.OrderByDescending(d => d.InTime)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 新闻单一审核状态
        public ActionResult CheckState(int id, string checkIsPass, string currentAdmin)
        {
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
               string terms=form["terms"];
               var  news = NewsBll.LoadEntities(n => n.Id == Id).FirstOrDefault();             
                news.Title = form["title"];
                news.EditTitle = form["editTitle"];
                news.Type = form["newsType"];
                news.Editor=form["editor"];
                news.Source = form["source"];
                news.Memo = form["memo"];
                news.Msg = form["msg"];
                 news.GameTerms=terms;              
                news.InTime =news.State!="1"?DateTime.Now:news.InTime;
                if (NewsBll.Update(news))
                {
                    //更新关键词库的状态
                 var  key= KeyWordsBll.LoadEntities(k=>k.Key.Contains(terms)).FirstOrDefault();
                 if (key!=null)
                 {
                     key.IsLibrary = "1";
                     KeyWordsBll.Update(key);  
                 }
                
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
        #region  资讯添加（娱乐八卦）
        public ActionResult AddHappyNews()
        {           
            return View();
        }
        [ValidateInput(false)]
        public ActionResult AddHappyNewsInfo(FormCollection form)
        {
            try
            {
                string terms = form["terms"];
                News news = new News()
                {
                    CompanyId = 0,
                    Title = form["title"],
                    EditTitle = form["editTitle"] == null ? null : form["editTitle"],
                    Source = form["source"],
                    Editor = form["editor"],
                    Kewords = form["kewords"],
                    Memo = form["memo"],//摘要
                    Msg = form["msg"],
                    Type = form["type"],
                    Game = null,
                    InTime = DateTime.Now,
                    State = "2",
                    Rec_Forum_Index = "0",
                    Rec_Forum_Time = DateTime.Now,
                    Rec_Hot_Index = "0",
                    Rec_Hot_Time = DateTime.Now,
                    CheckName = null,
                    AddedBy = GetCurrentAdmin().UName,
                    Views = 0,
                    LeaveMsgId = 0,                    
                    GameTerms = terms,
                    Tip=0,
                    Stamp=0,                                      
                };
                if (news != null)
                {
                    NewsBll.Add(news);
                    //更新关键词库的状态
                    var key = KeyWordsBll.LoadEntities(k => k.Key.Contains(terms)).FirstOrDefault();
                    if (key!=null)
                    {
                        key.IsLibrary = "1";
                        KeyWordsBll.Update(key); 
                    }                   
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
           
          
           
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
        #endregion
        #region 开测测表及模糊搜索
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult GetTest()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string gameName = Request["gameName"];


            TestParam para = new TestParam()
             {
                 PageSize = pageSize,
                 PageIndex = pageIndex,
                 Total = total,
                 gameName = gameName,
             };
            var data = TestBll.LoadPageTests(para).Select(t => new { t.Id, t.GameName, t.InTime, t.State, t.StartTime }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 开测表删除
        public ActionResult DelTest(int ids)
        {
            Tests test = new Tests()
            {
                Id = ids
            };
            if (TestBll.Delete(test))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 开测表信息添加
        public ActionResult AddTestInfo()
        {

            return View();
        }
        public ActionResult AddTest(FormCollection form)
        {
            string name = Request["gamename"];
            DateTime startday = DateTime.Parse(Request["startday"]);
            string state = Request["state"];
            string url = Request["url"];
            string packageUrl = Request["packageUrl"];
            Tests test = new Tests()
            {
                GameName = name,
                StartTime = startday,
                State = state,
                Url = url,
                Package = packageUrl,
                InTime = DateTime.Now
            };
            if (TestBll.Add(test) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 开测表编辑

        public ActionResult TestEdit(int id)
        {
            ViewData.Model = TestBll.LoadEntities(t => t.Id == id).FirstOrDefault();
            return View();
        }

        public ActionResult TestsEdit(int id)
        {
            var test = TestBll.LoadEntities(t => t.Id == id).FirstOrDefault();
            string name = Request["gamename"];
            DateTime startday = DateTime.Parse(Request["startday"].ToString());
            string state = Request["state"];
            string url = Request["url"];
            string packageUrl = Request["packageUrl"];
            test.GameName = name;
            test.StartTime = startday;
            test.State = state;
            test.Url = url;
            test.Package = packageUrl;
            if (TestBll.Update(test))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 福利美图管理
        [OutputCache(Duration = 10)]
        public ActionResult BeautifulGirlsList()
        {
            return View();
        }
        public ActionResult GetBeautifulGirls()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string title = Request["title"];
            string addedBy = Request["addedBy"];
            GirlsParam param = new GirlsParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                Title = title,
                AddedBy = addedBy,
            };
            var data = BeautifulGirlsBll.LoadGirlSInfo(param).Select(g => new GirlsViewModel()
            {
                Id = g.Id,
                Image = g.Image,
                Views = g.Views,
                InTime = g.InTime,
                Title = g.Title,
                AddedBy = g.AddedBy,
                LeaveMsgCount = g.LeaveMsgCount,
                ImageCount = g.ImageCount,
                Rec = g.Rec,
                Rec_Time = g.Rec_Time
            }).ToList();
            var result = new { total = param.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 福利美图删除
        public ActionResult DelBeautiful(int id)
        {
            BeautifulGirls girls = new BeautifulGirls()
            {
                Id = id
            };
            if (BeautifulGirlsBll.Delete(girls))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 添加福利美图
      
        public ActionResult AddBeautifulGirls()
        {
            return View();
        }
        public ActionResult AddGirls()
        {
            //string  allTags = Request["tag"];
            //string [] arryTags  = allTags.Split(',');
            //int a = 0;            
            //for (int i = 0; i <arryTags.Length; i++)
            //{
            //    a += Convert.ToInt32(arryTags[i]);
            //}
            BeautifulGirls girls = new BeautifulGirls()
            {
                Title = Request["title"],
                Tags = Request["tag"],
                Imgs = Request["oldimgs"],
                InTime = DateTime.Now,
                Rec = 0,
                Rec_Time = DateTime.Now,
                LeadTxt = Request["guideMsg"],
                Views = 0,
                AddedBy = GetCurrentAdmin().UName
            };
            if (BeautifulGirlsBll.Add(girls) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 福利美图编辑
        [HttpGet]
        public ActionResult BeautifulgirlEdit(int id)
        {
            //ViewData.Model 
            var girl = BeautifulGirlsBll.LoadEntities(b => b.Id == id).FirstOrDefault();
            ViewData.Model = girl as BeautifulGirls;
            string[] arryImgs = girl.Imgs.Split(',');
            ViewData["arryTags"] = arryImgs;
            BeautifulGirls bb = null;
            List<BeautifulGirls> list = new List<BeautifulGirls>();
            for (int i = 0; i < arryImgs.Length; i++)
            {
                bb = new BeautifulGirls();
                bb.Imgs = arryImgs[i];
                list.Add(bb);
            }
            ViewData["ImgsList"] = list;
            return View();
        }
        [HttpPost]
        public ActionResult BeautifulgirlEdit(int id, FormCollection form)
        {
            var girl = BeautifulGirlsBll.LoadEntities(b => b.Id == id).FirstOrDefault();
            girl.Id = id;
            girl.Title = form["title"];
            girl.Imgs = form["oldimgs"];
            girl.Tags = form["tag"];
            girl.InTime = DateTime.Now;
            girl.LeadTxt = form["guideMsg"];
            if (BeautifulGirlsBll.Update(girl))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 订单管理
        public ActionResult OrderList()
        {
            return View();
        }
        public ActionResult GetOrder()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string name = Request["uName"];
            string giftName = Request["giftName"];
            string state = Request["state"];
            var allUser = PersonalUserBll.LoadEntities(p => p.Id > 0).Select(pp => new { pp.Id, pp.UName });
            var allOrder = OrderBll.LoadEntities(o => true);
            var data = from o in allOrder
                       join u in allUser on o.UserId equals u.Id                  
                       select new { o.Id, o.InTime, o.Price, o.color, o.Address_Name, o.Address, o.Name, o.Phone, o.SendTime, u.UName, o.state, o.Num, o.Order_Num, o.Type };

            if (!string.IsNullOrEmpty(name))
            {
                data = data.Where(t => t.UName.Contains(name));
            }
            if (!string.IsNullOrEmpty(giftName))
            {
                data = data.Where(t => t.Name.Contains(giftName));
            }
            if (!string.IsNullOrEmpty(state))
            {
                data = data.Where(t => t.state.Contains(state));
            }
            total = data.Count();
            data = data.OrderByDescending(d => d.InTime)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize);
            var result = new { total = total, rows = data.ToList() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 关于订单发货
        public ActionResult SendOrder(int id)
        {
            ViewData.Model = OrderBll.LoadEntities(o => o.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult SendOrder(FormCollection form, int id)
        {
            //<option value="0">已付款</option>
            //<option value="1">已发货</option>
            //<option value="2">已收货</option>
            var order = OrderBll.LoadEntities(o => o.Id == id).FirstOrDefault();
            // order. Id=id;
            order.Order_Num = form["order_Num"];//orderType
            order.Type = form["orderType"];
            order.SendTime = DateTime.Now;
            order.state = "1";
            if (OrderBll.Update(order))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 意见反馈
        public ActionResult Feedback()
        {
            return View();
        }
        public ActionResult GetFeedback()
        {
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string key = Request["key"];
            FeedbackParam param = new FeedbackParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Key = key,
            };

            var data = FeedbackBll.LoadFeedback(param).Select(f => new { f.Id, f.Msg, f.Mobile, f.InTime });
            //if (!string.IsNullOrEmpty(key))
            //{
            //    data = data.Where(t => t.Msg.Contains(key));
            //}

            // total = data.Count();

            var result = new { total = param.Total, rows = data };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  意见删除
        public ActionResult DelFeedback(int id)
        {
            Feedback feedback = new Feedback()
            {
                Id = id
            };
            if (FeedbackBll.Delete(feedback))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 礼品管理
        public ActionResult GiftList()
        {
            return View();
        }
        public ActionResult GetGift()
        {
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string giftName = Request["giftName"];
            string state = Request["state"] as string;
            GiftParam param = new GiftParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                GiftName = giftName,
                State = state,
            };
            var data = GiftBll.LoadGift(param).Select
                (g => new { g.Id, g.Name, g.State, g.Price, g.Stock, g.Logo, g.Intime }).ToList();
            var result = new { total = param.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 删除礼品
        public ActionResult DelGift(int id)
        {
            Gift gift = new Gift()
            {
                Id = id
            };
            if (GiftBll.Delete(gift))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 添加礼品
        public ActionResult AddGift()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult AddGrifts(FormCollection form)
        {
            // int price = int.Parse(form["price"].ToString());
            try
            {
                string imgs = form["oldimgs"];
                string name = Request["name"];
                var price = Convert.ToInt32(form["price"].Trim());
                int oldPrice = int.Parse(form["oldPrice"]);
                int stock = int.Parse(form["stock"]);
                string color = form["color"];//产品规格
                string logo = form["logo"];
                string explain = form["explain"];
                string msg = form["msg"];
                Gift gift = new Gift()
                    {
                        Name = name,
                        Price = price,
                        OldPrice = oldPrice,
                        State = "1",
                        Stock = stock,
                        Color = color,
                        Logo = logo,
                        Explain = explain,
                        Imgs = imgs,
                        Msgs = msg,
                        Intime = DateTime.Now
                    };

                if (GiftBll.Add(gift) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }

            }
            catch (Exception ex)
            {

                throw new Exception("抛异常" + ex.Message);
            }
        }
        #endregion
        #region 编辑礼品
        public ActionResult GiftEdit(int id)
        {
            Gift gift = GiftBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            ViewData.Model = gift as Gift;
            Gift gg = null;
            List<Gift> list = new List<Gift>();
            string[] arryImgs = gift.Imgs.Split(',');
            for (int i = 0; i < arryImgs.Length; i++)
            {
                gg = new Gift();
                gg.Imgs = arryImgs[i];
                list.Add(gg);
            }
            ViewData["ImgsList"] = list;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GiftEdit(FormCollection form)
        {
            int id = int.Parse(form["id"]);
            Gift g = GiftBll.LoadEntities(ggg => ggg.Id == id).FirstOrDefault();
            g.Id = id;
            g.Name = form["name"];
            g.Color = form["color"];
            g.State = "1";
            g.Stock = int.Parse(Request["stock"]);
            g.Price = int.Parse(form["price"]);
            g.OldPrice = int.Parse(form["oldPrice"]);
            g.Explain = form["explain"];
            g.Logo = form["logo"];
            g.Imgs = form["oldImgs"];
            g.Msgs = form["msg"];
            g.Intime = DateTime.Now;
            if (GiftBll.Update(g))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 改变礼物状态
        public ActionResult ChangeGiftState(int id)
        {
            Gift gift = new Gift()
            {
                Id = id,
            };
            if (gift != null)
            {
                var giftState = GiftBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前用户状态等于0的改变为1
                if (giftState.State == "0")
                {
                    giftState.State = "1";

                    if (GiftBll.Update(giftState))
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
                    giftState.State = "0";
                    if (GiftBll.Update(giftState))
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
        #region 游戏试玩管理
        public ActionResult GameDemoList()
        {
            ViewData.Model = GetCurrentAdmin();
            return View();
        }
        public ActionResult GetGameDemo()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;            
            var demo = GameDemoBll.LoadEntities(d => true).Select(d => new { d.Id, d.GameName, d.CheckName, d.Type, d.InTime, d.state });
            var demoRequeire = GameDemoRequiresBll.LoadEntities(r => true);                    
            var data = from d in demo                      
                       let PaySum = (from r in demoRequeire where d.Id == r.GameDemoId select (int?)r.AwardCoins).Sum()
                       select new { d.state, d.Id, d.InTime, d.GameName, d.CheckName, d.Type, Pay = PaySum ?? 0,  };
            string gamename = Request["gameName"];
            string type = Request["type"];
            if (!string.IsNullOrEmpty(gamename))
            {
                data = data.Where(t => t.GameName.Contains(gamename));
            }
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(t => t.Type.Contains(type));
            }
            total = data.Count();
            var allData = data.OrderByDescending(d => d.InTime)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //删除游戏试玩
        public ActionResult DelGameDemo(int id)
        {
            //  <删除试玩游戏>
            // <sql><![CDATA[delete demo
            //output deleted.img
            //where id=@id
            //delete DemoAccounts where demoid=@id
            //delete DemoRequires where demoid=@id
            //delete demouser where demoid=@id]]></sql>
            //  </删除试玩游戏>          
            if (GameDemoAccountsBll.DeleteGameDemo(id))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //游戏试玩状态审核
        public ActionResult GameDemoCheckState(int id, string checkIsPass, string currentAdmin)
        {

           var  demo = GameDemoBll.LoadEntities(n => n.Id == id).FirstOrDefault();
                if (checkIsPass == "1")
                {
                    demo.state = 1;
                    demo.CheckName = currentAdmin;
                    GameDemoBll.Update(demo);
                    return Content("ok");
                }
                else if (checkIsPass == "0")
                {
                    demo.state = 0;
                    demo.CheckName = currentAdmin;
                    GameDemoBll.Update(demo);
                    return Content("ok");
                }
                else { 
                 return Content("no");
                }        
        }
        #endregion
        #region 游戏试玩批量添加
        public ActionResult AddGameDemo()
        {
            return View();
        }
        [ValidateInput(false)]
        public JsonResult AddGameDemos(List<string> accounts, List<string> requires)
        {
            string type = Request["type"];
            string name = Request["name"];
            string img = Request["img"];
            GameDemo demo = new GameDemo()
            {
                GameName = name,
                Type = type,
                Img = img,
                state = 0,
                rec_HotTime = DateTime.Now,
                Rec_Hot = "0",
                Rec_Time = DateTime.Now,
                Rec = "0",
                InTime = DateTime.Now,
                CheckName = null
            };
            if (GameDemoBll.Add(demo) != null)
            {
                GameDemoAccounts demoAccount = null;
                for (int i = 0; i < accounts.Count; i++)
                {
                    GameDemoAccounts demoAccounts = JsonConvert.DeserializeObject<GameDemoAccounts>(accounts[i]);
                    demoAccount = new GameDemoAccounts();
                    demoAccount.GameDemoId = demo.Id;
                    demoAccount.SystemName = demoAccounts.SystemName;
                    demoAccount.Pwd = demoAccounts.Pwd;
                    demoAccount.UName = demoAccounts.UName;
                    demoAccount.City = demoAccounts.City;
                    demoAccount.Url = demoAccounts.Url;
                    demoAccount.InTime = DateTime.Now;
                    GameDemoAccountsBll.Add(demoAccount);
                }
                GameDemoRequires require = null;
                for (int i = 0; i < requires.Count; i++)
                {
                    GameDemoRequires demoRequires = JsonConvert.DeserializeObject<GameDemoRequires>(requires[i]);
                    require = new GameDemoRequires();
                    require.GameDemoId = demo.Id;
                    require.Demand = demoRequires.Demand;
                    require.AwardCoins = demoRequires.AwardCoins;
                    require.CoinsEquity = demoRequires.CoinsEquity;
                    GameDemoRequiresBll.Add(require);
                }
            }
            return Json(new { success = "true" });
        }
        #endregion
        #region 试玩账号及试玩要求
        //当点击游戏试玩表的游戏名城时、将对应的试玩id传到试玩账号表和要求表 、将对应的数据查询出来
        public ActionResult DemoAccounts_Requires()
        {
            int id = int.Parse(Request["id"]);
            ViewBag.CurrentId = id;
            string systemName = Request["systemName"];
            string gameAccount = Request["gameAccount"];
            string city = Request["citylist"];
            var user = PersonalUserBll.LoadEntities(u => true);
            var requires = GameDemoRequiresBll.LoadEntities(r => r.GameDemoId==id).ToList();          
            ViewData["requires"] = requires;      
            ViewData["accounts"] = GameDemoAccountsBll.DemoAccountDetail(id, systemName, gameAccount, city);
            return View();
        }
        public ActionResult GameDemoRequires(int id)
        {
            var data = GameDemoRequiresBll.LoadEntities(d => d.Id > 0).ToList();

            return View();
        }
        #endregion
        #region 试玩账号添加
        public ActionResult AddDemoAccounts(int id)
        {
            ViewBag.GameDemoId = id;
            return View();
        }
        public ActionResult AddDemoAccountInfo(FormCollection form)
        {
            GameDemoAccounts accounts = new GameDemoAccounts()
            {
                GameDemoId=int.Parse(form["gameDemoId"]),
                SystemName = form["systemname"],
                Url = form["url"],
                UName = form["uName"],
                Pwd = form["pwd"],
                City = form["citylist"],
                InTime = DateTime.Now
            };
            if (GameDemoAccountsBll.Add(accounts) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 删除账号
        public ActionResult DelDemoAccount(int accountId,int demoId)
        {
            // <后台删除试玩帐号>
            // <sql><![CDATA[delete DemoAccounts where demoid=@demoid and id=@accountid
            //and not exists(select 1 from demouser where accountid=DemoAccounts.id)]]></sql>
            //  </后台删除试玩帐号>
           // GameDemoAccounts accounts = new GameDemoAccounts() { Id = accountId ,GameDemoId=demoId};
            if (GameDemoAccountsBll.DeleteDemoAccount(accountId,demoId))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 游戏入库的状态改变事件
        public ActionResult GameStateChange()
        {
            int id = int.Parse(Request["id"]);
            string state = Request["gameState"];
            Game game= GameBll.LoadEntities(g => g.Id == id).FirstOrDefault();
            game.State = state;
            if (GameBll.Update(game))
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }            
        }
        #endregion
        public ActionResult DemoAccountCountDetail(int gameDemoId)
        {
          var data= GameDemoAccountsBll.DemoAccountCountDetail(gameDemoId);       
          return Json(new { accountCount = data.Count, data = data }, JsonRequestBehavior.AllowGet);
        }
        //==============================推荐管理===============================================
        #region 新闻推荐
        [OutputCache(Duration = 10)]
        public ActionResult Rec_News()
        {
            return View();
        }
      
        public ActionResult Rec_GetNews()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            //表关联拿到相应的id
            var allNews = NewsBll.LoadEntities(n => n.State=="1")
                                 .Select(n => new { n.Id, n.InTime, n.CompanyId, n.Game, n.Title, n.Type, n.Rec_Hot_Index, n.Rec_Forum_Index, n.EditTitle,n.AddedBy,n.Kewords });
            var cpy = CompanyBll.LoadEntities(c => c.State==1)
                                .Select(c => new { c.Id, c.SystemName });
            var data = from a in allNews
                       join c in cpy on a.CompanyId equals c.Id into  aci
                       from  ac  in  aci.DefaultIfEmpty()
                       select new NewsViewModel()
                       {
                         Id=  a.Id,
                          InTime= a.InTime,
                          Title= a.Title,
                          EditTitle= a.EditTitle,
                          Game= a.Game,
                          Type= a.Type,
                          SystemName=ac!=null?ac.SystemName:null,
                          Rec_Hot_Index= a.Rec_Hot_Index,
                           Rec_Forum_Index=a.Rec_Forum_Index,
                           AddedBy=a.AddedBy,
                           Kewords=a.Kewords
                       };

            string title = Request["title"];
            string cpyName = Request["cpyName"];
            string newsType = Request["newsType"];
            string rec_Forum = Request["rec_Forum"];
            string rec_Hot = Request["rec_Hot"];
            string addedBy = Request["addedBy"];           
            string keyWords = Request["keyWords"];

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
            if (!string.IsNullOrEmpty(addedBy))
            {
                data = data.Where(d => d.AddedBy.Contains(addedBy));
            }
           
            if (!string.IsNullOrEmpty(keyWords))
            {
                data = data.Where(d => d.Kewords.Contains(keyWords));
            }
            if (!string.IsNullOrEmpty(rec_Hot))
            {
                data = data.Where(d => d.Rec_Hot_Index.Contains(rec_Hot));
            }

            if (!string.IsNullOrEmpty(rec_Forum))
            {
                data = data.Where(d => d.Rec_Forum_Index == rec_Forum);
            }
            total = data.Count();
            var allData = data              
                 .OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion      
        #region 资讯推荐首页、版块状态改变
        public ActionResult RecMsgState(int id, string recParam)
        {
            News news = new News()
              {
                  Id = id,
              };
            if (recParam == "recHot")
            {
                var newsState = NewsBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (newsState.Rec_Hot_Index == "0")
                {
                    newsState.Rec_Hot_Index = "1";
                    newsState.Rec_Hot_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
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
                    newsState.Rec_Hot_Index = "0";
                    newsState.Rec_Hot_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "recForum")
            {
                var newsState = NewsBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (newsState.Rec_Forum_Index == "0")
                {
                    newsState.Rec_Forum_Index = "1";
                    newsState.Rec_Forum_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
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
                    newsState.Rec_Forum_Index = "0";
                    newsState.Rec_Forum_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }

        #endregion
        #region 新闻推荐状态改变
        public ActionResult RecNewsState(int id, string recParam)
        {
            News news = new News()
            {
                Id = id,
            };
            if (recParam == "recHot")
            {
                var newsState = NewsBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (newsState.Rec_Hot_Index == "0")
                {
                    newsState.Rec_Hot_Index = "1";
                    newsState.Rec_Hot_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
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
                    newsState.Rec_Hot_Index = "0";
                    newsState.Rec_Hot_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "recForum")
            {
                var newsState = NewsBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (newsState.Rec_Forum_Index == "0")
                {
                    newsState.Rec_Forum_Index = "1";
                    newsState.Rec_Forum_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
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
                    newsState.Rec_Forum_Index = "0";
                    newsState.Rec_Forum_Time = DateTime.Now;
                    if (NewsBll.Update(newsState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }
        #endregion
        #region 礼包推荐
        public ActionResult Rec_Package()
        {
            return View();
        }
        public ActionResult Rec_GetPackage()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var Package = PackageBll.LoadEntities(r =>r.State=="1"||r.State=="3")
                                           .Select(p => new { p.Id, p.InTime, p.GiftName, p.GameName, p.Type, p.ServerName, p.StartDate, p.EndDate, p.Msg, p.CompanyId, p.Rec, p.Rec_Hot, p.Rec_Index });
            var cpy = CompanyBll.LoadEntities(c =>c.State==1).Select(c => new { c.Id, c.SystemName });
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
                           p.Rec,
                           c.SystemName,
                           p.Rec_Hot,
                           p.Rec_Index
                       };
            string giftName = Request["giftName"];
            string cpyName = Request["cpyName"];
            string giftType = Request["giftType"];
            string gameName = Request["gameName"];
            string rec = Request["rec"];
            string rec_hot = Request["rec_hot"];
            string rec_index = Request["rec_index"];
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
            if (!string.IsNullOrEmpty(gameName))
            {
                data = data.Where(d => d.GameName.Contains(gameName));
            }

            if (!string.IsNullOrEmpty(rec))
            {
                data = data.Where(d => d.Rec.Contains(rec));
            }
            if (!string.IsNullOrEmpty(rec_hot))
            {
                data = data.Where(d => d.Rec_Hot.Contains(rec_hot));
            }
            if (!string.IsNullOrEmpty(rec_index))
            {
                data = data.Where(d => d.Rec_Index.Contains(rec_index));
            }
            total = data.Count();
            var allData = data
                 .OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 礼包 热门 、首页、普通推荐状态改变
        public ActionResult Rec_Packagestate(int id, string recParam)
        {
            Package package = new Package()
            {
                Id = id,
            };
            if (recParam == "rec")
            {
                var packageState = PackageBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (packageState.Rec == "0")
                {
                    packageState.Rec = "1";
                    packageState.Rec_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
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
                    packageState.Rec = "0";
                    packageState.Rec_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "recHot")
            {
                var packageState = PackageBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (packageState.Rec_Hot == "0")
                {
                    packageState.Rec_Hot = "1";
                    packageState.Rec_Hot_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
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
                    packageState.Rec_Hot = "0";
                    packageState.Rec_Hot_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "recIndex")
            {
                var packageState = PackageBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (packageState.Rec_Index == "0")
                {
                    packageState.Rec_Index = "1";
                    packageState.Rec_Index_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
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
                    packageState.Rec_Index = "0";
                    packageState.Rec_Index_Time = DateTime.Now;
                    if (PackageBll.Update(packageState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }
        #endregion
        #region 攻略推荐
        public ActionResult Rec_UserRaiders()
        {
            return View();
        }
        public ActionResult Rec_GetUserRaiders(DateTime? inTime)
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            //个人用户表（personuser）和UserRaiders关联
            var allUserRaider = UserRaidersBll.LoadEntities(ur =>ur.State=="1")
                .Select(ur => new { ur.State, ur.Id, ur.InTime, ur.Title, ur.UserId, ur.Rec_Hot, ur.GameName, ur.Rec, ur.EditTitle });
            var allPersonUser = PersonalUserBll.LoadEntities(p =>p.State==1).Select(p => new { p.Id, p.UName });
            var data = from ur in allUserRaider
                       join u in allPersonUser on
                       ur.UserId equals u.Id
                       select new { ur.Id, ur.InTime, ur.Title, ur.Rec_Hot, ur.GameName, u.UName, ur.Rec, ur.EditTitle };
            string uName = Request["uName"];
            string game = Request["game"];
            string title = Request["title"];         
            string rec = Request["rec"];
            string rechot = Request["rec_Hot"];
            if (!string.IsNullOrEmpty(uName))
            {
                data = data.Where(d => d.UName.Contains(uName));
            }
            if (!string.IsNullOrEmpty(game))
            {
                data = data.Where(d => d.GameName.Contains(game));
            }
            if (!string.IsNullOrEmpty(title))
            {
                data = data.Where(d => d.Title.Contains(title));
            }
            if (inTime.HasValue)
            {
                data = data.Where(d => DbFunctions.DiffDays(d.InTime, inTime) == 0);               
            }
            if (!string.IsNullOrEmpty(rec))
            {
                data = data.Where(d => d.Rec.Contains(rec));
            }
            if (!string.IsNullOrEmpty(rechot))
            {
                data = data.Where(d => d.Rec_Hot.Contains(rechot));
            }
            total = data.Count();
            data = data.OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize);                
            var result = new { total = total, rows =data.ToList()};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 关于攻略（频道、网站首页）推荐状态
        //后期可以加上事务处理、取消推荐到首页的原因未写
        public ActionResult RecRaideersState(int id, string recParam)
        {
            UserRaiders raiders = new UserRaiders()
            {
                Id = id,
            };
            if (recParam == "rec")
            {
                var raidersState = UserRaidersBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (raidersState.Rec == "0")
                {
                    raidersState.Rec = "1";
                    raidersState.Rec_Time = DateTime.Now;
                    if (UserRaidersBll.Update(raidersState))
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
                    raidersState.Rec = "0";
                    raidersState.Rec_Time = DateTime.Now;
                    if (UserRaidersBll.Update(raidersState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            //网站首页推荐
            else if (recParam == "recIndex")
            {
                var raidersState = UserRaidersBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (raidersState.Rec_Hot == "0")
                {
                    raidersState.Rec_Hot = "1";
                    raidersState.Rec_Hot_Time = DateTime.Now;
                    if (UserRaidersBll.Update(raidersState))
                    {
                        //2、----将审核通过的消息写入到userMessage--
                        UserMessage um = new UserMessage();
                        um.UserId = raidersState.UserId;
                        um.Title = "攻略投稿—推送到首页<<" + raidersState.Title + ">>";
                        um.Msg = "亲爱的爽赞会员恭喜您，您投稿的攻略《" + raidersState.Title + "》非常优秀，已经被推送到首页啦，特此奖励10爽币。希望您在之后的日子里面，多多投稿，帮助更多的玩家朋友哦。";
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
                    else
                    {
                        return Content("fail");
                    }
                }
                else
                {
                    raidersState.Rec_Hot = "0";
                    raidersState.Rec_Hot_Time = DateTime.Now;
                    if (UserRaidersBll.Update(raidersState))
                    {
                        //2、----将通取消推荐到网站首页的消息写入到userMessage--
                        UserMessage um = new UserMessage();
                        um.UserId = raidersState.UserId;
                        um.Title = "攻略投稿—取消推送到首页<<" + raidersState.Title + ">>";
                        um.Msg = "亲爱的爽赞会员您好，由于“xxx”的原因，您的攻略《" + raidersState.Title + "》已经被取消推送到首页。如有什么疑问可直接联系在线客服哦";
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
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }

        #endregion
        #region 游戏推荐
        public ActionResult Rec_Game()
        {
            return View();
        }
        public ActionResult Rec_GetGame()
        {

            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string gameName = Request["gameName"];
            string type = Request["type"];
            string theme = Request["theme"];
            string rec_hot = Request["rec_hot"];
            Rec_GameParam para = new Rec_GameParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                GameName = gameName,
                Type = type,
                Theme = theme,
                Rec_Hot = rec_hot
            };
            var data = GameBll.Rec_GameList(para).Select(gg => new { gg.Id, gg.Type, gg.Theme, gg.Name, gg.InTime, gg.Rec_Hot }).ToList();
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region   推荐游戏最热状态改变

        public ActionResult Rec_GameChangeState(int id)
        {
            Game game = new Game()
            {
                Id = id,
            };
            var gameState = GameBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            //当前状态等于0的改变为1
            if (gameState.Rec_Hot == "0")
            {
                gameState.Rec_Hot = "1";
                gameState.Rec_Hot_Time = DateTime.Now;
                if (GameBll.Update(gameState))
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
                gameState.Rec_Hot = "0";
                gameState.Rec_Hot_Time = DateTime.Now;
                if (GameBll.Update(gameState))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("fail");
                }

            }
        }
        #endregion
        #region 游戏试玩推荐
        public ActionResult Rec_GameDemo()
        {
            return View();
        }
        public ActionResult Rec_GetGameDemo()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            //游戏试玩表和游戏试玩充值表关联 拿到充值爽币
            var demo = GameDemoBll.LoadEntities(d =>d.state==1).Select(d => new { d.Id, d.GameName, d.Type, d.InTime, d.Rec, d.Rec_Time, d.Rec_Hot, d.rec_HotTime });
             var  demoRequeire=  GameDemoRequiresBll.LoadEntities(r => true);    
            var data = from d in demo
                       let PaySum = (from r in demoRequeire where d.Id == r.GameDemoId select (int?)r.AwardCoins).Sum()
                       select new { d.Rec, d.Rec_Time, d.Rec_Hot, d.rec_HotTime, d.Id, d.InTime, d.GameName, d.Type, Pay = PaySum ?? 0 };
            string gamename = Request["gameName"];
            string type = Request["type"];
            string rec = Request["rec"];
            string rec_hot = Request["rec_hot"];

            if (!string.IsNullOrEmpty(gamename))
            {
                data = data.Where(t => t.GameName.Contains(gamename));
            }
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(t => t.Type.Contains(type));
            }
            if (!string.IsNullOrEmpty(rec))
            {
                data = data.Where(t => t.Rec.Contains(rec));
            }
            if (!string.IsNullOrEmpty(rec_hot))
            {
                data = data.Where(t => t.Rec_Hot.Contains(rec_hot));
            }
            total = data.Count();
            var allData = data.OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 游戏试玩（频道、首页）推荐单一状态改变
        public ActionResult Rec_GameDemoState(int id, string recParam)
        {
            GameDemo demo = new GameDemo()
            {
                Id = id,
            };
            if (recParam == "recHot")
            {
                var demoState = GameDemoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (demoState.Rec_Hot == "0")
                {
                    demoState.Rec_Hot = "1";
                    demoState.rec_HotTime = DateTime.Now;
                    if (GameDemoBll.Update(demoState))
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
                    demoState.Rec_Hot = "0";
                    demoState.rec_HotTime = DateTime.Now;
                    if (GameDemoBll.Update(demoState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "rec")
            {
                var demoState = GameDemoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (demoState.Rec == "0")
                {
                    demoState.Rec = "1";
                    demoState.Rec_Time = DateTime.Now;
                    if (GameDemoBll.Update(demoState))
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
                    demoState.Rec = "0";
                    demoState.Rec_Time = DateTime.Now;
                    if (GameDemoBll.Update(demoState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }
        #endregion
        #region 福利美图推荐
        public ActionResult Rec_BeautifulGirls()
        {
            return View();
        }
        #endregion
        #region 美图推荐状态改变
        public ActionResult Rec_BeautifulState(int id)
        {
            BeautifulGirls girls = new BeautifulGirls()
             {
                 Id = id,
             };
            var girlsState = BeautifulGirlsBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            //当前状态等于0的改变为1
            if (girlsState.Rec == 0)
            {
                girlsState.Rec = 1;
                girlsState.Rec_Time = DateTime.Now;
                if (BeautifulGirlsBll.Update(girlsState))
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
                girlsState.Rec = 0;
                girlsState.Rec_Time = DateTime.Now;
                if (BeautifulGirlsBll.Update(girlsState))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("fail");
                }

            }
        }
        #endregion
        #region 一键清空游戏试玩状态
        public ActionResult ClearGameDemoState(string ids)
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
            if (GameDemoBll.ClearSetState(idList) > 0)
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
}
