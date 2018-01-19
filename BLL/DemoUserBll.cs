using IBLL;
using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace BLL
{
    public class DemoUserBll : BaseBll<DemoUser>, IDemoUserBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.DemoUserDal;
        }
        #region 试玩截图审核
        /// <summary>
        /// 试玩截图审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<Models.MapModel.DemoCutImgCheckViewModel> LoadDemoCutImgCheckData(Models.Params.DemoCutImgParam param)
        {
            //1、先查出试玩账号表的数据及对其他表所对应的id
            var demoUser = dbSession.DemoUserDal.LoadEntities(du => true).Select(du => new { du.UserId,du.RequiredId,du.GameDemoId,du.AccountId,du.InTime,du.State,du.Img});
            var demoRequires = dbSession.GameDemoRequiresDal.LoadEntities(dr => dr.Id > 0);
            var demoAccounts = dbSession.GameDemoAccountsDal.LoadEntities(da => da.Id > 0);
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0);
            var gameDemo = dbSession.GameDemoDal.LoadEntities(d => d.Id > 0);
            //2、试玩表的所存在的各类表id去对应查询所需要的数据
            var data = from du in demoUser
                       join d in gameDemo on du.GameDemoId equals d.Id
                       join da in demoAccounts on du.AccountId equals da.Id
                       join dr in demoRequires on du.RequiredId equals dr.Id
                       join u in user on du.UserId equals u.Id
                       where du.Img!=null
                       select (new DemoCutImgCheckViewModel()
                       {        
                           Id = du.UserId,
                           RequireId=du.RequiredId,
                           DemoId=du.GameDemoId,
                            AccountId=du.AccountId,
                           GameName = d.GameName,//试玩游戏表游戏名称
                           SystemName = da.SystemName,//账号表
                           Account = da.UName,//账号表的试玩账号
                           UName = u.UName,//玩家用户
                           AwardCoins = dr.AwardCoins,//需求表的奖励爽币
                           CoinsEquity = dr.CoinsEquity,//需求表的奖励权益
                           Demand = dr.Demand,//需求表的要求
                           Img = du.Img,//要求表的图片
                           InTime = du.InTime,//要求表的时间
                           Type = d.Type,//试玩表中试玩类型
                           State = du.State
                       });

            if (!string.IsNullOrEmpty(param.UName))
            {
                data = data.Where(d => d.UName.Contains(param.UName));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                data = data.Where(d => d.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.SystemName))
            {
                data = data.Where(d => d.SystemName.Contains(param.SystemName));
            }
            if (!string.IsNullOrEmpty(param.GameName))
            {
                data = data.Where(d => d.GameName.Contains(param.GameName));
            }
            if (!string.IsNullOrEmpty(param.State))
            {
                data = data.Where(d => d.State.Contains(param.State));
            }
            param.Total = data.Count();
            return data.OrderByDescending(d => d.InTime).Take(param.PageSize).Skip(param.PageSize * (param.PageIndex - 1)).AsQueryable();
        } 
        #endregion
        #region 试玩详情
        /// <summary>
        /// 试玩详情
        /// </summary>
        /// <param name="demoid"></param>
        /// <param name="accountid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IQueryable<DemoCutImgCheckViewModel> LoadDemoCutImgDetails(int demoid, int accountid, int userid)
        {
            var demoUser = dbSession.DemoUserDal.LoadEntities(du => true).Select(du => new { du.UserId, du.RequiredId, du.GameDemoId, du.AccountId, du.InTime, du.State, du.Img }); 
            var demoRequires = dbSession.GameDemoRequiresDal.LoadEntities(dr => dr.Id > 0);
            var demoAccounts = dbSession.GameDemoAccountsDal.LoadEntities(da => da.Id > 0).Select(da => new { da.GameDemoId, da.SystemName, da.UName, da.Id });
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id });
            var gameDemo = dbSession.GameDemoDal.LoadEntities(d => d.Id > 0);
            var data = (from du in demoUser
                        join d in gameDemo on du.GameDemoId equals d.Id
                        join da in demoAccounts on du.AccountId equals da.Id
                        join dr in demoRequires on du.RequiredId equals dr.Id
                        //对应用户账号
                        join u in user on du.UserId equals u.Id
                        where du.GameDemoId == demoid && du.AccountId == accountid && du.UserId == userid
                        select (new DemoCutImgCheckViewModel()
                        {
                            UserId = du.UserId,
                            DemoId = du.GameDemoId,
                            AccountId = du.AccountId,
                            RequireId = du.RequiredId,
                            GameName = d.GameName,//试玩游戏表游戏名称
                            SystemName = da.SystemName,//账号表
                            Account = da.UName,//账号表的试玩账号
                            UName = u.UName,//玩家用户
                            AwardCoins = dr.AwardCoins,//需求表的奖励爽币
                            CoinsEquity = dr.CoinsEquity,//需求表的奖励权益
                            Demand = dr.Demand,//需求表的要求
                            Img = du.Img,//要求表的图片
                            InTime = du.InTime,//要求表的时间
                            Type = d.Type,//试玩表中试玩类型
                            State = du.State
                        })).AsQueryable();
            return data;
        } 
        #endregion
        #region 试玩充值列表数据
        /// <summary>
        /// 试玩充值列表数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<DemoCutImgCheckViewModel> LoadDemoRechargeCheckData(Models.Params.DemoCutImgParam param)
        {
            var demoRecharge = dbSession.GameDemoRechargeDal.LoadEntities(dr => dr.Id > 0).Select(dr => new { dr.Id,dr.Memo,dr.InTime,dr.Pay,dr.GameDemoId,dr.AccountId,dr.UserId,dr.State});
            var demoAccounts = dbSession.GameDemoAccountsDal.LoadEntities(da => da.Id > 0).Select(da => new { da.UName,da.SystemName,da.Id,da.Pwd,da.CurrentPlayer});
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0).Select(u => new { u.Id,u.UName});
            var gameDemo = dbSession.GameDemoDal.LoadEntities(d => d.Id > 0).Select(d => new { d.GameName,d.Id,d.Type});
            var data = from dr in demoRecharge
                       join d in gameDemo on dr.GameDemoId equals d.Id
                       join da in demoAccounts on dr.AccountId equals da.Id
                       join u in user on dr.UserId equals u.Id
                       select (new DemoCutImgCheckViewModel()
                       {
                           Id = dr.Id,
                           UserId=dr.UserId,
                           GameName = d.GameName,//试玩游戏表游戏名称
                           SystemName = da.SystemName,//账号表
                           Account = da.UName,//账号表的试玩账号
                           UName = u.UName,//玩家用户
                           Pwd = da.Pwd,
                           ServerName = dr.Memo,  //多少服                                              
                           InTime = dr.InTime,//要求表的时间
                           Type = d.Type,//试玩表中试玩类型
                           RechargeState = dr.State,
                           Pay = dr.Pay,
                           CurrentPlayer = da.CurrentPlayer
                       });
            if (!string.IsNullOrEmpty(param.UName))
            {
                data = data.Where(d => d.UName.Contains(param.UName));
            }
            if (!string.IsNullOrEmpty(param.Account))
            {
                data = data.Where(d => d.Account.Contains(param.Account));
            }

            if (!string.IsNullOrEmpty(param.GameName))
            {
                data = data.Where(d => d.GameName.Contains(param.GameName));
            }
            if (!string.IsNullOrEmpty(param.RechargeState))
            {
                data = data.Where(d => d.RechargeState.ToString().Contains(param.RechargeState));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                data = data.Where(d => d.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.SystemName))
            {
                data = data.Where(d => d.SystemName.Contains(param.SystemName));
            }
            param.Total = data.Count();
            return data.OrderByDescending(d => d.InTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        } 
        #endregion
        public Sql_DemoCheckModel DemoCheckEvent(int accountId, int demoId, int userId, int requireId, string state, string type, string reason)
        {
            return dbSession.DemoUserDal.DemoCheckEvent(accountId, demoId, userId, requireId, state, type, reason);
        }
        #region 前台试玩截图上传
        public bool DemoGameCutImgUpload(int gamedemoId, int accountId, int requiredId, int userId, string img)
        {
            string sql = "update DemoUser  set InTime=GETDATE(),Img=@img,State=2 where GameDemoId=@gamedemoId and AccountId=@accountId and RequiredId=@requiredId and userid=@userId";
            SqlParameter[] pms ={
                               new SqlParameter("@gamedemoId",SqlDbType.Int){Value=gamedemoId},
                               new SqlParameter("@accountId",SqlDbType.Int){Value=accountId},
                               new SqlParameter("@requiredId",SqlDbType.Int){Value=requiredId},
                               new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                               new SqlParameter("@img",SqlDbType.NVarChar){Value=img}
                               };
            return dbSession.ExecuteSql(sql, pms) > 0;
        } 
        #endregion
    }
}
