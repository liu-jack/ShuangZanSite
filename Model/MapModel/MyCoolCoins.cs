using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    /// <summary>
    /// 获取我的爽币记录
    /// </summary>
   public class MyCoolCoins
    {
        //c.Id,c.InTime,c.Pay,c.Msg,c.Title,c.Memo2,c.Memo1,c.Memo,Pays=pays
        public int Id { get; set; }
        public int ? Pay { get; set; }
        public string Msg  { get; set; }
        public string  Title { get; set; }
        public string  Memo { get; set; }
        public string  Memo1 { get; set; }
        public string  Memo2 { get; set; }
        public int? Pays { get; set; }
        public DateTime? InTime { get; set; }
        public string  OrderNo { get; set; }//订单号
    }
}
