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
    public partial class UserInfo
    {
        public UserInfo()
        {
            this.Login_Num = 1;
            this.State = "1";
            this.DelFlag = 0;
            this.RoleInfo = new HashSet<RoleInfo>();
            this.R_UserInfo_ActionInfo = new HashSet<R_UserInfo_ActionInfo>();
        }
    
        public int Id { get; set; }
        public string UName { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public System.DateTime Last_login_Time { get; set; }
        public int Login_Num { get; set; }
        public string State { get; set; }
        public string Pwd { get; set; }
        public string Tel { get; set; }
        public short DelFlag { get; set; }
    
        public virtual ICollection<RoleInfo> RoleInfo { get; set; }
        public virtual ICollection<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
    }
}