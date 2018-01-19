using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class PackageViewModel
    {
        public int Id { get; set; }
        public string  GameName { get; set; }
        public string  SystemName { get; set; }
        public string  GiftName { get; set; }
        public string  SmallImg { get; set; }
        public string  Logo { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? RecTime { get; set; }
        public string  DescImg { get; set; }
        public string  Type { get; set; }
        public int ? Used { get; set; }//剩余量
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string  Msg { get; set; }
        public string  Memo { get; set; }    
    }
   public class PackageViewModel2
   {
       public int Id { get; set; }
       public string GameName { get; set; }
       public string SystemName { get; set; }
       public string Msg { get; set; }
       public string Memo { get; set; }
       public string Type { get; set; }
       public string GiftName { get; set; }     
       public string DescImg { get; set; }    
       public int? Used { get; set; }//剩余量
       public DateTime? StartTime { get; set; }
       public DateTime? EndTime { get; set; }     
       /// <summary>
       /// 游戏链接
       /// </summary>
       public string Url { get; set; }
       public int? Cardcou1 { get; set; }
   }
    /// <summary>
    /// 领号实体
    /// </summary>
   public class PackageCodeViewModel
   {
       public string  Code { get; set; }
       public string Msg { get; set; }
       public string  Memo { get; set; }
       public string  GameName { get; set; }
       public string  Systemname { get; set; }
   }
}
