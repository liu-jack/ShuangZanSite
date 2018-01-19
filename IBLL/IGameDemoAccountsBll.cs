using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IGameDemoAccountsBll : IBaseBll<GameDemoAccounts>
    {
        IQueryable<GameDemoAccounts> LoadGameDemoAccounts(Models.Params.DemoAccountsParam param);
        IList<DemoAccountModel> DemoAccountDetail(int id,string systemname,string gameAccount,string city);
        /// <summary>
        /// 删除试玩游戏
        /// </summary>
        /// <param name="gameDemoId"></param>
        /// <returns></returns>
        bool DeleteGameDemo(int gameDemoId);
        /// <summary>
        /// 删除试玩账号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        bool DeleteDemoAccount(int accountId,int gameDemoId);
        IList<string> DemoAccountCountDetail(int gameDemoId);
    }
}
