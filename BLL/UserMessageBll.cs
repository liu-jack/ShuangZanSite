using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class UserMessageBll : BaseBll<UserMessage>, IUserMessageBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.UserMessageDal;
        }
        #region 得到我的爽币记录
       /// <summary>
        /// 得到我的爽币记录
       /// </summary>
       /// <param name="user">当前用户</param>
       /// <param name="pageIndex">当前页</param>
       /// <param name="pageSize">一夜多少条</param>
       /// <param name="total">总数量</param>
        /// <param name="payType">1-赚爽币记录；2-充值记录；3-消费记录</param>
       /// <returns></returns>
        public IQueryable<MyCoolCoins> GetCoolCoins(PersonalUser user, int pageIndex, int pageSize, out int total, string payType)
        {
            //id,intime,pay,title,msg,memo,memo1,memo2,
            var coinsList = dbSession.UserMessageDal.LoadEntities(um =>true);
            var data = from c in coinsList
                       let pays = (from d in coinsList where d.UserId == user.Id && d.PayType == payType select d.Pay).Sum()
                       where c.UserId == user.Id && c.PayType == payType && c.Pay != 0
                       select new MyCoolCoins { 
                           Id = c.Id,
                           InTime = c.InTime,
                           Pay = c.Pay, 
                           Msg = c.Msg, 
                           Title = c.Title,
                           Memo2 = c.Memo2, 
                           Memo1 = c.Memo1,
                           Memo = c.Memo,
                           OrderNo = c.orderNo,
                           Pays = pays 
                       };                                              
            total = data.Count();
            return data.OrderByDescending(c => c.InTime).Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize).AsQueryable();
        } 
        #endregion         
        /// <summary>
        /// 爽币余额
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetCoinsUsedCount(PersonalUser user)
        {
            var earnCoins = dbSession.UserMessageDal.LoadEntities(um => um.UserId == user.Id && um.PayType == "1").Sum(um => um.Pay == null ? 0 : um.Pay);
              var payCoins = dbSession.UserMessageDal.LoadEntities(um => um.UserId == user.Id && um.PayType == "2").Sum(um => um.Pay == null ? 0 : um.Pay);
              var expenCoins = dbSession.UserMessageDal.LoadEntities(um => um.UserId == user.Id && um.PayType == "3").Sum(um => um.Pay == null ? 0 : um.Pay);
            ////爽币余额=赚的总额+充值的总额-消费的总额           
              var CoolCoinsUsedCount = (int)(earnCoins + payCoins + (expenCoins));
              return CoolCoinsUsedCount;
        }
         /// <summary>
         /// 第二种方式操作爽币余额
         /// </summary>
         /// <param name="user"></param>
         /// <returns></returns>
 
        public int CoolCoinsUsed(PersonalUser user)
        {
            return  dbSession.UserMessageDal.LoadEntities(um => um.UserId == user.Id).Sum(um=>um.Pay);
        }
        public int MoreMsgRead(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var model = dbSession.UserMessageDal.LoadEntities(r => r.Id == id).FirstOrDefault();
                if (model != null)
                {
                    model.State = 1;//已读
                    dbSession.UserMessageDal.Update(model);
                }
            }
            return dbSession.SaveChanges();
        }


        public int MoreDelMsg(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var model = dbSession.UserMessageDal.LoadEntities(r => r.Id == id).FirstOrDefault();
                if (model != null)
                {
                    model.IsDel = "1";//删除
                    dbSession.UserMessageDal.Update(model);
                }
            }
            return dbSession.SaveChanges();
        }



        public bool UserRechargeCoolCoins(string userId, string trade_no, string rmb, string sb, string feetype)
        {
            return dbSession.UserMessageDal.UserRechargeCoolCoins(userId, trade_no, rmb, sb, feetype);
        }
    }
}
