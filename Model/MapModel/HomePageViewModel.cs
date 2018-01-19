using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class HomePageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public DateTime? Intime { get; set; }
        public int? RedLight { get; set; }//幻灯区
        public int? GreyHeadlines { get; set; }//灰字头条
        public int? redHeadlines { get; set; }//红字头条
        public int? FiveStick { get; set; }//五条置顶区
        public int? DirectSeeding { get; set; }//直播
        public int? MobileGame { get; set; }//手游
        public int? RecGame { get; set; }//推荐游戏
        public int? JoinCompany { get; set; }//合作厂商
        public int? NewestHotGame { get; set; } //最热游戏
    }
}
