using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IOpenServiceDal : IBaseDal<OpenService>
    {
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
        /// <summary>
        /// 删除开服并且归还条数
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        bool ReturnServiceNum(int serviceId, int companyId);
       
    }
}
