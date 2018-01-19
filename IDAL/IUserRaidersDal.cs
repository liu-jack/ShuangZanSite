using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IUserRaidersDal : IBaseDal<UserRaiders>
    {
        IList<Sql_RecRaidersModel> RelatedRaiders(int top, int id);
    }
}
