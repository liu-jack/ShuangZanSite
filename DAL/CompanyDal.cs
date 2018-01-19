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
    public class CompanyDal : BaseDal<Company>, ICompanyDal
    {
        public IList<CompanyViewModel> CompanyList(int pageIndex, int pageSize, out int total, string key, string py)
        {
            StringBuilder sb = new StringBuilder("select  id ,logo,systemname,CopanyMsg,state from company  where state=1 ");
            StringBuilder totalSql = new StringBuilder("select count(*) from company where state=1");
            
            List<SqlParameter> listParameters = new List<SqlParameter>();
            List<SqlParameter> totalParameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(key))
            {
                sb.Append("and systemname like'%'+@key+'%' ");
                totalSql.Append(" and systemname like'%'+@key+'%' ");
                listParameters.Add(new SqlParameter("@key", SqlDbType.NVarChar,100) { Value = key });
                totalParameters.Add(new SqlParameter("@key", SqlDbType.NVarChar, 100) { Value = key });
            }
            if (!string.IsNullOrEmpty(py))
            {
                 sb.Append("and dbo.f_getpy(systemname)=@py");
                 totalSql.Append(" and dbo.f_getpy(systemname)=@py");
                 listParameters.Add(new SqlParameter("@py", SqlDbType.NVarChar,1000) { Value = py });
                 totalParameters.Add(new SqlParameter("@py", SqlDbType.NVarChar, 1000) { Value = py });
            }
           
            sb.Append(" order by InTime desc  offset @pageIndex rows fetch next @pageSize row only");
            listParameters.Add( new SqlParameter("@pageIndex",SqlDbType.Int){Value=pageIndex});
            listParameters.Add(  new SqlParameter("@pageSize",SqlDbType.Int){Value=pageSize});
            SqlParameter[] pms = listParameters.ToArray();
            if (totalParameters.Count > 0)
            {
                SqlParameter[] totalpms = totalParameters.ToArray();
                total = db.Database.SqlQuery<int>(totalSql.ToString(), totalpms).SingleOrDefault();
            }
            else {
                total = db.Database.SqlQuery<int>(totalSql.ToString()).SingleOrDefault();
            }                            
            return db.Database.SqlQuery<CompanyViewModel>(sb.ToString(), pms).ToList();
        }
    }
}
