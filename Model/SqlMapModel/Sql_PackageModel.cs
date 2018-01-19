using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
    /// <summary>
    /// 热游礼包
    /// </summary>
  public  class Sql_PackageModel
    {
        public string  Name { get; set; }
        public string  SmallImg { get; set; }
        public string  Data { get; set; }
    }
    /// <summary>
    /// 同游戏名相关的礼包
    /// </summary>
  public class Sql_PackageModel2
  {
      public int Id { get; set; }
      public string  GameName { get; set; }
      public string  CompanyName { get; set; }
      public string  GiftName { get; set; }
      public DateTime? StartTime { get; set; }
      public DateTime? EndTime { get; set; }
      public int? Used { get; set; }
  }
    /// <summary>
    /// 厂商详情相关的礼包
    /// </summary>
    public class Sql_CpyPackageModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
      
        public string GiftName { get; set; }
       // public string  Type { get; set; }
        public string  DescImg { get; set; }
    }
}
