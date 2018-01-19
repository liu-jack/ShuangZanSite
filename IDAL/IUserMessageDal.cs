using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IUserMessageDal : IBaseDal<UserMessage>
    {
        bool UserRechargeCoolCoins(string userId, string trade_no, string rmb, string sb, string feetype);
    }
}
