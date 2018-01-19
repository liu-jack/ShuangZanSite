using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    [Serializable]
 public  class ServiceViewModel
    {
        public int Id { get; set; }
        public string  GameName { get; set; }
     /// <summary>
     /// 开服时间
     /// </summary>
        public DateTime? StartTime { get; set; }
        public string  ServerName { get; set; }
        public string  SystemName { get; set; }
        public string  GiftName { get; set; }
     /// <summary>
     /// 开服类型
     /// </summary>
        public string  Type { get; set; }
        public string  Url { get; set; }
        public int  ?LikeNum { get; set; }
        public int? ServiceCount { get; set; }
        public string  SmallImg { get; set; }

        public int CompanyId { get; set; }
        public DateTime? InTime { get; set; }
        public string State { get; set; }    
        public string GiftType { get; set; }
        public string CheckName { get; set; }
        public int? PackageCardCount { get; set; }
        
      
    }
    //--------------------------------------------------------
    /// <summary>
    /// 我的开服历史游戏名称
    /// </summary>
    public class MyServerGame
    {
        public string GameName { get; set; }
    }
}
