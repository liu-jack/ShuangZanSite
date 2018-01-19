using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    [Serializable]
    public class LeaveMsgViewModel
    {
        public int Id { get; set; }
     
        public string  NewsTitle { get; set; }
        public string RaidersTitle { get; set; }
        public string GirlTitle { get; set; }
        public string  EditTitle { get; set; }
        public string  UserName { get; set; }
        public string  IP { get; set; }
        public string  Content { get; set; }
        public DateTime? InTime { get; set; }
        public string  Msg { get; set; }
        //踩
        public  int ? LikeNum { get; set; }
        //顶
        public int? Tip { get; set; }
        public string  ReplyMsg { get; set; }
        public class Reply
        {
            public string  ReplyName { get; set; }
            public string  ReplyUserImg { get; set; }
            public string  ReplyContent { get; set; }
            public DateTime? ReplyInTime { get; set; }
            public int? ReplayLikeNum { get; set; }
            public string ReplyIp { get; set; }
            public string ReplyCity { get; set; }
            /// <summary>
            /// 顶
            /// </summary>
            public int?  ReplyTip { get; set; }
            /// <summary>
            /// 踩
            /// </summary>
            public int? ReplyStamp { get; set; }
                 
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string  UserNameImg { get; set; }
        /// <summary>
        /// 所属板块的类型
        /// </summary>
        public string  Type { get; set; }
        public string  City { get; set; }
    }
}
