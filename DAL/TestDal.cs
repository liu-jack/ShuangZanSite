using IDAL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
   public class TestDal:BaseDal<Tests>,ITestDal
    {
       //开测日历
        public IList<Models.MapModel.TestViewModel> TestDate(string date)
        {
            string sql = "select g.id,starttime,t.gamename,g.SmallImg,t.state,g.company,g.type,g.play,t.package as packageUrl,t.url from tests t inner join game g on t.gamename=g.name where starttime>=format(cast(@start as date),'yyyy-M-1') and starttime<format(dateadd(M,1,cast(@start as date)),'yyyy-M-1') order by starttime desc";
            SqlParameter[] pms = { new SqlParameter("@start",SqlDbType.NVarChar){Value=date}};
            return db.Database.SqlQuery<TestViewModel>(sql, pms).ToList();
        }
    }
}
