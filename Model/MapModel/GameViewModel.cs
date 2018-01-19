using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Enum
{
             [Serializable]
  public  class GameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string  EdItTitle { get; set; }
        public DateTime? InTime { get; set; }
        public string  GameName { get; set; }
        public string  SmallImg { get; set; }
        public string  DescImg { get; set; }
        public string  Msg { get; set; }
                 //主运
        public string Url { get; set; }
        public string  Company { get; set; }
        public string  Type { get; set; }
        public string  Play { get; set; }
        public string  Theme { get; set; }
        
                 //图片分割
    }
             public class GameViewModel2
             {
                 public string  BigImg { get; set; }
                 public int Id { get; set; }
             }
             public class GameName
             {
                 public string  Name { get; set; }
             }

    //前台公共数据输出model
             public class FrontGame
             {
                 public int Id { get; set; }
                 public string  Name { get; set; }
                 public string Type  { get; set; }
                 public int? LikeNum { get; set; }
                 public string   SmallImg { get; set; }
             }
}
