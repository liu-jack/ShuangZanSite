using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class OrderBll : BaseBll<Order>, IOrderBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.OrderDal;
        }
        /// <summary>
        /// 玩家个人批量删除订单
        /// </summary>
        /// <param name="ids">当前选中订单的所有id</param>
        /// <returns></returns>
        public int MoreDelteOrder(IList<int> ids)
        {
            foreach (var id in ids)
            {
              var order=dbSession.OrderDal.LoadEntities(r => r.Id == id).FirstOrDefault();
              if (order!=null)
              {
                  order.IsDel = (short)Models.Enum.DelFlagEnum.Deleted;
              }
            }
            return dbSession.SaveChanges();
        }
    }
}
