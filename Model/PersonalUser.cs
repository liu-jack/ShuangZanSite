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
    public partial class PersonalUser
    {
        public int Id { get; set; }
        public string UName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public System.DateTime InTime { get; set; }
        public string QQ { get; set; }
        public string WeChat { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Head { get; set; }
        public string Contacts { get; set; }
        public string ContactNum { get; set; }
        public int State { get; set; }
        public int ReferrerId { get; set; }
        public int RecommendNum { get; set; }
    }
}
