using IBLL;
using IDAL;
using Models;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
   public class ActionInfoBll:BaseBll<ActionInfo>,IActionInfoBll
    {
       public override void SetCurrentDal()
       {
           CurrentDal = dbSession.ActionInfoDal;
       }
       #region 删除

       public int DeleteActions(IList<int> ids)
       {
           foreach (var id in ids)
           {
               var role = dbSession.ActionInfoDal.LoadEntities(u => u.Id == id).FirstOrDefault();
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
