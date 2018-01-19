using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
    /// <summary>
    /// 注册用户
    /// </summary>
   public class Sql_UserModel
    {
        public string  Mobile { get; set; }
        public string  Code { get; set; }
        public string  Ip { get; set; }
       /// <summary>
       /// 返回的验证码
       /// </summary>
        public string  OutCode { get; set; }
    }
   public class Sql_UserGetPwd
   {
       public string  Mobile { get; set; }
       public string  Code { get; set; }
       public string  Pass { get; set; }
       public string  Err { get; set; }
   }
}
