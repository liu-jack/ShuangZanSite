using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
  public  class Sql_UserSignInModel
    {
        public string  UName { get; set; }
        public string  Pass { get; set; }
        public string Mobile { get; set; }
        public string  Code { get; set; }
          /// <summary>
          /// 推荐id
          /// </summary>
        public int ?TjId { get; set; }
        public string  Err { get; set; }
    }
}
