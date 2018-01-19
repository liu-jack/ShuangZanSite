using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
  public  class Sql_NewsModel
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string  EditTitle { get; set; }
        public string  Img { get; set; }
        public DateTime? InTime { get; set; }
        public int ?MsgNum { get; set; }
        public int ?Views { get; set; }
    }
}
