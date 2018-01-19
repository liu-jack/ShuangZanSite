using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class RechargeViewModel
    {
        //t.Id, t.ComboName, t.InTime, t.Money, t.Remark, t.Num, c.SystemName
        public int? Id { get; set; }
        public string  ComboName { get; set; }
        public DateTime? InTime { get; set; }
        public int? Money { get; set; }
        public string  Remark { get; set; }
        public int? Num { get; set; }
        public int? Used{ get; set; }
        public string  SystemName  { get; set; }
       /// <summary>
       /// 剩下的条数
       /// </summary>
        public int? LeftNum { get; set; }
    }
}
