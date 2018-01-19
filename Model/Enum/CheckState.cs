using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Enum
{
    /// <summary>
    /// 审查通过的状态
    /// </summary>
   public enum CheckState
    {
        /// <summary>
        /// 审核通过
        /// </summary>
        Pass=1,
        /// <summary>
        /// 未通过
        /// </summary>
        NoPass=0,
        /// <summary>
        /// 未审核
        /// </summary>
        NoCheck=2,

    }
}
