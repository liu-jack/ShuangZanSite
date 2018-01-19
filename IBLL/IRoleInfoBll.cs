using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IRoleInfoBll : IBaseBll<RoleInfo>
    {
        int DeleteRoles(IList<int> ids);
    }
}
