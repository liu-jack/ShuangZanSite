using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.SqlMapModel
{
   public class Sql_GameModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string GameName { get; set; }
        public int? PaySum { get; set; }
        public string  Url { get; set; }
    }
}
