using DAL;
using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class R_UserInfo_ActionInfoBll : BaseBll<R_UserInfo_ActionInfo>,IR_UserInfo_ActionInfoBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.R_UserInfo_ActionInfoDal;
        }
    }
}
