using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Enum
{
    public enum  PaymentStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        Pending = 10,
        /// <summary>
        /// 授权
        /// </summary>
        Authorized = 20,
        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 30,
        /// <summary>
        /// 部分已退款
        /// </summary>
        PartiallyRefunded = 35,
        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 40,
        /// <summary>
        /// 放弃支付
        /// </summary>
        Voided = 50,
    }
}
