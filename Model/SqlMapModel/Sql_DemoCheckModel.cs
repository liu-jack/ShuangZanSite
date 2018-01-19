using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
   public class Sql_DemoCheckModel
    {
        public int DemoId { get; set; }
        public int AccountId { get; set; }
        public int RequireId { get; set; }
        public int UserId { get; set; }
        public string  Type { get; set; }
        public string  Reason { get; set; }
        public string  Err { get; set; }
    }
}
