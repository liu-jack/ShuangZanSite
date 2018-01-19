
using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IUserInfoBll : IBaseBll<UserInfo>
    {
        int DeleteUsers(IList<int> ids);

        IQueryable<UserInfo> LoadPageUserInfos(QueryParam param);

        bool SetUserRole(int userId, List<int> selectRoleIds);
    }
}
