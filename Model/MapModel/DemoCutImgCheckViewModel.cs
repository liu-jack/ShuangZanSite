using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
  public  class DemoCutImgCheckViewModel
    {
        public int Id { get; set; }
      
     
        public string  GameName { get; set; }
      /// <summary>
      /// 试玩类型
      /// </summary>
        public string  Type { get; set; }
      /// <summary>
      /// 审核状态
      /// </summary>
        public string  State { get; set; }
      /// <summary>
      /// 运营商
      /// </summary>
        public string  SystemName { get; set; }
      /// <summary>
      /// 试玩账号
      /// </summary>
        public string  Account { get; set; }
      /// <summary>
      /// 领号玩家
      /// </summary>
        public string  UName { get; set; }
     /// <summary>
     /// 试玩要求
     /// </summary>
        public string  Demand { get; set; }
      /// <summary>
      /// 奖励爽币
      /// </summary>
        public int?  AwardCoins{ get; set; }
      /// <summary>
      /// 爽币权益
      /// </summary>
        public string CoinsEquity { get; set; }
      /// <summary>
      /// 运营商url
      /// </summary>
        public string  Url { get; set; }
      /// <summary>
      /// 上传时间
      /// </summary>
        public DateTime? InTime { get; set; }

        public int DemoId { get; set; }
        public int AccountId { get; set; }
        public int RequireId { get; set; }
        public int UserId { get; set; }
      /// <summary>
      /// 图片
      /// </summary>
        public string  Img { get; set; }
      //-----补充充值审核的字段
        public string  ServerName { get; set; }
        public int? Pay { get; set; }
        public string  Pwd { get; set; }
        public int? RechargeState { get; set; }
        public string  CurrentPlayer { get; set; }
    }
}
