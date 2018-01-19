using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
namespace BLL
{
    public class LeaveMsgBll : BaseBll<LeaveMsg>,ILeaveMsgBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.LeaveMsgDal;
        }
        #region 留言管理
        /// <summary>
        /// 留言管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<Models.MapModel.LeaveMsgViewModel> LoadLeaveMsg(Models.Params.LeaveMsgParam param)
        {
            var allUser = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0).Select(u => new { u.Id, u.UName });
            var allNews = dbSession.NewsDal.LoadEntities(n => n.Id > 0).Select(n => new { n.Id, n.Title, n.EditTitle, n.Type });
            var allRaiders = dbSession.UserRaidersDal.LoadEntities(r => r.Id > 0).Select(r => new { r.Id, r.Title, r.EditTitle });
            var allGirls = dbSession.BeautifulGirlsDal.LoadEntities(g => g.Id > 0).Select(g => new { g.Id, g.Title });
            var allMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0);
            var data = from m in allMsg
                       join u in allUser on m.PersonalUserId equals u.Id into uc
                       from uci in uc.DefaultIfEmpty()
                       join n in allNews on m.NewsId equals n.Id into nc
                       from nci in nc.DefaultIfEmpty()
                       join r in allRaiders on m.UserRaidersId equals r.Id into rc
                       from rci in rc.DefaultIfEmpty()
                       join g in allGirls on m.GirlId equals g.Id into gc
                       from gci in gc.DefaultIfEmpty()
                       select (new LeaveMsgViewModel()
                       {
                           Id = m.Id,
                           UserName = uci.UName,
                           NewsTitle = nci.Title,
                           // EditTitle=nci.EditTitle,
                           RaidersTitle = rci.Title,
                           GirlTitle = gci.Title,
                           Type = nci.Type,
                           City = m.City,
                           IP = m.IP,
                           Content = m.Msg,
                           InTime = m.InTime
                       });
            if (!string.IsNullOrEmpty(param.UserName))
            {
                data = data.Where(d => d.UserName.Contains(param.UserName));
            }
            if (!string.IsNullOrEmpty(param.Content))
            {
                data = data.Where(d => d.Content.Contains(param.Content));
            }
            param.Total = data.Count();
            return data.OrderByDescending(m => m.InTime)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize).AsQueryable();
        } 
        #endregion
        #region  前台新闻留言输出信息
        /// <summary>
        /// 前台新闻留言输出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<Reply> GetNewsLeaveMsgData(int newsId)
        {
            var msg = dbSession.LeaveMsgDal.LoadEntities(m => true);
            var news = dbSession.NewsDal.LoadEntities(n => n.Id > 0);
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0);
            var data = from m in msg
                       join u in user on m.PersonalUserId equals u.Id into um
                       from umi in um.DefaultIfEmpty()
                       join n in news on m.NewsId equals n.Id                   
                       join mm in msg on m.Id equals mm.ReplyId
                       where m.NewsId == newsId
                       select new Reply()
                       {
                           SelfId = mm.Id,
                           ReplyId = mm != null ? mm.ReplyId : 0,
                           ReplyContent =  mm.Msg,
                           ReplyCity = mm.City ,
                           ReplyName =umi==null ?(mm.Id + "爽赞网友"): umi.UName ,
                           ReplyUserImg = umi !=null ? umi.Head : null,//用户头像
                           ReplyInTime = mm.InTime,
                           ReplyTip = mm.Tip != null ? mm.Tip : 0,
                           ReplyStamp = mm.Stamp != null ? mm.Stamp :0
                       };
            return data.AsNoTracking().OrderByDescending(d => d.ReplyInTime).AsQueryable();
        } 
        #endregion
        #region 福利美图留言回复
        /// <summary>
        /// 福利美图留言回复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<Reply> GetGirlLeaveMsgData(int girlId)
        {
            var msg = dbSession.LeaveMsgDal.LoadEntities(m => true);
            var girl = dbSession.BeautifulGirlsDal.LoadEntities(n => n.Id > 0).Select(n => new { n.Id });
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id, u.Head });
            var data = from m in msg
                       join u in user on m.PersonalUserId equals u.Id into um
                       from umi in um.DefaultIfEmpty()
                       join n in girl on m.GirlId equals n.Id
                       where m.GirlId == girlId
                       join mm in msg on m.Id equals mm.ReplyId
                       select new Reply()
                       {
                           SelfId = mm.Id,
                           ReplyId = mm != null ? mm.ReplyId : 0,
                           ReplyContent = mm != null ? mm.Msg : null,
                           ReplyCity = mm != null ? mm.City : null,
                           ReplyName = umi != null ? umi.UName : "" + mm.Id + "" + "爽赞网友",
                           ReplyUserImg = umi != null ? umi.Head : null,//用户头像
                           ReplyInTime = mm.InTime ,
                           ReplyTip = mm.Tip != null ? mm.Tip : 0,
                           ReplyStamp = mm.Stamp != null ? mm.Stamp : 0
                       };
          
            return data.OrderByDescending(d => d.ReplyInTime).AsQueryable();
        } 
        #endregion
        #region  用户攻略留言回复
        /// <summary>
        /// 用户攻略留言回复
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>int raidersId
        public IQueryable<Reply> GetRaidersLeaveMsgData(int raidersId)
        {
            var msg = dbSession.LeaveMsgDal.LoadEntities(m => true);
            var raider = dbSession.UserRaidersDal.LoadEntities(n => n.Id > 0).Select(n => new { n.Id });
            var user = dbSession.PersonalUserDal.LoadEntities(u => u.Id > 0).Select(u => new { u.UName, u.Id, u.Head });
            var data = from m in msg
                       join u in user on m.PersonalUserId equals u.Id into um
                       from umi in um.DefaultIfEmpty()
                       join n in raider on m.UserRaidersId equals n.Id
                       where m.UserRaidersId ==raidersId
                       join mm in msg on m.Id equals mm.ReplyId
                       select new Reply()
                       {
                           SelfId = mm.Id,
                           ReplyId = mm != null ? mm.ReplyId : 0,
                           ReplyContent = mm != null ? mm.Msg : null,
                           ReplyCity = mm != null ? mm.City : null,
                           ReplyName = umi != null ? umi.UName : "" + mm.Id + "" + "爽赞网友",
                           ReplyUserImg = umi != null ? umi.Head : null,//用户头像
                           ReplyInTime = mm.InTime ,
                           ReplyTip = mm.Tip != null ? mm.Tip : 0,
                           ReplyStamp = mm.Stamp != null ? mm.Stamp : 0
                       };
            return data.AsNoTracking().OrderByDescending(d => d.ReplyInTime).AsQueryable();
        }
        #endregion
    }
}
