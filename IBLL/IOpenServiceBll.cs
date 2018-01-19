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
    public interface IOpenServiceBll : IBaseBll<OpenService>
    {
        IQueryable<ServiceViewModel> LoadOpenService(ServiceParam param);
        int MoreCheckOpenService(IList<int> ids, string checkIsPass, string currentAdmin);  
      
        bool ReturnServiceNum(int serviceId, int companyId);
        /// <summary>
        /// 厂商开服详情
        /// </summary>
        /// <param name="id">当前的id号</param>
        /// <param name="cpy">当前的厂商用户</param>
        /// <returns></returns>      
        IQueryable<OpenService_Package> GetCpyServiceDetail(int id,Company cpy);
        int allCount();
        /// <summary>
        /// 得到全天开服的信息
        /// </summary>
        /// <returns></returns>
        List<Sql_OpenServer> GetAllServiceInfo(int take, string type);
        /// <summary>
        /// 得到开服的搜索的结果
        /// </summary>
        /// <param name="key">有关平台名或者游戏名</param>
        /// <returns></returns>
        IQueryable<ServiceViewModel> GetServiceSearchData(string key);
        List<Sql_ServerModel> RelatedGameService(int packageId);
        /// <summary>
        /// 查询最近七天开服
        /// </summary>
        /// <returns></returns>
        List<ServiceViewModel> WillSevenDayService();
        /// <summary>
        /// <获取我的开服历史游戏名称>
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<MyServerGame> MyHistoryGameName(int companyId);
        /// <summary>
        /// 1）该厂商发布的当天开服信息，最多显示4条。规则：优先全天开服的显示，有几条就显示几条，不够4条，就看是否有置顶开服条数，如果有的，就当前时间段的优先显示，接着是即将开服（正序），再是已经开服的显示（倒序）。
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<Sql_ServerModel> CurrentCpyService(int companyId);
        List<Sql_OpenServer> IndexCurrntService(string type,int take);
       
    }
}
