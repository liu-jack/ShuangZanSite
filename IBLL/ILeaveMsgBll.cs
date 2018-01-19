using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IBLL
{
    public interface ILeaveMsgBll : IBaseBll<LeaveMsg>
    {
        IQueryable<LeaveMsgViewModel> LoadLeaveMsg(Models.Params.LeaveMsgParam  param);
        /// <summary>
        /// 新闻留言回复
        /// </summary>
        /// <returns></returns>
        IQueryable<Reply> GetNewsLeaveMsgData(int newsId);
        /// <summary>
        /// 福利美图留言回复
        /// </summary>
        /// <param name="girlId">前台传递过来的福利美图id</param>
        /// <returns></returns>
        IQueryable<Reply> GetGirlLeaveMsgData(int girlId);
        /// <summary>
        ///  用户攻略留言回复
        /// </summary>
        /// <param name="raidersId">前台传递过来的攻略id</param>
        /// <returns></returns>
        IQueryable<Reply> GetRaidersLeaveMsgData(int raidersId);
    }
}
