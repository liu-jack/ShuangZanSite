using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
  public  interface ITestDal:IBaseDal<Tests>
    {
      IList<TestViewModel> TestDate(string date);
    }
}
