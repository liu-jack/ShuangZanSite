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
    public class PersonalUserSignDetailBll : BaseBll<PersonalUserSignDetail>, IPersonalUserSignDetailBll 
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.PersonalUserSignDetailDal;
        }
        /// <summary>
        /// 获取按月签到的信息
        /// </summary>
        /// <param name="day"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> InMonthlySign(string day, int userId)
        {
            //    <按月签到信息>
            //    <sql><![CDATA[select dbo.StringJoin(format(intime,'yyyy-MM-dd')) from usersigndetail 
            //where userid=@userid and intime>=format(cast(@day as date),'yyyy-M-1') and intime<=format(dateadd(M,1,cast(@day as date)),'yyyy-M-1')]]></sql>
            //  </按月签到信息>
            string sql = "select dbo.StringJoin(format(intime,'yyyy-MM-dd')) from PersonalUserSignDetail   where userid=@userId and intime>=format(cast(@day as date),'yyyy-M-1') and intime<=format(dateadd(M,1,cast(@day as date)),'yyyy-M-1')";
            SqlParameter[] pms = { 
                              new SqlParameter("@userId",SqlDbType.Int){Value=userId},                          
                              new SqlParameter("@day",SqlDbType.NVarChar){Value=day}
                          };
            return this.dbSession.ExecuteQuery<string>(sql, pms);
        }
    }
}
