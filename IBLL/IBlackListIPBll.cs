using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IBlackListIPBll : IBaseBll<BlackListIP>
    {
        List<string > GetBlackListIP();
        bool CheckBlackListIp(string ip);


    }
}
