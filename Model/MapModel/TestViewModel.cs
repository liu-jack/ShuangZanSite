using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
                [Serializable]
   public class TestViewModel
    {
       /// <summary>
       /// 进入专区的详情链接
       /// </summary>
        public int  Id { get; set; }
        public string  GameName { get; set; }
       /// <summary>
       /// 开测状态
       /// </summary>
        public string  State { get; set; }
       /// <summary>
       /// 开测的时间
       /// </summary>
        public DateTime? StartTime { get; set; }
       /// <summary>
       /// 开测的游戏类型
       /// </summary>
        public string  Type { get; set; }
       /// <summary>
       /// 开测游戏的题材
       /// </summary>
        public string  Theme { get; set; }
       /// <summary>
       /// 开测游戏的玩法
       /// </summary>
        public string  Play { get; set; }
       /// <summary>
       /// 小图标
       /// </summary>
        public string  SmallImg { get; set; }
       /// <summary>
       /// 研发
       /// </summary>
        public string  Company { get; set; }
      /// <summary>
      /// 游戏链接
      /// </summary>
        public string  Url { get; set; }
       /// <summary>
       /// 礼包链接
       /// </summary>
        public string  PackageUrl { get; set; }
   }
}
