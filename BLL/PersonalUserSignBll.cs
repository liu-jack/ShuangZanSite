using IBLL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class PersonalUserSignBll : BaseBll<PersonalUserSign>,IPersonalUserSignBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.PersonalUserSignDal;
        }
     

        ///// <summary>
        ///// 签到
        ///// </summary>
        ///// <param name="userId">当前玩家</param>
        ///// <returns></returns>
        public List<SignViewModel> Sign(int userId, int p)
        {
            //@userid int,
            //@p int,                       --0-获取；1-签到
            //@num int out,                 --连续签到天数
            //@signHistory varchar(500) out --当月签到记录
            SqlParameter[] pms = {                                    
                                 new SqlParameter("@userid",SqlDbType.Int){Value=userId},
                                  new SqlParameter("@p",SqlDbType.Int){Value=p},
                                   new SqlParameter("@num",SqlDbType.Int),
                                    new SqlParameter("@signHistory",SqlDbType.NVarChar,500),
                                  };
            pms[2].Direction = ParameterDirection.Output;
            pms[3].Direction = ParameterDirection.Output;
          
          var data= dbSession.ExecuteQuery<SignViewModel>("exec sign @userid,@p,@num out,@signHistory out", pms).FirstOrDefault();
          var num = (int)pms[2].Value;
          var signHistory = pms[3].Value.ToString();
          var result = new SignViewModel () { Num=num,SignHistory=signHistory};
          List<SignViewModel> list = new List<SignViewModel>();
          list.Add(result);
          return list;
        }

    }
}
