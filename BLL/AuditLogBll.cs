using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class AuditLogBll : BaseBll<AuditLog>, IAuditLogBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.AuditLogDal;
        }

       
    }
}
