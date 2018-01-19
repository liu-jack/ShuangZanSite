using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
    /// <summary>
    /// 淘福利-领取帐号
    /// </summary>
  public  class Sql_DemoModel
    {
       
        public string  UName { get; set; }
        public string  Pass { get; set; }
      /// <summary>
        /// 返回出运营商
      /// </summary>
        public string  Systemname { get; set; }
        public string  Url { get; set; }
      /// <summary>
        /// 返回内容
      /// </summary>
        public string  Err { get; set; }

    }
}
