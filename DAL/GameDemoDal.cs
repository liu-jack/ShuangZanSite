using IDAL;
using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GameDemoDal : BaseDal<GameDemo>, IGameDemoDal
    {
        public List<Models.MapModel.GameDemoViewModel> GetGameDemo(Models.Params.DemoParam param)
        {
            string sql = "select  d.id,dbo.isnull(d.img,g.DescImg) as img ,gamename,  (select sum(AwardCoins) from [dbo].[GameDemoRequires] where GameDemoId=d.Id)as paySum from GameDemo d inner join game g on d.GameName=g.Name where d.state=1  order by d.InTime desc offset @pageIndex rows fetch next @pageSize row only";
            SqlParameter[] pms ={
               new SqlParameter("@pageIndex",param.PageIndex),
               new SqlParameter("@pageSize",param.PageSize),
               // new SqlParameter("@gamename",param.GameName) 
           };      
          var  data=  db.Database.SqlQuery<GameDemoViewModel>(sql, pms).ToList();
          if (!string.IsNullOrEmpty(param.GameName))
          {
              data = data.Where(d => d.GameName.Contains(param.GameName)).ToList();
          }
          return data;
        }
        public int GetGameDemoCount()
        {
            string sqlStr = "select count(*)from( select  (select sum(AwardCoins) from [dbo].[GameDemoRequires] where GameDemoId=d.Id)as paySum from GameDemo d inner join game g on d.GameName=g.Name where d.state=1 )as t";
           return db.Database.SqlQuery<int>(sqlStr).SingleOrDefault();
        }
    }
}
