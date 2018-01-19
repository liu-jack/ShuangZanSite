using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
   public class Sql_ServerModel
    {
       
        public string GameName { get; set; }
        /// <summary>
        /// 开服时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        public string ServerName { get; set; }
        public string SystemName { get; set; }
        public string GiftName { get; set; }
        public string  Url { get; set; }
        public string  Rec_Hot { get; set; }   
    }
   public class Sql_OpenServer
   {
       public int Id { get; set; }
       public string GameName { get; set; }
       public DateTime ?StartTime { get; set; }
       public string ServerName { get; set; }
       public string SystemName { get; set; }
       public int PackageId { get; set; }
       public string  GiftName { get; set; }
       public int ?PackageCardCount { get; set; }
       public string  Url { get; set; }//游戏链接
       public string  Type { get; set; }
   }
}
