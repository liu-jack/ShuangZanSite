using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface IDemoUserDal : IBaseDal<DemoUser>
    {
        Sql_DemoCheckModel DemoCheckEvent(int accountId, int demoId, int userId, int requireId, string state, string type, string reason);
    }
}
