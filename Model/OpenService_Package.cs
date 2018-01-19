using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
  public  class OpenService_Package
    {
        public int Id { get; set; }
        public int? companyId { get; set; }
        public string GameName { get; set; }
        public DateTime? StartDate { get; set; }
        public string  ServerName { get; set; }
        public string  Url { get; set; }
        public string  GiftName { get; set; }
        public string  Rec_Hot { get; set; }
    }
}
