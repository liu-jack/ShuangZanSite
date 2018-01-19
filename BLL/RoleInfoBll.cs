using IBLL;
using Models;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RoleInfoBll : BaseBll<RoleInfo>, IRoleInfoBll
    {
       
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.RoleInfoDal;
        }
        #region 删除
      
        public int DeleteRoles(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var role = dbSession.RoleInfoDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (role != null)
                {
                    role.DelFlag = (short)DelFlagEnum.Deleted;
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
    }
}
