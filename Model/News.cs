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
    public partial class News
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string EditTitle { get; set; }
        public string Game { get; set; }
        public string Kewords { get; set; }
        public string Type { get; set; }
        public string CheckName { get; set; }
        public string Memo { get; set; }
        public string Msg { get; set; }
        public Nullable<int> Views { get; set; }
        public Nullable<int> LeaveMsgId { get; set; }
        public string State { get; set; }
        public string Source { get; set; }
        public System.DateTime InTime { get; set; }
        public string Rec_Forum_Index { get; set; }
        public System.DateTime Rec_Forum_Time { get; set; }
        public string Rec_Hot_Index { get; set; }
        public System.DateTime Rec_Hot_Time { get; set; }
        public string Editor { get; set; }
        public string AddedBy { get; set; }
        public Nullable<int> Tip { get; set; }
        public Nullable<int> Stamp { get; set; }
        public string GameTerms { get; set; }
    }
}
