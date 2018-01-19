using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
  public  class GirlsViewModel
    {
        public int Id { get; set; }
        public string  Title { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public  int ? Views { get; set; }
        public int LeaveMsgCount  { get; set; }
       /// <summary>
       /// 封面图片
       /// </summary>
        public string  Image { get; set; }
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 图片总数
        /// </summary>
        public int?  ImageCount { get; set; }
        /// <summary>
        /// 发布者
        /// </summary>
        public string AddedBy { get; set; }
        public string  Tags { get; set; }
        public DateTime? Rec_Time { get; set; }
        public int? Rec  { get; set; }
        public string  LeadTxt { get; set; }
    }
}
