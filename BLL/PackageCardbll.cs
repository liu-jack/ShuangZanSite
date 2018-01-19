using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class PackageCardbll : BaseBll<PackageCard>, IPackageCardBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.PackageCardDal;
        }
    }
}
