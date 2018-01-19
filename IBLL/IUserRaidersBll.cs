using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IUserRaidersBll : IBaseBll<UserRaiders>
    {
        int MoreCheckUserRaiders(IList<int> ids, string checkIsPass, string currentAdmin,string reason);
        int MoreDelteRaiders(IList<int> ids);
        /// <summary>
        /// 得到最新攻略
        /// </summary>
        /// <returns></returns>
        List<RaidersViewModel> GetNewestRaiders();
        /// <summary>
        /// 攻略详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<UserRaidersViewModel> GetRaidersDetails(int id);
        List<Sql_RaidersModel> GetHotRaiders();
        /// <summary>
        /// 最赞攻略
        /// </summary>
        /// <returns></returns>
        List<RaidersViewModel> GetMostGreatRaiders();
         /// <summary>
         /// 最赞攻略
         /// </summary>
         /// <returns></returns>
         List<UserRaidersViewModel> GetMostGreatRaiders(int take);
         IList<Sql_RecRaidersModel> RelatedRaiders(int top, int id);
     
    
     
    }
}
