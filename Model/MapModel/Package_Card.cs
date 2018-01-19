using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
  public  class Package_Card
    {
        public int Id { get; set; }
      /// <summary>
      /// 厂商id
      /// </summary>
        public int ?CompanyId { get; set; }
        public string  GameName { get; set; }
        public  string  ServerName  { get; set; }
        public  string  GiftType  { get; set; }
        public string  GiftName { get; set; }
        public DateTime ?StartDate { get; set; }
        public string  State { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ?InTime { get; set; }//领取时间
        public string  Url { get; set; }
        public string  Card { get; set; }
        public string   Memo { get; set; }
        public string  Msg { get; set; }
        public string  SystemName { get; set; }
    }
}
