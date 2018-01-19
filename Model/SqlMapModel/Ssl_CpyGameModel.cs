using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
    /// <summary>
    /// 厂商详情联运等游戏分页
    /// </summary>
  public  class Sql_CpyGameModel
    {
        public int Id { get; set; }         
        public string GameName { get; set; }
        public string SmallImg { get; set; }
        public string Msg { get; set; }
        //主运
        public string Url { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string Play { get; set; }
        public string Theme { get; set; }
        public string  DescImg { get; set; }
        
    }
}
