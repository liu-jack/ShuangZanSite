//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    public partial class BeautifulGirls
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Imgs { get; set; }
        public System.DateTime InTime { get; set; }
        public int Rec { get; set; }
        public System.DateTime Rec_Time { get; set; }
        public string LeadTxt { get; set; }
        public Nullable<int> Views { get; set; }
        public string AddedBy { get; set; }
        public Nullable<int> Tip { get; set; }
        public Nullable<int> Stamp { get; set; }
    }
}
