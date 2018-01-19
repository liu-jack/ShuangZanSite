using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IPersonalUserSignBll : IBaseBll<PersonalUserSign>
    {
        ///// 获取已经签到的信息
        ///// </summary>
        ///// <param name="userId">当前玩家</param>
        ///// <returns></returns>
        //List<PersonalUserSign> GetSignForMon(int userId, string day);
        /// <summary>
        /// 玩家去签到
        /// </summary>
        /// <param name="userId">当前玩家</param>
        /// <returns></returns>
        List<SignViewModel> Sign(int userId, int p);

    }
}
