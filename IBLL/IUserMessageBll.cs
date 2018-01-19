using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IUserMessageBll : IBaseBll<UserMessage>
    {
        IQueryable<MyCoolCoins> GetCoolCoins(PersonalUser user, int pageIndex, int pageSize, out int total, string payType);
        int GetCoinsUsedCount(PersonalUser user);
        /// <summary>
        /// 第二种方式拿取爽币余额
      
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int CoolCoinsUsed(PersonalUser user);
        int MoreMsgRead(IList<int> ids);
        int MoreDelMsg(IList<int> ids);
        bool UserRechargeCoolCoins(string userId, string trade_no, string rmb, string sb, string feetype);
       
    }
}
