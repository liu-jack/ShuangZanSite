using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class DemoAccountModel
    {
        public int Id { get; set; }
        public int GameDemoId { get; set; }
        public string SystemName { get; set; }
        public string Url { get; set; }
        public string UName { get; set; }
        public string Pwd { get; set; }
        public string City { get; set; }
     
        public DateTime? GetAccountIntime  { get; set; }
        public string State { get; set; }
        public DateTime? InTime { get; set; }
    }
}
