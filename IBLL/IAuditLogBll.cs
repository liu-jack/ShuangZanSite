using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IAuditLogBll : IBaseBll<AuditLog>
    {
        //礼包审核是1、新闻审核2 、开服审核0、对应AuditLog表中的tableId
        //获取最新审核未通过的日志信息
      
    }
}
