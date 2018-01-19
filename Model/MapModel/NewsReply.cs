using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
   public class NewsReply
    {
       //自己的id
        public int? SelfId { get; set; }
       //回复的id对象
        public int? ReplyId { get; set; }
        public string ReplyName { get; set; }
        public string ReplyUserImg { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? ReplyInTime { get; set; }
     
        public string ReplyIp { get; set; }
        public string ReplyCity { get; set; }
        /// <summary>
        /// 顶
        /// </summary>
        public int? ReplyTip { get; set; }
        /// <summary>
        /// 踩
        /// </summary>
        public int? ReplyStamp { get; set; }
    }
}
