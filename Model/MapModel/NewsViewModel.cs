using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
            [Serializable]
  public  class NewsViewModel
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string  Image { get; set; }
       
        public string  EditTitle  { get; set; }
        public string  Editor { get; set; }
        public string  Url { get; set; }
        public string  Type { get; set; }
        public int ? Views { get; set; }
        public string  Source { get; set; }
        public int ?ViewNum { get; set; }//浏览量
        public string  Msg { get; set; }//留言内容
        public string Memo { get; set; }//内容摘要
        public int? MsgNum { get; set; }//留言数量
        public DateTime InTime { get; set; }
        public DateTime? Rec_Forum_Time { get; set; }
        //新增新闻推荐
        public string Game { get; set; }
        public string   SystemName { get; set; }
        public string   Rec_Hot_Index { get; set; }
        public string  Rec_Forum_Index { get; set; }
        public string  AddedBy { get; set; }
        public string  Kewords { get; set; }
    }
    //前台公共输出数据的model
    public class FrontNews
       {
           public int Id { get; set; }
           public string Title { get; set; }
          // public string Image { get; set; }
           public DateTime? InTime { get; set; }
           public string EditTitle { get; set; }
           public string  Type { get; set; }
           public int? Views { get; set; }
        }
}
