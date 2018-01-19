using IDAL;
using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace DAL
{
    public class PackageDal : BaseDal<Package>, IPackageDal
    {
        /// <summary>
        /// 领号
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<PackageCodeViewModel> GetPackageNumber(int packageId, int userId)
        {
            string sql = "	update top(1) PackageCard set userid=@userId,intime=getdate()	output deleted.Code 	where packageid=@packageId and userid=0 select p.msg,p.memo,p.gamename,c.systemname from Package p inner join company c on p.companyid=c.id where p.id=@packageId";
            SqlParameter[] pms ={
                                    new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                                     new SqlParameter("@packageId",SqlDbType.NVarChar){Value=packageId},
                                     // new SqlParameter("@Code",SqlDbType.NVarChar,300)
                                };
           // pms[2].Direction = ParameterDirection.Output;
           // var code = pms[0].Value;
           return db.Database.SqlQuery<PackageCodeViewModel>(sql,pms).ToList();       
        }

        public IList<Sql_PackageDetail> GetPackageNumber(int packageId, int userId, out Sql_PackageCode code)
        {
            string sql = "update top(1) PackageCard set userid=@userId,intime=getdate()	output deleted.Code 	where packageid=@packageId and userid=0";
              SqlParameter[] pms ={
                                    new SqlParameter("@userId",SqlDbType.Int){Value=userId},
                                     new SqlParameter("@packageId",SqlDbType.NVarChar){Value=packageId},
                                     // new SqlParameter("@Code",SqlDbType.NVarChar,300)
                                };
              code = db.Database.SqlQuery<Sql_PackageCode>(sql, pms).FirstOrDefault();
              string str = "select p.msg,p.memo,p.gamename,c.systemname from Package p inner join company c on p.companyid=c.id where p.id=@packageId";
              SqlParameter[] ps ={                                 
                                     new SqlParameter("@packageId",SqlDbType.NVarChar){Value=packageId},                                   
                                };
              return db.Database.SqlQuery<Sql_PackageDetail>(str, ps).ToList();

        }
        /// <summary>
        /// 热游礼包
        /// </summary>
        /// <returns></returns>
        public IList<Models.SqlMapModel.Sql_PackageModel> PackageHot()
        {
            //--创建公共表达式
            string sql = "with cte_name(gamename,lasttime) as( select top 6 gamename,max(rec_hot_time) from Package where rec_hot=1  group by gamename order by 2 desc)select name,SmallImg,(select c.systemname, c.id,max(p.rec_hot_time) as rec_hot_time from package p inner join company c on c.id=p.companyId where p.gamename=g.name and c.state=1 and p.state in (1,3)  group by c.systemname, c.id for xml path('N'),root('P')) as data from cte_name t inner join game g on t.gamename=g.name order by t.lasttime desc";
            return db.Database.SqlQuery<Sql_PackageModel>(sql).ToList();
        }

        public IList<PackageViewModel> PackageRecommend(int top, int type)
        {
            string sql = "select top(@top) p.id,g.SmallImg,g.DescImg,c.systemname,p.GiftName,p.gamename from Package p inner join game g on p.gamename=g.name inner join company c on c.id=p.companyId where p.state in (1,3) and p.rec=1 and c.state=1 and p.type=@type order by p.rec_time desc";
            SqlParameter[] pms ={
                                    new SqlParameter("@top",SqlDbType.Int){Value=top},
                                     new SqlParameter("@type",SqlDbType.Int){Value=type},                                   
                                };
            return db.Database.SqlQuery<PackageViewModel>(sql,pms).ToList();
        }



        public int PackageSingleCheck(int id, string state, string reason, string currentAdmin)
        {
            string sql = " if @state=0  insert into auditlog (companyid,title,type,tableid,reason)  select companyid,concat(gamename,' ',GiftName,' 礼包，未通过审核，原因：'),1,id,@reason from package where id=@id and state=2 update package set state=@state,CheckName=@checkname where id=@id and state=2";
            SqlParameter[] pms ={
                                    new SqlParameter("@state",SqlDbType.NVarChar){Value=state},
                                     new SqlParameter("@id",SqlDbType.Int){Value=id},  
                                   new SqlParameter("@reason",SqlDbType.NVarChar){Value=reason}, 
                                     new SqlParameter("@checkname",SqlDbType.NVarChar){Value=currentAdmin},  
                                };
            return db.Database.ExecuteSqlCommand(sql, pms);
        }


      
    }
}
