using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DalFactory
{
    public class DbSessionFactory
    {
        public static IDbSession GetCurrentDbSession()
        {
            IDbSession dbSession = HttpContext.Current.Items["DbSession"] as IDbSession;
            if (dbSession == null)
            {
                dbSession = new DbSession();
                HttpContext.Current.Items.Add("DbSession", dbSession);
            }
            return dbSession;
        }
    }
}
