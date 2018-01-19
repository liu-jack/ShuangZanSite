using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface ICompanyGameBll : IBaseBll<CompanyGame>
    {
      IQueryable<CompanyGame> LoadCpyGame(CpyGameParam param);
    }
}
