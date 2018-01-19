using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    [Serializable]
    public class UserRaidersViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EditTitle { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int? ViewsNum { get; set; }
        /// <summary>
        /// 留言数
        /// </summary>
        public int? MsgNum { get; set; }
        public string Source { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 编辑者
        /// </summary>
        public string Editor { get; set; }
        /// <summary>
        /// 攻略内容
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 攻略文章中的第一张图片提取
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 攻略显示的游戏名
        /// </summary>
        public string GameName { get; set; }
        /// <summary>
        /// 显示的大背图
        /// </summary>
        public string BigImg { get; set; }
        public DateTime? Rec_Time { get; set; }
        /// <summary>
        /// 游戏中的logo图
        /// </summary>
        public string Logo { get; set; }
        public string Memo { get; set; }
        public string  KeyWords { get; set; }
    }
}
