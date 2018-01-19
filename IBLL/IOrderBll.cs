using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IOrderBll : IBaseBll<Order>
    {
        int MoreDelteOrder(IList<int> ids);
    }
}
