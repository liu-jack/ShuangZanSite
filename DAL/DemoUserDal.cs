using IDAL;
using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DemoUserDal : BaseDal<DemoUser>, IDemoUserDal
    {
        public Sql_DemoCheckModel DemoCheckEvent(int accountId, int demoId, int userId, int requireId, string state, string type, string reason)
        {
            SqlParameter[] pms = { 
                                 new SqlParameter("@demoid",SqlDbType.Int){Value=demoId},
                                   new SqlParameter("@accountid",SqlDbType.Int){Value=accountId},
                                     new SqlParameter("@requireid",SqlDbType.Int){Value=requireId},
                                       new SqlParameter("@userid",SqlDbType.Int){Value=userId},
                                         new SqlParameter("@state",SqlDbType.VarChar,5){Value=state},
                                          new SqlParameter("@type",SqlDbType.VarChar,100){Value=type},
                                          new SqlParameter("@reason",SqlDbType.VarChar,200){Value=reason},
                                            new SqlParameter("@err",SqlDbType.VarChar,1000),
                                 };
            pms[7].Direction = ParameterDirection.Output;
            var data = db.Database.SqlQuery<Sql_DemoCheckModel>("exec DemosConfirm @demoid,@accountid,@requireid,@userid,@state,@type,@reason,@err out", pms).FirstOrDefault();
            Sql_DemoCheckModel model = new Sql_DemoCheckModel() {
                Err = pms[7].Value.ToString()
            };
            return model;
        }


     
    }
}
