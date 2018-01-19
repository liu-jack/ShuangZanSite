using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
  public  class GameDemoViewModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string GameName { get; set; }
        public string  DemoType { get; set; }//试玩类型
        public int? PaySum { get; set; }
      
        public string Play { get; set; }
        public string Theme { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Operator { get; set; }
        /// <summary>
        /// 试玩链接
        /// </summary>
        public string Url { get; set; }
        public DateTime? InTime { get; set; }
    }
}
