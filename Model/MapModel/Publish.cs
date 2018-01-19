using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.MapModel
{
    /// <summary>
    /// 留言发布类
    /// </summary>
  public  class Publish
    {
        public int? Id { get; set; }
        public string Msg { get; set; }
        public string  City { get; set; }
        public string UserName { get; set; }
        public string   UserNameImg { get; set; }
        public DateTime?  InTime  { get; set; }
      /// <summary>
      /// 处理货的时间
      /// </summary>
        public string  StrTime { get; set; }
        public int? Tip { get; set; }
        public int?Stamp  { get; set; }
        
    }
}
