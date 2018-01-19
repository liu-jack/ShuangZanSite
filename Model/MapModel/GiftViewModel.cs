using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    /// <summary>
    /// 订单兑换-存储过程
    /// </summary>
  public  class GiftViewModel
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int? OrderId { get; set; }
      /// <summary>
      /// 提示错误消息
      /// </summary>
        public string  Err { get; set; }
    }
}
