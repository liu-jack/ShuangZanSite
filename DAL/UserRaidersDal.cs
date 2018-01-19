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
    public class UserRaidersDal : BaseDal<UserRaiders>, IUserRaidersDal
    {
        public IList<Sql_RecRaidersModel> RelatedRaiders(int top, int id)
        {
            string sql = "select top(@top) t2.id,t2.title,t2.EditTitle,t2.intime from UserRaiders t1 inner join UserRaiders t2 on t1.gamename=t2.gamename where t1.id=@id and t2.id!=@id and t1.state=1 and t2.state=1 order by t2.intime desc";
            SqlParameter[] pms ={
               new SqlParameter("@id",SqlDbType.Int){Value=id},
               new SqlParameter("@top",SqlDbType.Int){Value=top},               
                                 };
            return db.Database.SqlQuery<Sql_RecRaidersModel>(sql, pms).ToList();
        }
    }
}
