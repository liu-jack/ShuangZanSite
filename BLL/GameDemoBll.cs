using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GameDemoBll : BaseBll<GameDemo>, IGameDemoBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GameDemoDal;
        }
        #region 一键清空
        /// <summary>
        /// 一键清除所设置的状态
        /// </summary>
        /// <param name="ids">前台接受到的全部id</param>
        /// <returns></returns>
    
        public int ClearSetState(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var demo = dbSession.GameDemoDal.LoadEntities(c => c.Id == id).FirstOrDefault();
                if (demo != null)
                {
                    demo.Rec = "0";
                    demo.Rec_Hot = "0";
                    demo.rec_HotTime = DateTime.Now;
                    demo.Rec_Time = DateTime.Now;
                    dbSession.GameDemoDal.Update(demo);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 我的试玩       
        public IQueryable<Models.MapModel.MyGameDemo> GetMyGameDemo(PersonalUser user, int pageIndex, int pageSize,out int count)
        {
            var demoUser = dbSession.DemoUserDal.LoadEntities(du => du.UserId == user.Id).Distinct();
            var demoAccounts = dbSession.GameDemoAccountsDal.LoadEntities(da => da.Id > 0);
            var gameDemo = dbSession.GameDemoDal.LoadEntities(d => d.Id > 0);
            var demoRequires = dbSession.GameDemoRequiresDal.LoadEntities(dr => dr.Id > 0);
            var data = (from da in demoAccounts
                        join du in demoUser on da.Id equals du.AccountId
                        join d in gameDemo on da.GameDemoId equals d.Id
                        let Coins = (from dr in demoRequires where dr.GameDemoId == d.Id select dr.AwardCoins).Sum()
                        let passCheck = (from duu in demoUser where duu.GameDemoId == da.GameDemoId && duu.AccountId == du.AccountId && duu.UserId == user.Id && duu.State == "1" select duu).Count()
                        let all = (from duu in demoUser where duu.GameDemoId == da.GameDemoId && duu.AccountId == du.AccountId && duu.UserId == user.Id select duu).Count()
                        select new MyGameDemo()
                        {
                            GameDemoId = da.GameDemoId,
                            AccountId = du.AccountId,
                            GameName = d.GameName,
                            SystemName = da.SystemName,
                            Url = da.Url,
                            Account = da.UName,
                            Pwd = da.Pwd,
                            Coins = Coins,
                            InTime = d.InTime,
                            Type = d.Type,
                            PassCheck = passCheck,
                            Progress = all,
                        }).Distinct();

            count = data.Count();
            return data.OrderByDescending(d => d.InTime)
                         .Skip(pageSize * (pageIndex - 1));
         
            #region sql
            //string sql = "select da.GameDemoId,du.accountid,d.Type,d.gamename,da.systemname,da.uname as  account,da.Pwd,da.url, (select count(1) from demouser where GameDemoId=da.GameDemoId and accountid=du.accountid and userid=@userid)as progress,(select count(1) from demouser where GameDemoId=da.GameDemoId and accountid=du.accountid and userid=@userid and state=1) as passCheck,(select sum(cast(AwardCoins as int)) from GameDemoRequires where GameDemoId=d.id)as Coins from (select distinct accountid from DemoUser where userid=@userid) du inner join GameDemoAccounts da on du.accountid=da.id inner join GameDemo d on d.id=da.GameDemoId   order  by  d.InTime  desc offset @pageIndex rows fetch next @pageSize row only";
            //SqlParameter[] pms = {
            //                         new SqlParameter("@userid",SqlDbType.Int){Value=user.Id},
            //                         new SqlParameter("@pageIndex",SqlDbType.Int){Value=pageIndex},
            //                         new SqlParameter("@pageSize",SqlDbType.Int){Value=pageSize},                                   
            //                     };            
            //var  data =dbSession.ExecuteQuery<MyGameDemo>(sql, pms).AsQueryable();
            //count = data.Count();
            //return data; 
            #endregion

        }
        #endregion
        #region  最强福利  
        public List<Sql_GameDemo> GetNewestGameDemo()
        {


            string sql = "select top(4) d.id,dbo.isnull(d.img,g.DescImg) as img,gamename,(select sum(AwardCoins) from GameDemoRequires where GameDemoId=d.id) as paySum from GameDemo d inner join game g on d.gamename=g.name where d.state=1 and d.rec_hot=1 order by d.rec_HotTime desc";        
         return dbSession.ExecuteQuery<Sql_GameDemo>(sql).ToList();
        }
        #endregion
        #region 淘福利游戏试玩
        /// <summary>
        /// 淘福利游戏试玩
        /// </summary>
        /// <returns></returns>
        public List<GameDemoViewModel> GetRecForumDemo()
        {
            string sql = "select top 8 img ,gameName, id , (select SUM(AwardCoins) from [dbo].[GameDemoRequires] where GameDemoId=GameDemo.Id)as PaySum  from GameDemo  where GameDemo .rec=1 and state=1 order by GameDemo.Rec_Time desc ";
            return dbSession.ExecuteQuery<GameDemoViewModel>(sql);
        } 
        #endregion
        #region 前台试玩列表
        /// <summary>
        ///前台 试玩列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
      
           public List<GameDemoViewModel> GetGameDemo(Models.Params.DemoParam param)
            {          
                var param2 = new DemoParam()
            {
                PageIndex = (param.PageIndex - 1) * param.PageSize,
                PageSize = param.PageSize,             
                GameName = param.GameName
            };
                return dbSession.GameDemoDal.GetGameDemo(param2);          
         }
           public int GetGameDemoCount()
           {
               return dbSession.GameDemoDal.GetGameDemoCount();
           }
        #endregion
        #region 淘福利试玩
           /// <summary>
           /// 试玩详情第一步
           /// </summary>
           /// <param name="id"></param>
           /// <returns></returns>
           public List<GameDemoViewModel> DemoDetailsFirst(int id)
           {
               string sql = "select d.Id,dbo.isnull(d.img,g.DescImg) as img,d.GameName,d.Type as demoType,(select sum(AwardCoins) from GameDemoRequires where GameDemoId=d.Id)as paySum,g.play,g.theme,g.Type,g.company,(select dbo.stringjoin(distinct systemname) from GameDemoAccounts where GameDemoId=d.Id and not exists (select 1 from DemoUser where AccountId=GameDemoAccounts.id))as operator  from gameDemo d inner join game g on d.gameName=g.Name and d.Id=@id";
               SqlParameter[] pms = { 
                              new SqlParameter("@id",SqlDbType.Int){Value=id},
                             
                          };
               return dbSession.ExecuteQuery<GameDemoViewModel>(sql, pms);
           }
           //淘福利_游戏试玩详情_第二步
           public List<DemoTwoViewModel> DemoDetail_Step2(int userId, int gamedemoId)
           {
               string sql = "select id,systemname,uname,pwd,url,(select top 1 userid from demouser where accountid=da.id and userid=@userId) as userid from gameDemoAccounts da where da.gamedemoid=@gamedemoId";
               SqlParameter[] pms = { 
                              new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                              new SqlParameter("@gamedemoId",SqlDbType.Int){Value=gamedemoId},
                          };
               return dbSession.ExecuteQuery<DemoTwoViewModel>(sql, pms);
           }
           //淘福利_游戏试玩详情_第三步
           public List<DemoThreeViewModel> DemoDetail_Step3(int userId, int gamedemoId, int accountId)
           {
               string sql = "select dr.id,dr.Demand,dr.AwardCoins,dr.CoinsEquity,du.img,du.state,du.userid,du.reason from gameDemoRequires dr left join demouser du on du.requiredid=dr.id and userid=@userId and accountid=@accountId where dr.gamedemoid=@gamedemoId";
               SqlParameter[] pms = { 
                              new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                              new SqlParameter("@accountId",SqlDbType.Int){Value=accountId},
                              new SqlParameter("@gamedemoId",SqlDbType.Int){Value=gamedemoId},
                          };
               return dbSession.ExecuteQuery<DemoThreeViewModel>(sql, pms);
           }
           /// <summary>
           /// 淘福利_个人领的全部帐号
           /// </summary>
           /// <param name="userId"></param>
           /// <returns></returns>
           public List<DemoViewModel> GetAlreadyNeckAllDemoAccount(int userId)
           {
               string sql = "select du.gamedemoid,d.gamename,du.accountid,da.systemname,da.uname,da.pwd from gamedemo d inner join (select distinct gamedemoid,accountid from Demouser where userid=@userId) du on d.id=du.gamedemoid inner join GameDemoAccounts da on du.accountid=da.id";
               SqlParameter[] pms = { 
                              new SqlParameter("@userId",SqlDbType.Int){Value=userId},                          
                          };
               return this.dbSession.ExecuteQuery<DemoViewModel>(sql, pms);
           } 
           #endregion
        #region 淘福利-最大充值数
           //淘福利-最大充值数
           public List<Sql_DemoMaxRecharge> DemoMaxRecharge(int userId, int gameDemoId, int accountId)
           {
               string sql = "declare @p1 int,@p2 int select @p1=max(cast(CoinsEquity as int)) from DemoUser du inner join GameDemoRequires dr on du.Gamedemoid=dr.Gamedemoid and du.requiredid=dr.id where userid=@userid and du.Gamedemoid=@demoid and accountid=@accountid and state=1 select @p2=isnull(sum(pay),0) from GameDemoRecharge  where GameDemoId=@demoid and accountid=@accountid and userid=@userid and state!=0 select @p1-@p2 as maxrecharge,sum(pay) as myPay  from usermessage where userid=@userid";
               SqlParameter[] pms = { 
                              new SqlParameter("@userid",SqlDbType.Int){Value=userId},  
                              new SqlParameter("@demoid",SqlDbType.Int){Value=gameDemoId},
                             new SqlParameter("@accountid",SqlDbType.Int){Value=accountId}
                          };
               return this.dbSession.ExecuteQuery<Sql_DemoMaxRecharge>(sql, pms);
           } 
           #endregion
        #region 淘福利-领取帐号
       public Sql_DemoModel DemoGetAccount(int demoId, int userId, string sName, string area, string city)
           {
               SqlParameter[] pms = {  
                                  new SqlParameter("@demoid",SqlDbType.Int){Value=demoId},
                                 new SqlParameter("@userid",SqlDbType.Int){Value=userId},                               
                                 new SqlParameter("@sname",SqlDbType.NVarChar){Value=sName},
                                 new SqlParameter("@userarea",SqlDbType.NVarChar){Value=area},
                                 new SqlParameter("@usercity",SqlDbType.NVarChar){Value=city},
                                 new SqlParameter("@uname",SqlDbType.NVarChar,200),
                                 new SqlParameter("@pass",SqlDbType.NVarChar,200),
                                 new SqlParameter("@systemname",SqlDbType.NVarChar,200),
                                 new SqlParameter("@url",SqlDbType.NVarChar,500),
                                 new SqlParameter("@err",SqlDbType.NVarChar,1000),
                                  };
               pms[5].Direction = ParameterDirection.Output;
               pms[6].Direction = ParameterDirection.Output;
               pms[7].Direction = ParameterDirection.Output;
               pms[8].Direction = ParameterDirection.Output;
               pms[9].Direction = ParameterDirection.Output;
               var data = dbSession.ExecuteQuery<Sql_DemoModel>("exec DemosDole @demoid, @userid,@sname,@userarea,@usercity,@uname out, @pass out,@systemname out,@url out,@err out", pms).FirstOrDefault();
               Sql_DemoModel model = new Sql_DemoModel()
               {
                   UName = pms[5].Value.ToString(),
                   Pass = pms[6].Value.ToString(),
                   Systemname = pms[7].Value.ToString(),
                   Url = pms[8].Value.ToString(),
                   Err = pms[9].Value.ToString(),
               };
               return model;
           } 
           #endregion
        #region 淘福利充值
       /// <summary>
       /// 淘福利充值
       /// </summary>
       /// <param name="gameDemoid"></param>
       /// <param name="userId"></param>
       /// <param name="accountId"></param>
       /// <param name="pay"></param>
       /// <param name="memo"></param>
       /// <returns></returns>
       public List<Sql_DemoRecharge> DemoRecharge(int gameDemoid, int userId, int accountId, int pay, string memo)
       {
           SqlParameter[] pms = {  
                                  new SqlParameter("@demoid",SqlDbType.Int){Value=gameDemoid},                                                           
                                 new SqlParameter("@accountid",SqlDbType.Int){Value=accountId},
                                  new SqlParameter("@userid",SqlDbType.Int){Value=userId},    
                                 new SqlParameter("@pay",SqlDbType.Int){Value=pay},
                                 new SqlParameter("@memo",SqlDbType.NVarChar,200){Value=memo},  
                                   new SqlParameter("@err",SqlDbType.NVarChar,1000),  
                                  };
           pms[5].Direction = ParameterDirection.Output;
           var data = dbSession.ExecuteQuery<Sql_DemoRecharge>("exec DemosRecharge @demoid,@accountid, @userid,@pay,@memo,@err out", pms).FirstOrDefault();
           List<Sql_DemoRecharge> list = new List<Sql_DemoRecharge>();
           Sql_DemoRecharge d = new Sql_DemoRecharge()
           {
               Err = pms[5].Value.ToString()
           };
           list.Add(d);
           return list;
       } 
       #endregion
    }
}
