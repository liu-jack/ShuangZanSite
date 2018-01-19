using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class UserMessageDal : BaseDal<UserMessage>, IUserMessageDal
    {

        public bool UserRechargeCoolCoins(string userId, string trade_no, string rmb, string sb, string feetype)
        {
            string sql = "if not exists(select 1 from UserMessage where userid=@userid and orderNo=@trade_no)                                       insert into UserMessage (userid,title,msg,paytype,pay,memo1,memo2,orderNo)                                               values (@userid,'爽币充值—'+@sb+'爽币充值成功','亲爱的爽赞会员恭喜您，您充值的'+@sb+'爽币已经到账啦！赶紧去“淘福利”版块看看吧~','2'/*充值方式*/,@sb,@rmb,@feetype,@trade_no)";
            SqlParameter[] pms = {
                                 new  SqlParameter("@userid",userId),
                                 new  SqlParameter("@trade_no",SqlDbType.NVarChar){Value=trade_no},
                                 new  SqlParameter("@sb",sb),
                                 new  SqlParameter("@rmb",SqlDbType.NVarChar){Value=rmb},
                                 new  SqlParameter("@feetype",SqlDbType.NVarChar){Value=feetype},                               
                                 };
            return db.Database.ExecuteSqlCommand(sql, pms) > 0;
        }
    }
}
