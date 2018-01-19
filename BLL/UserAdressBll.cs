using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace BLL
{
    public class UserAdressBll : BaseBll<UserAdress>,IUserAdressBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.UserAdressDal;
        }

        public bool UpdateUserAddress(int id,int userId)
        {
            string sql = "update  UserAdress set isdefault=0 where userid=@userid update    UserAdress set isdefault=1 where id=@id and userid=@userid";
            SqlParameter[] pms = {
                                     new SqlParameter("@id",SqlDbType.Int){Value=id},
                              new SqlParameter("@userId",SqlDbType.Int){Value=userId},                          
                          };
            return this.dbSession.ExecuteSql(sql, pms)>0;          
        }

        public bool AddUserAddress(int userId,int isdefault,string name,string address,string phone)
        {


            string sql = "declare @cou int select @cou=count(1) from useradress where userid=@userId                                                                                if @cou<3 begin if @isdefault=1  update useradress set isdefault=0 where userid=@userid        insert into useradress (userid,name,address,phone,isdefault,intime) values (@userId,@name,@address,@phone,@isdefault,getdate()) end";
            SqlParameter [] pms = {
                                      new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                                      new SqlParameter("@name",SqlDbType.NVarChar){Value=name},
                                      new SqlParameter("@address",SqlDbType.NVarChar){Value=address},
                                      new SqlParameter("@phone",SqlDbType.NVarChar){Value=phone},
                                      new SqlParameter("@isdefault",SqlDbType.Int){Value=isdefault},
                                  };
            try
            {
                return this.dbSession.ExecuteSql(sql, pms) > 0;
            }
            catch (Exception ex)
            {
                
                throw new Exception("出错啦"+ ex.Message);
            }
            
        }
    }
}
