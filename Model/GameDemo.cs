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
    public partial class GameDemo
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string Img { get; set; }
        public int state { get; set; }
        public string Rec { get; set; }
        public System.DateTime Rec_Time { get; set; }
        public string Rec_Hot { get; set; }
        public System.DateTime rec_HotTime { get; set; }
        public System.DateTime InTime { get; set; }
        public string CheckName { get; set; }
        public string Type { get; set; }
    }
}
