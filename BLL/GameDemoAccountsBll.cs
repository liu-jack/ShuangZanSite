using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GameDemoAccountsBll : BaseBll<GameDemoAccounts>, IGameDemoAccountsBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GameDemoAccountsDal;
        }
        public IQueryable<GameDemoAccounts> LoadGameDemoAccounts(Models.Params.DemoAccountsParam param)
        {
            var temp = dbSession.GameDemoAccountsDal.LoadEntities(a => a.Id > 0);
            if (!string.IsNullOrEmpty(param.SystemName))
            {
                temp = temp.Where(t => t.SystemName.Contains(param.SystemName));
            }
            if (!string.IsNullOrEmpty(param.Accoounts))
            {
                temp = temp.Where(t => t.UName.Contains(param.Accoounts));
            }
            if (!string.IsNullOrEmpty(param.City))
            {
                temp = temp.Where(t => t.City.Contains(param.City));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(u => u.Id)
                      .Skip(param.PageSize * (param.PageIndex - 1))
                      .Take(param.PageSize).AsQueryable();
        }
        #region 后台试玩详情
        public IList<Models.MapModel.DemoAccountModel> DemoAccountDetail(int id, string systemname, string gameAccount, string city)
        {
            StringBuilder sql = new StringBuilder("select *,(select top 1 uname from demouser inner join  PersonalUser  on DemoUser.userid=PersonalUser.id where accountid=GameDemoAccounts.id) as state,(select top 1 DemoUser.InTime from demouser inner join  PersonalUser  on DemoUser.userid=PersonalUser.id where accountid=GameDemoAccounts.id) as GetAccountIntime  from GameDemoAccounts where GameDemoId=@id ");
            List<SqlParameter> listParameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(systemname))
            {
                sql.Append(" and  GameDemoAccounts.SystemName like'%'+@systemname+'%' ");
                listParameters.Add(new SqlParameter("@systemname", SqlDbType.NVarChar) { Value = systemname });
            }
            if (!string.IsNullOrEmpty(gameAccount))
            {
                sql.Append(" and  GameDemoAccounts.UName like'%'+@gameAccount+'%' ");
                listParameters.Add(new SqlParameter("@gameAccount", SqlDbType.NVarChar) { Value = gameAccount });
            }
            if (!string.IsNullOrEmpty(city))
            {
                sql.Append(" and GameDemoAccounts.City like'%'+@city+'%' ");
                listParameters.Add(new SqlParameter("@city", SqlDbType.NVarChar) { Value = city });
            }
            sql.Append(" order  by  GameDemoAccounts.InTime  desc ");
            listParameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = id });



            SqlParameter[] pms = listParameters.ToArray();
            return dbSession.ExecuteQuery<DemoAccountModel>(sql.ToString(), pms).ToList();
        } 
        #endregion

        public bool DeleteGameDemo(int gameDemoId)                                                                                             
        {
            string sql = "delete GameDemo output deleted.img where id=@id delete GameDemoAccounts where GameDemoId=@id delete GameDemoRequires where GameDemoId=@id delete demouser where GameDemoId=@id ";
            SqlParameter[] pms = { 
                                 new   SqlParameter("@id",SqlDbType.Int){Value=gameDemoId}
                                 };
            return dbSession.ExecuteSql(sql, pms) > 0;
        }

        public bool DeleteDemoAccount(int accountId,int gameDemoId)
        {
            string sql = "delete GameDemoAccounts where GameDemoId=@demoid and id=@accountid and not exists(select 1 from demouser where accountid=GameDemoAccounts.id)";
            SqlParameter[] pms = { 
                                 new   SqlParameter("@accountid",SqlDbType.Int){Value=accountId},
                                  new   SqlParameter("@demoid",SqlDbType.Int){Value=gameDemoId}
                                 };
            return dbSession.ExecuteSql(sql, pms) > 0;
        }

        public IList<string> DemoAccountCountDetail(int gameDemoId)
        {
            string sql = "select (select  top 1 UName from demouser inner join  PersonalUser  on DemoUser.userid=PersonalUser.id where accountid=GameDemoAccounts.id )  as  state from GameDemoAccounts where GameDemoId=@demoid";
            SqlParameter[] pms = {                                 
                                  new   SqlParameter("@demoid",SqlDbType.Int){Value=gameDemoId}
                                 };
            return dbSession.ExecuteQuery<string>(sql, pms).ToList();
        }
    }
}
