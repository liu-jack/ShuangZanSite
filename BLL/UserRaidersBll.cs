using IBLL;
using Models;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Models.MapModel;
using Models.SqlMapModel;

namespace BLL
{
    public class UserRaidersBll : BaseBll<UserRaiders>, IUserRaidersBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.UserRaidersDal;
        }
        #region 批量审核
        /// <summary>
        /// 批量审核攻略
        /// </summary>
        /// <param name="ids">当前选中的所有id</param>
        /// <param name="checkIsPass">审核是否通过的类型</param>
        /// <param name="currentAdmin">当前登录的管理员姓名</param>
        /// <returns></returns>
        public int MoreCheckUserRaiders(IList<int> ids, string checkIsPass, string currentAdmin,string reason)
        {
            foreach (var id in ids)
            {
                //先查询出来每个ID对应的数据
                var ur = dbSession.UserRaidersDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (checkIsPass == "1")//1   代表审核通过
                {   //然后将对应的值更新到数据库
                    ur.State = "1";
                    ur.CheckName = currentAdmin;//当前审核人
                    dbSession.UserRaidersDal.Update(ur);
                    UserMessage um = new UserMessage();
                    um.UserId = ur.UserId;
                    um.Title = "攻略投稿—通过审核<<"+ur.Title +">>";
                    um.Msg = "亲爱的爽赞会员恭喜您，你投递的攻略《" + ur.Title + "》已审核通过啦，特此奖励10爽币。希望您在之后的日子里面，多多投稿，帮助到更多的玩家朋友哦";
                    um.State = 0;//消息未读
                    um.PayType = "1";//赚爽币记录
                    um.Pay = 10;
                    um.InTime = DateTime.Now;
                    um.IsDel = "0";
                    um.Memo = null;
                    um.Memo1 = null;
                    um.Memo2 = null;
                    um.orderNo = null;
                    dbSession.UserMessageDal.Add(um);
                  
                }
                else if (checkIsPass == "0")//0  代表审核未通过
                {
                    ur.State = "0";
                    ur.CheckName = currentAdmin;
                    dbSession.UserRaidersDal.Update(ur);
                    //2、----将审核通过的消息写入到userMessage--
                    UserMessage um = new UserMessage();
                    um.UserId = ur.UserId;
                    um.Title = "攻略投稿—未通过审核<<"+ur.Title+">>";
                    um.Msg = "亲爱的爽赞会员您好，非常抱歉，您投递的攻略《" + ur.Title + "》因“"+reason+"”的原因未通过审核，希望您针对我们提出的意见做一些修改之后再次提交。如有什么疑问，可直接联系在线客服哦";
                    um.State = 0;//消息未读
                    um.PayType = "1";//赚爽币记录
                    um.Pay = -10;
                    um.InTime = DateTime.Now;
                    um.IsDel = "0";
                    um.Memo = null;
                    um.Memo1 = null;
                    um.Memo2 = null;
                    um.orderNo = null;
                    dbSession.UserMessageDal.Add(um);
                   
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 玩家批量上删除攻略
        /// <summary>
        /// 玩家批量上删除攻略
        /// </summary>
        /// <param name="ids">当前所有选中的id</param>

        /// <returns></returns>
        public int MoreDelteRaiders(IList<int> ids)
        {
            foreach (var id in ids)
            {
                UserRaiders raiders = new UserRaiders() { Id = id };
                if (raiders != null)
                {
                    dbSession.UserRaidersDal.Delete(raiders);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 查询的数据在这个月内的阅读量排行榜最高的最新攻略
        /// <summary>
        /// 查询的数据在这个月内的阅读量排行榜最高的最新攻略
        /// </summary>
        /// <returns></returns>
        public List<RaidersViewModel> GetNewestRaiders()
        {
            int  rec=(short) Reccommend.RecIndex;
            string t=rec.ToString();
            int state = (short)CheckState.Pass;
            string pass = state.ToString();
             return dbSession.UserRaidersDal.LoadEntities(r => r.State == pass)
                //查询的数据在这个月内的阅读量排行榜最高的
                  .Where(n => DbFunctions.DiffMonths(n.InTime, DateTime.Now) <= 30).AsNoTracking()
                  .OrderByDescending(n => n.Views).Take(10).Select(r => new RaidersViewModel { Title = r.Title, EditTitle = r.EditTitle, Id = r.Id }).ToList();                                         
        }
        #endregion   
        #region 攻略详情
        public IQueryable<Models.MapModel.UserRaidersViewModel> GetRaidersDetails(int id)
        {
           
            var allRaiders = dbSession.UserRaidersDal.LoadEntities(n => n.Id == id&&n.State=="1").Select(n => new
            {
                n.Id,
                n.InTime,
                n.Views,
                n.Source,
                n.Title,
                n.Msg,
                n.Editor,
                n.EditTitle,
                n.Memo,
                n.Key
            });
            var allLeaveMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0).Select(m => new
            {
                m.UserRaidersId,
                m.MsgNum
            });
            //链接查询
            var data = from n in allRaiders
                       join m in allLeaveMsg on n.Id equals m.UserRaidersId into  mc
                       from mci in mc.DefaultIfEmpty()
                       let MsgNum = (from a in allLeaveMsg where a.UserRaidersId == n.Id select a.UserRaidersId).Count()
                       select new UserRaidersViewModel
                       {
                           Id = n.Id,
                           InTime = n.InTime,
                           ViewsNum = n.Views==null?0:n.Views,
                           Source = n.Source,
                           Title = n.Title,
                           Editor = n.Editor,
                           Msg = n.Msg,
                           MsgNum = MsgNum==0?0:MsgNum,
                           EditTitle = n.EditTitle,
                           Memo=n.Memo ,
                           KeyWords=n.Key
                       };
            return data.AsNoTracking().AsQueryable();
        } 
        #endregion
        #region 得到热点攻略
        public List<Sql_RaidersModel> GetHotRaiders()
        {
            string sql = "with cte_name(gamename,lasttime)  as( select top 3 gamename,max(rec_hot_time) from UserRaiders where rec=1  group by gamename  order by 2 desc)select gamename,BigImg,( select dbo.stringjoin(concat('{  \"id\":\"',id,'\",\"title\":\"',title,'\"  }')) from ( select top 3 id,dbo.isnull(EditTitle,title) as title from UserRaiders where gamename=t.gamename order by rec_time desc) p)as data from cte_name t inner join game g on t.gamename=g.name";
            return dbSession.ExecuteQuery<Sql_RaidersModel>(sql);
        }       
        #endregion
        #region 最赞攻略 总后台攻略推荐管理操作——首页推荐。倒序排，最后操作的显示最前
        /// <summary>
        /// 最赞攻略 总后台攻略推荐管理操作——首页推荐。倒序排，最后操作的显示最前
        /// </summary>
        /// <returns></returns>
        public List<RaidersViewModel> GetMostGreatRaiders()
        {
            int forum = (short)Reccommend.RecIndex;
            string t = forum.ToString();
            int p = (short)CheckState.Pass;
            string pass = p.ToString();
            return dbSession.UserRaidersDal.LoadEntities(r => r.State == pass && r.Rec_Hot == t).AsNoTracking().OrderByDescending(r => r.Rec_Hot_Time).Take(10)
                 .Select(r => new RaidersViewModel() { Title = r.Title, EditTitle = r.EditTitle, Id = r.Id }).ToList();
        } 
        #endregion
        #region 最赞攻略拿游戏logo图
        /// <summary>
        /// 最赞攻略拿游戏logo图
        /// </summary>
        /// <returns></returns>
        public List<UserRaidersViewModel> GetMostGreatRaiders(int take)
        {
            int forum = (short)Reccommend.RecIndex;
            string t = forum.ToString();
            int p = (short)CheckState.Pass;
            string pass = p.ToString();
            var allGame = dbSession.GameDal.LoadEntities(g => g.Id > 0).Select(g => new { g.Name, g.LoGo });
            var allRaiders = dbSession.UserRaidersDal.LoadEntities(r => r.State == pass && r.Rec_Hot == t).Select(r => new
            {
                r.Id,
                r.Rec_Hot_Time,
                r.Title,
                r.EditTitle,
                r.GameName
            });
            var data = (from r in allRaiders
                        join g in allGame on r.GameName equals g.Name
                        select (new UserRaidersViewModel() { Id = r.Id, Title = r.Title, EditTitle = r.EditTitle, Logo = g.LoGo, Rec_Time = r.Rec_Hot_Time })).AsNoTracking().OrderByDescending(r => r.Rec_Time).Take(take).ToList();
            return data;
        } 
        #endregion
     public IList<Sql_RecRaidersModel> RelatedRaiders(int top, int id)
     {
         return dbSession.UserRaidersDal.RelatedRaiders(top, id);
     }
    }
}
