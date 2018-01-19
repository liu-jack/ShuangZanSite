using Models;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IBLL
{
    public interface IGameDemoBll : IBaseBll<GameDemo>
    {
        int ClearSetState(IList<int> ids);
        IQueryable<MyGameDemo> GetMyGameDemo(PersonalUser user,int pageIndex,int pageSize,out int count);
        /// <summary>
        /// 最强福利
        /// </summary>
        /// <returns></returns>
        List<Sql_GameDemo> GetNewestGameDemo();
        /// <summary>
        /// 拿取游戏试玩的频道首页推荐数据
        /// </summary>
        /// <returns></returns>
        List<GameDemoViewModel> GetRecForumDemo();
        /// <summary>
        /// 前台试玩列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<GameDemoViewModel> GetGameDemo(DemoParam param);
        int GetGameDemoCount();    
        List<GameDemoViewModel> DemoDetailsFirst(int id);
        List<DemoTwoViewModel> DemoDetail_Step2(int userId, int gamedemoId);
        List<DemoThreeViewModel> DemoDetail_Step3(int userId, int gamedemoId, int accountId);

        /// <summary>
        /// 得到当前玩家已领的所有试玩账号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<DemoViewModel> GetAlreadyNeckAllDemoAccount(int userId);
        /// <summary>
        /// 淘福利-最大充值数
        /// </summary>
        /// <param name="userId">当前用户</param>
        /// <param name="gameDemoId">当前试玩游戏id</param>
        /// <param name="accountId">试玩账号ID</param>
        /// <returns></returns>
        List<Sql_DemoMaxRecharge> DemoMaxRecharge(int userId, int gameDemoId, int accountId);
        /// <summary>
        /// 淘福利-领取帐号
        /// </summary>
        /// <param name="demoId"></param>
        /// <param name="userId"></param>
        /// <param name="sName">领取指定的运营商</param>
        /// <param name="area">领取用户所在省</param>
        /// <param name="city"></param>
        /// <returns></returns>
        Sql_DemoModel DemoGetAccount(int demoId, int userId, string sName, string area, string city);
       /// <summary>
        /// 淘福利-充值
       /// </summary>
       /// <param name="gameDemoid">试玩游戏id</param>
       /// <param name="userId"></param>
       /// <param name="accountId">账号id</param>
       /// <param name="pay">充值金额</param>
       /// <param name="memo">多少服</param>
       /// <returns></returns>
        List<Sql_DemoRecharge> DemoRecharge(int gameDemoid, int userId, int accountId, int pay, string memo);
    

    }
}
