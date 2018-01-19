using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class GiftBll : BaseBll<Gift>, IGiftBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.GiftDal;
        }
        #region 礼品管理
        /// <summary>
        /// 礼品管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<Gift> LoadGift(Models.Params.GiftParam param)
        {
            var temp = dbSession.GiftDal.LoadEntities(g => g.Id > 0);

            if (!string.IsNullOrEmpty(param.GiftName))
            {
                temp.Where(g => g.Name.Contains(param.GiftName));
            }
            if (!string.IsNullOrEmpty(param.State))
            {
                temp.Where(g => g.State.Contains(param.State));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(d => d.Intime)
                    .Skip(param.PageSize * (param.PageIndex - 1))
                    .Take(param.PageSize).AsQueryable();
        } 
        #endregion
        #region 查询最新发布的6条产品
        /// <summary>
        /// 查询最新发布的6条产品
        /// </summary>
        /// <returns></returns>
        public List<Gift> NewestPublishGift()
        {
            string sql = "select top  6  * from Gift  where Gift.state=1 order by Gift.intime  desc";
            return dbSession.ExecuteQuery<Gift>(sql);
        } 
        #endregion
        #region 礼品兑换
        public GiftViewModel ExChangeGift(int userId, int giftId, int addressId, int num, string color)
        {
            SqlParameter[] pms = {                                    
                                 new SqlParameter("@userid",SqlDbType.Int){Value=userId},
                                 new SqlParameter("@giftid",SqlDbType.Int){Value=giftId},
                                   new SqlParameter("@addressid",SqlDbType.Int){Value=addressId},
                                 new SqlParameter("@num",SqlDbType.Int){Value=num},
                                 new SqlParameter("@color",SqlDbType.NVarChar){Value=color},
                                 new SqlParameter("@orderid",SqlDbType.Int),
                                  new SqlParameter("@err",SqlDbType.NVarChar,1000),
                                  };
            pms[5].Direction = ParameterDirection.Output;
            pms[6].Direction = ParameterDirection.Output;
            var data = dbSession.ExecuteQuery<GiftViewModel>("exec exchangeGift @userid,@giftid,@addressid,@num,@color, @orderid out,@err out", pms).FirstOrDefault();
            GiftViewModel model = new GiftViewModel()
            {
                OrderId = (int)pms[5].Value,
                Err = pms[6].Value.ToString()
            };
            return model;
        } 
        #endregion
    }
}
