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
    public class NewsDal : BaseDal<News>, INewsDal
    {
        public List<Models.SqlMapModel.Sql_NewsModel> GetRelatedFunNews(int id,int top, string type)
        {
            string sql = "select top(@top) n.id,n.title,n.EditTitle,n.views,dbo.getimg(n.msg) as img,n.intime,count(m.NewsId)as msgNum from news as n  left join  LeaveMsg as m  on n.Id=m.NewsId    where n.id<@id and type=@type and State=1  group  by n.id,n.title,n.EditTitle,n.Views,n.msg,n.intime  order by intime desc";
            SqlParameter[] pms ={
               new SqlParameter("@id",SqlDbType.Int){Value=id},
               new SqlParameter("@top",SqlDbType.Int){Value=top},
                new SqlParameter("@type",SqlDbType.NVarChar){Value=type},
                                 };
            return db.Database.SqlQuery<Models.SqlMapModel.Sql_NewsModel>(sql, pms).ToList();
        }
    }
}
