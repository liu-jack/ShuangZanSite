using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IPersonalUserSignDetailBll : IBaseBll<PersonalUserSignDetail>
    {
        /// <summary>
        /// 获取按月签到的信息
        /// </summary>
        /// <param name="day">传递过来的当前日期</param>
        /// <param name="userId">当前玩家</param>
        /// <returns></returns>
        List<string> InMonthlySign(string day, int userId);
    }
}
