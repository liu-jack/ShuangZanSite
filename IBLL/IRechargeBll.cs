using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IRechargeBll : IBaseBll<Recharge>
    {
        IQueryable<Recharge> LoadRechargeTotal(RechargeParam param);
        /// <summary>
        /// 置顶开服剩余条数
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        int TopUsedNum(int companyId);
        /// <summary>
        /// 全天开服剩余条数
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        int allDayUsedNum(int companyId);
    }
}
