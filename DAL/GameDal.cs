using IDAL;
using Models;
using Models.Enum;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace DAL
{
    public class GameDal : BaseDal<Game>, IGameDal
    {
        public IList<SqlGameModel> GetAllGamesInfo(GameParam param)
        {
            StringBuilder sbSql = new StringBuilder("select  name ,id,DescImg from  Game where State<>2");
            StringBuilder totalSql = new StringBuilder("select COUNT(*)  from Game  where  State<>2");
            List<SqlParameter> sqlPm = new List<SqlParameter>();
            List<SqlParameter> totalPm = new List<SqlParameter>();
            SqlParameter[] pms;
            if (!string.IsNullOrEmpty(param.Type))
            {
                sbSql.Append(" and Type=@type");
                sqlPm.Add(new SqlParameter("@type", SqlDbType.NVarChar, 50) { Value = param.Type });
                totalSql.Append(" and Type=@type");              
                totalPm.Add(new SqlParameter("@type", SqlDbType.NVarChar, 50) { Value = param.Type });
            }
            if (!string.IsNullOrEmpty(param.Theme))
            {
                sbSql.Append(" and Theme=@theme");
                totalSql.Append(" and Theme=@theme");
                sqlPm.Add(new SqlParameter("@theme", SqlDbType.NVarChar, 50) { Value = param.Theme });
                totalPm.Add(new SqlParameter("@theme", SqlDbType.NVarChar, 50) { Value = param.Theme });
            }
            if (!string.IsNullOrEmpty(param.Play))
            {
                sbSql.Append(" and Play=@play");
                totalSql.Append(" and Play=@play");
                sqlPm.Add(new SqlParameter("@play", SqlDbType.NVarChar, 50) { Value = param.Play });
                totalPm.Add(new SqlParameter("@play", SqlDbType.NVarChar, 50) { Value = param.Play });
            }
            if (!string.IsNullOrEmpty(param.Letter))
            {
                sbSql.Append(" and dbo.f_getpy(name)=@letter");
                totalSql.Append(" and dbo.f_getpy(name)=@letter");
                sqlPm.Add(new SqlParameter("@letter", SqlDbType.NVarChar, 50) { Value = param.Letter });
                totalPm.Add(new SqlParameter("@letter", SqlDbType.NVarChar, 50) { Value = param.Letter });
            }
            if (!string.IsNullOrEmpty(param.GameName))
            {
                sbSql.Append(" and name like'%'+@name+'%'");
                totalSql.Append(" and name like'%'+@name+'%'");
                sqlPm.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = param.GameName });
                totalPm.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = param.GameName });
            }
            if (!string.IsNullOrEmpty(param.Games) && param.Games == "hot")
            {
                sbSql.Append(" order by LikeNum desc  offset @pageSize*( @pageIndex-1 )rows fetch next @pageSize row only");
               
                sqlPm.Add(new SqlParameter("@pageIndex", SqlDbType.Int) { Value = param.PageIndex });
                sqlPm.Add(new SqlParameter("@pageSize", SqlDbType.Int) { Value = param.PageSize });
                pms = sqlPm.ToArray();
                if (totalPm.Count > 0)
                {
                    SqlParameter[] totalpms = sqlPm.ToArray();
                    param.Total = db.Database.SqlQuery<int>(totalSql.ToString(), totalpms).SingleOrDefault();
                }
                else {
                    param.Total = db.Database.SqlQuery<int>(totalSql.ToString()).SingleOrDefault();
                }
                return db.Database.SqlQuery<SqlGameModel>(sbSql.ToString(), pms.Select(x => ((ICloneable)x).Clone()).ToArray()).ToList();
            }
            else 
            {
                sbSql.Append(" order by InTime desc  offset @pageSize*( @pageIndex-1 ) rows fetch next @pageSize row only");
                sqlPm.Add(new SqlParameter("@pageIndex", SqlDbType.Int) { Value = param.PageIndex });
                sqlPm.Add(new SqlParameter("@pageSize", SqlDbType.Int) { Value = param.PageSize });
                pms = sqlPm.ToArray();
                if (totalPm.Count > 0)
                {
                    SqlParameter[] totalpms = sqlPm.ToArray();
                    param.Total = db.Database.SqlQuery<int>(totalSql.ToString(), totalpms).SingleOrDefault();
                }
                else
                {
                    param.Total = db.Database.SqlQuery<int>(totalSql.ToString()).SingleOrDefault();
                }
                return db.Database.SqlQuery<SqlGameModel>(sbSql.ToString(), pms.Select(x => ((ICloneable)x).Clone()).ToArray()).ToList(); 
            }
           
        }
    }
}
