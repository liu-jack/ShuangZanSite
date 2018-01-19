using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class DemoViewModel
    {
        public int  GameDemoId { get; set; }
        public string  GameName { get; set; }
        public string  SystemName { get; set; }
        public int AccountId  { get; set; }
        public string UName { get; set; }
        public string  Pwd { get; set; }
    }
   public class DemoFirstViewModel
   {
       public int Id { get; set; }
       public int Img { get; set; }
       public string GameName { get; set; }
       public int PaySum { get; set; }
       public string  Play { get; set; }
       public string Theme { get; set; }
       public string   Type { get; set; }
       public string  Company { get; set; }
       public string  Operator { get; set; }
   }
   public class DemoTwoViewModel
   {
       public int Id { get; set; }
       public string SystemName { get; set; }
       public string UName { get; set; }
       public string Pwd { get; set; }
       public string Url { get; set; }
       public int? UserId { get; set; }
   }
   public class DemoThreeViewModel
   {
       public int Id { get; set; }
       public string Demand { get; set; }
       public int AwardCoins { get; set; }
       public string CoinsEquity { get; set; }
       public string Img { get; set; }
       public string State { get; set; }
       public int? UserId { get; set; }
       public string Reason { get; set; }
   }

}
