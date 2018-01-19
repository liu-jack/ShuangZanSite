using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
    /// <summary>
    /// 公共表达式sql所需要的model
    /// </summary>
   public class Sql_RaidersModel
    {
        public string  GameName { get; set; }
        public string  BigImg { get; set; }
        public string  Data { get; set; }
    }
   public class Sql_RecRaidersModel
   {
       public int Id { get; set; }
       public string  Title { get; set; }
       public string   EditTitle { get; set; }
       public DateTime? InTime { get; set; }
   }
}
