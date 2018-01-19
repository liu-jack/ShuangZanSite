using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    /// <summary>
    /// 我的试玩
    /// </summary>
  public  class MyGameDemo
    {
        public int GameDemoId { get; set; }
        public int AccountId { get; set; }
        public string  GameName { get; set; }
        public string SystemName { get; set; }
        public int? Coins { get; set; }//奖励的爽币
        public string  Url { get; set; }
        public string  Account { get; set; }
        public string  Pwd { get; set; }
        public string  Type { get; set; }
        public DateTime? InTime { get; set; }
        public int PassCheck { get; set; }
        public int Progress { get; set; }
    }
}
