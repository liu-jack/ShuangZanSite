using Models;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface ICompanyGameDal : IBaseDal<CompanyGame>
    {
        //IQueryable<Sql_CpyGameModel> currentCpyGame(GameParam param);
        //int CurrentCpyGameCount(int id, string type);
    }
}
