using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RechargedUsedBll : BaseBll<RechargedUsed>, IRechargedUsedBll
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.RechargedUsedDal;
        }
    }
}
