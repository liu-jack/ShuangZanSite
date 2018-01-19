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
    public class PersonalUserDal : BaseDal<PersonalUser>, IPersonalUserDal
    {
        public Sql_UserModel SendVCode(string mobile, string vcode, string ip)
        {
            SqlParameter [] pms = { 
                               new SqlParameter("@mobile",SqlDbType.VarChar,50){Value=mobile},
                                new SqlParameter("@code",SqlDbType.VarChar,50){Value=vcode},
                                 new SqlParameter("@ip",SqlDbType.VarChar,50){Value=ip},
                                  new SqlParameter("@outCode",SqlDbType.VarChar,50),                              
                               };
            pms[3].Direction = ParameterDirection.Output;
            var model = db.Database.SqlQuery<Sql_UserModel>("exec [dbo].[SendVCode] @mobile,@code, @ip,@outCode out", pms).FirstOrDefault();
          model = new Sql_UserModel() { 
            OutCode=pms[3].Value.ToString()
            };          
            return model;
        } 
        //注册
        public Sql_UserSignInModel SignIn(string uname, string pass, string mobile, string code, int tjid)
        {
            SqlParameter[] pms = { 
                               new SqlParameter("@uname",SqlDbType.VarChar,500){Value=uname},
                                new SqlParameter("@pass",SqlDbType.VarChar,500){Value=pass},
                                 new SqlParameter("@mobile",SqlDbType.VarChar,50){Value=mobile},
                                  new SqlParameter("@code",SqlDbType.VarChar,10){Value=code}, 
                                   new SqlParameter("@tjid",SqlDbType.Int){Value=tjid},
                                    new SqlParameter("@err",SqlDbType.VarChar,1000),
                               };
            pms[5].Direction = ParameterDirection.Output;
            var model = db.Database.SqlQuery<Sql_UserSignInModel>("exec [dbo].[RegionUser] @uname,@pass, @mobile,@code,@tjid,@err out", pms).FirstOrDefault();
            model = new Sql_UserSignInModel()
            {
                Err = pms[5].Value.ToString()
            };
            return model;
        }
    }
}
