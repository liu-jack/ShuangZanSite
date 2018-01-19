using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GameBll : BaseBll<Game>,IGameBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GameDal;
        }
        #region 游戏入库管理    
        public IQueryable<Game> LoadGamePutLibrary(Models.Params.GameParam param)
        {
            var temp = dbSession.GameDal.LoadEntities(g => g.Id > 0);
            if (!string.IsNullOrEmpty(param.GameName))
            {
                temp = temp.Where(g => g.Name.Contains(param.GameName));
            }
            if (!string.IsNullOrEmpty(param.Alias))
            {
                temp = temp.Where(g =>  g.Alias.Contains(param.Alias));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                temp = temp.Where(g => g.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.Theme))
            {
                temp = temp.Where(g => g.Theme.Contains(param.Theme));
            }
            if (!string.IsNullOrEmpty(param.Play))
            {
                temp = temp.Where(g => g.Play.Contains(param.Play));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(u =>u.InTime)
                       .Skip(param.PageSize * (param.PageIndex - 1))
                       .Take(param.PageSize).AsQueryable();
        }
        #endregion
        #region 后台游戏推荐          
        public IQueryable<Game> Rec_GameList(Models.Params.Rec_GameParam param)
        {
            var temp = dbSession.GameDal.LoadEntities(g => g.State=="1");
            if (!string.IsNullOrEmpty(param.GameName))
            {
                temp = temp.Where(g => g.Name.Contains(param.GameName));
            }          
            if (!string.IsNullOrEmpty(param.Type))
            {
                temp = temp.Where(g => g.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.Theme))
            {
                temp = temp.Where(g => g.Theme.Contains(param.Theme));
            }
            if (!string.IsNullOrEmpty(param.Rec_Hot))
            {
                temp = temp.Where(g => g.Rec_Hot.Contains(param.Rec_Hot));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(u =>u.InTime)
                       .Skip(param.PageSize * (param.PageIndex - 1))
                       .Take(param.PageSize).AsQueryable();
        }
        #endregion
        #region 根据游戏对应的类型去拿数据
        public List<Game> GetAllTypeGame(string type, GameParam param)
        {
            var data = dbSession.GameDal.LoadEntities(g => g.Type == type).ToList().Select(g => new Game()
            {
                Name = g.Name,
                Id = g.Id,
                DescImg = g.DescImg
            }).OrderByDescending(g => g.InTime).Take(param.PageIndex).Skip(param.PageSize * (param.PageIndex - 1)).ToList();
            param.Total = data.Count();
            return data;
        } 
        #endregion
        #region 游戏详情数据
        // 同游戏名称相关的资讯
        public List<GameViewModel> GetGameRelatedNews(int id)
        {         
                var allGame = dbSession.GameDal.LoadEntities(g =>true);
                var allNews = dbSession.NewsDal.LoadEntities(n => n.Id > 0);
                var list = (from g in allGame
                        join n in allNews on g.Name equals n.Game
                        where g.Id==id&&n.State=="1"
                        select (new GameViewModel()
                        {
                            GameName=g.Name,
                            Title = n.Title,
                            EdItTitle = n.EditTitle,
                            Id = n.Id,
                            InTime =n.InTime
                        })).AsNoTracking().OrderByDescending(d => d.InTime).Take(5).ToList();               
            return list;
        }
      
        /// <summary>
        /// 根据游戏名得到相关的攻略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserRaidersViewModel> GetGameRelatedRaiders(int id)
        {
            
                var allGame = dbSession.GameDal.LoadEntities(g =>true);
                var allRaiders = dbSession.UserRaidersDal.LoadEntities(r => r.Id > 0);
              var  list = (from g in allGame
                        join r in allRaiders on g.Name equals r.GameName
                        where g.Id==id&&r.State=="1"
                        select (new UserRaidersViewModel()
                        {
                            GameName = g.Name,
                            Id = r.Id,
                            InTime =r.InTime,
                            Title = r.Title,
                            EditTitle = r.EditTitle
                        })).AsNoTracking().OrderByDescending(d => d.InTime).Take(5).ToList();
               
            return list;
        }         
        /// <summary>
        /// 与游戏相关的开服
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ServiceViewModel> GetGameRelatedService(int id)
        {
            int state = (short)CheckState.Pass;
            string pass = state.ToString();
            var allGame = dbSession.GameDal.LoadEntities(g => g.Id == id).Select(g => new { g.Name, g.Id, g.InTime });
            var allService = dbSession.OpenServiceDal.LoadEntities(s => s.State == pass)
                 .Where(s => DbFunctions.DiffDays(s.StartTime, DateTime.Now) == 0)
                .Select(s => new { s.Id, s.StartTime, s.GameName, s.CompanyId, s.Url, s.Rec_Hot });
            var allCpy = dbSession.CompanyDal.LoadEntities(c => c.State == 1).Select(c => new { c.Id, c.SystemName });
            var data = (from g in allGame
                        join s in allService on g.Name equals s.GameName
                        join c in allCpy on s.CompanyId equals c.Id
                        select (new
                        ServiceViewModel()
                        {
                            SystemName = c.SystemName,
                            Url = s.Url,
                            StartTime = s.StartTime,
                            Type = s.Rec_Hot
                        })).AsNoTracking().OrderByDescending(d => d.StartTime).Take(2).ToList();
            return data;
        }
       
        /// <summary>
        /// 与该游戏名相关的礼包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Package_Card> GetGameRelatedPackage(int id)
        {

            // --1先按照类别：独家礼包，推荐礼包，激活码（独家礼包没数据，就取推荐的，推荐的也没有，就激活码的）
            //--2、按照有效期排，倒序（比如2016-10-09开始的，接下来是2016-10-07开始的）
            //--类别序为 1-推荐礼包；2-激活码；3-独家礼包；
            //select top(@top4) p.id,c.systemname,g.name as gamename,p.name,case type when 3 then 1 when 1 then 2 when 2 then 3 end as sort 
            //from Package p inner join game g on p.gamename=g.name
            //inner join company c on p.companyid=c.id
            //where g.id=@id and p.state in (1,3) and c.state=1
            //and p.startdate<getdate() and p.enddate>cast(getdate() as date)
            //order by sort,startdate desc
            int state = (short)CheckState.Pass;
            string pass = state.ToString();
            var allGame = dbSession.GameDal.LoadEntities(g => true);
            var allpackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0);
            var allCpy = dbSession.CompanyDal.LoadEntities(c =>true);
            var data = (from g in allGame
                        join p in allpackage on g.Name equals p.GameName
                        join c in allCpy on p.CompanyId equals c.Id
                        where g.Id==id&&c.State==1
                        select (new
                       Package_Card()
                        {
                            Id=p.Id,
                            SystemName = c.SystemName,
                            GameName = g.Name,
                            GiftName = p.GiftName,
                            InTime =p.InTime
                        })).AsNoTracking().OrderByDescending(d => d.InTime).Take(2).ToList();
            return data;
        }
      
        // 该游戏下最新发布的试玩数据(福利)
        public List<Sql_GameModel> GetGameRelatedDemo(int id)
        {
            string sql = "select d.id,d.Img as img,gamename,url,(select sum(AwardCoins)  from GameDemoRequires where GameDemoId=d.id) as  paySum from gamedemo d inner join game g on d.gamename=g.name where g.id=@id  order by  d.InTime  desc";
            SqlParameter[] pms = { new SqlParameter("@id", SqlDbType.Int) { Value = id } };

            return dbSession.ExecuteQuery<Sql_GameModel>(sql, pms).ToList();
        }
        //联运
        public List<Sql_CpyModel> GetGameCpy(int id)
        {
            string sql = "select c.systemname,cg.url from CompanyGame cg inner join game g on cg.gamename=g.name inner join company c on c.id=cg.companyid where g.id=@id and c.state=1";
            SqlParameter[] pms = { new SqlParameter("@id", SqlDbType.Int) { Value = id } };

            return dbSession.ExecuteQuery<Sql_CpyModel>(sql, pms).ToList();
        }
     
       
        /// <summary>
        /// 游戏截图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Game> GetGameCutImgInfo(int id)
        {
            var allGame = dbSession.GameDal.LoadEntities(g => g.Id == id).ToList().OrderByDescending(g => g.InTime).Select(g => new Game()
            {
                CutImg = g.CutImg
            }).ToList();
            List<Game> list = new List<Game>();
            Game game = null;
            string[] imgArry;
            foreach (var item in allGame)
            {
                imgArry = item.CutImg.Split(',');
                for (int i = 0; i < imgArry.Length; i++)
                {
                    game = new Game();
                    game.CutImg = imgArry[i];
                    list.Add(game);
                }
            }
            return list;
        }
     
        #endregion
        #region 根据同类型的游戏得到相关推荐的游戏
        /// <summary>
        /// 根据同类型的游戏得到相关推荐的游戏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Game> GetGameSameTypeData(int id)
        {
            var allGame = dbSession.GameDal.LoadEntities(g => g.Id == id)
                .Select(g => new
                {
                    g.Type,
                }).ToList();
            var game = dbSession.GameDal.LoadEntities(g => g.Id > 0).Select(g => new
            {
                g.Id,
                g.Type,
                g.Name,
                g.Url,
                g.SmallImg
            }).ToList();
            var data = (from g in allGame
                        join a in game on g.Type equals a.Type
                        select new Game() { Id = a.Id, Name = a.Name, Url = a.Url,SmallImg=a.SmallImg }).OrderBy(d => Guid.NewGuid()).Take(9).ToList();
            return data;
        } 
        #endregion
    
        public List<Game> GetAllGame(GameParam  param)
        {
          List<Game> allList = dbSession.GameDal.LoadEntities(g =>g.Id>0).ToList().Select(g => new Game()
            {
                Theme = g.Theme,
                Play = g.Play,
                Name = g.Name,
                Id = g.Id,
                DescImg = g.DescImg
            }).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).ToList();
          param.Total = allList.Count();
          return allList;
        }
        #region 前台分类找游戏
        /// <summary>
        /// 前台分类找游戏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<SqlGameModel> GetAllGamesInfo(GameParam param)
        {
                      
            return dbSession.GameDal.GetAllGamesInfo(param);
            
        } 
        #endregion
        #region 是够包含该游戏名称
        /// <summary>
        /// 是够包含该游戏名称
        /// </summary>
        /// <param name="games"></param>
        /// <returns></returns>
        public bool IsContainsNewestData(string games)
        {
            List<string> list = new List<string>();
            list.Add("news");
            list.Add("hot");
            return list.Contains(games);
        }
        
        #endregion
        #region 根据点赞数查出热游排行的游戏名称
        /// <summary>
        /// 根据点赞数查出热游排行的游戏名称
        /// </summary>
        /// <returns></returns>
        public List<FrontGame> InLikeNumHotGame()
        {
            var allGame = dbSession.GameDal.LoadEntities(g => true).Select(g => new FrontGame
            {
                Id = g.Id,
                Name = g.Name,
                Type = g.Type,
                LikeNum = g.LikeNum ?? 0,
                SmallImg = g.SmallImg
            }).OrderByDescending(g => g.LikeNum).Take(10).AsNoTracking().ToList();
            return allGame;
        } 
        #endregion
       public IList<Sql_CityModel>GetAllCity()
        {
           string sql="select *   from  ip";
         return  dbSession.ExecuteQuery<Sql_CityModel>(sql).ToList();

        }



      
    }
}
