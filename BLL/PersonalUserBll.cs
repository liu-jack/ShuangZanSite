using IBLL;
using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class PersonalUserBll : BaseBll<PersonalUser>, IPersonalUserBll
    {
        public override void SetCurrentDal()
        {

            CurrentDal = dbSession.PersonalUserDal;
        }

        public IQueryable<PersonalUser> LoadPagePersonalUser(Models.Params.QueryParam param)
        {

            var allPerUser = dbSession.PersonalUserDal.LoadEntities(r => r.Id > 0);

            var allUserMsg = dbSession.UserMessageDal.LoadEntities(c => c.Id > 0);
                                                    
            var data = from t in allPerUser
                       join c in allUserMsg on t.Id equals c.UserId
                       select new { t.Id, t.InTime, t.UName, t.Mobile, t.QQ, t.WeChat, t.State, t.Email, c.Pay };
            if (!string.IsNullOrEmpty(param.SearchName))
            {
                data = data.Where(d => d.UName.Contains(param.SearchName));
            }
            if (!string.IsNullOrEmpty(param.Mobile))
            {
                data = data.Where(d => d.UName.Contains(param.SearchName));
            }
            param.Total = data.Count();
            return data.OrderByDescending(d => d.Id)
               .Skip(param.PageSize * (param.PageIndex - 1))
              .Take(param.PageSize).AsQueryable() as IQueryable<PersonalUser>;//类型转换为 IQueryable<Recharge>
        }
        public Sql_UserModel SendVCode(string mobile, string vcode, string ip)
        {
          return    dbSession.PersonalUserDal.SendVCode(mobile, vcode, ip);
        }
        public Sql_UserSignInModel SignIn(string uname, string pass, string mobile, string code, int tjid)
        {
            return dbSession.PersonalUserDal.SignIn(uname,pass,mobile,code,tjid);
        }
        public int CheckMobileCode(string mobile, string code)
        {
            string sql = "select 1 from  VerificationCode  where mobile=@mobile and code=@code  and DATEDIFF(MINUTE,intime,getdate())<300";
            SqlParameter[] pms = { 
                                  new SqlParameter("@mobile",SqlDbType.VarChar){Value=mobile},
                                new SqlParameter("@code",SqlDbType.VarChar){Value=code},
                                 };
            var  d= dbSession.ExecuteQuery<int>(sql,pms).FirstOrDefault();
            return d;
        }
        //todo..........脏数据
        //更新手机号
        public int UpdateUserMobile(string mobile, int id)
        {
            string sql = "if not exists(select 1 from PersonalUser where mobile=@mobile and id!=@id) and not exists(select 1 from company where phone=@mobile) update PersonalUser set mobile=@mobile where id=@id";
            SqlParameter[] pms = { 
                                  new SqlParameter("@mobile",SqlDbType.NVarChar){Value=mobile},
                                 new SqlParameter("@id",SqlDbType.Int){Value=id},
                                 };
           return  dbSession.ExecuteSql(sql,pms);
        }
        public int UpdateUserPassword(string pwd, int id)
        {
            string sql = "update PersonalUser set Password=@pwd where id=@id";
            SqlParameter[] pms = { 
                                  new SqlParameter("@pwd",SqlDbType.NVarChar){Value=pwd},
                                 new SqlParameter("@id",SqlDbType.Int){Value=id},
                                 };
            return dbSession.ExecuteSql(sql, pms);
        }


        public Sql_UserGetPwd UserGetPass(string mobile, string code)
        {
            SqlParameter[] pms = { 
                                  new SqlParameter("@mobile",SqlDbType.VarChar){Value=mobile},
                                 new SqlParameter("@code",SqlDbType.VarChar){Value=code},
                                 new SqlParameter("@pass",SqlDbType.VarChar,200),
                                 new SqlParameter("@err",SqlDbType.VarChar,10000),
                                 };
            pms[2].Direction = ParameterDirection.Output;
            pms[3].Direction = ParameterDirection.Output;
            var model = dbSession.ExecuteQuery <Sql_UserGetPwd>("exec UserGetPass", pms).FirstOrDefault();
            model = new Sql_UserGetPwd()
            {
               Pass=pms[2].Value.ToString(),
               Err=pms[3].Value.ToString()
            };
            return model;

        }
    }
}
