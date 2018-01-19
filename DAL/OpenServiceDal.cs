using IDAL;
using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace DAL
{
    public class OpenServiceDal : BaseDal<OpenService>, IOpenServiceDal
    {

        public IList<MyServerGame> MyHistoryGameName(int companyId)
        {
            string sql = "select gamename  from  OpenService where companyid=@companyid group by gamename  order by max(intime) desc";
           SqlParameter [] pms={
           new SqlParameter("@companyid",companyId)
            
           };
           return db.Database.SqlQuery<MyServerGame>(sql, pms).ToList();
        }

        public List<Sql_ServerModel> CurrentCpyService(int companyId)
        {
            string sql = "select top 4 g.name as GameName,s.servername,s.starttime,s.url,p.GiftName ,s.rec_hot 	from OpenService s inner join game g on s.gamename=g.name	left join package p on s.packageid=p.id		where s.companyid=@companyid and s.state=1 and cast(starttime as date)=cast(getdate() as date)		order by starttime desc";
            SqlParameter[] pms ={
           new SqlParameter("@companyid",companyId)            
           };
            return db.Database.SqlQuery<Sql_ServerModel>(sql, pms).ToList();
        }

        public bool ReturnServiceNum(int serviceId, int companyId)
        {
            string sql = "if exists(select 1 from Recharge r inner join RechargedUsed  ru on r.id = ru.rechargeid  where ru.OpenServiceId = @serviceId and r.CompanyId=@companyId) update Recharge set used = used - 1 where id  = (select rechargeid from RechargedUsed  where OpenServiceId = @serviceId)";
            SqlParameter[] pms ={
           new SqlParameter("@serviceId",serviceId),   
           new SqlParameter("@companyId",companyId)            
           };
            return db.Database.ExecuteSqlCommand(sql, pms)>0;
        }
    }
}
