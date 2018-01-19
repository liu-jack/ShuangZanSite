using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace BLL
{
    public class OpenServiceBll : BaseBll<OpenService>, IOpenServiceBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.OpenServiceDal;
        }
        #region 后台开服审核管理
        public IQueryable<ServiceViewModel> LoadOpenService(ServiceParam param)
        {
            var allServices = dbSession.OpenServiceDal.LoadEntities(o => o.Id > 0).Select(s => new { s.Id,s.Rec_Hot,s.StartTime,s.State,s.ServerName,s.CheckName,s.GameName,s.InTime,s.Url,s.CompanyId,s.PackageId});
            var allCpy = dbSession.CompanyDal.LoadEntities(c => true);
            var allPackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0);
            var datas = from s in allServices
                        join c in allCpy on s.CompanyId equals c.Id
                        join p in allPackage on s.PackageId equals p.Id into ss
                        from ssi in ss.DefaultIfEmpty()                        
                        select new ServiceViewModel()
                        {
                            CompanyId = c.Id,
                            GameName = s.GameName,
                            Id = s.Id,
                            InTime = s.InTime,
                            Type = s.Rec_Hot,
                            State = s.State,
                            StartTime = s.StartTime,
                            Url = s.Url,
                            SystemName = c.SystemName,
                            GiftName = ssi != null ? ssi.GiftName : null,
                            ServerName = s.ServerName,
                            GiftType = ssi != null ? ssi.Type : null,
                            CheckName = s.CheckName
                        };
            if (!string.IsNullOrEmpty(param.GameName))
            {
                datas = datas.Where(d => d.GameName.Contains(param.GameName));
            }
            if (!string.IsNullOrEmpty(param.CompanyName))
            {
                datas = datas.Where(d => d.SystemName.Contains(param.CompanyName));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                datas = datas.Where(d => d.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.State))
            {
                datas = datas.Where(d => d.State.Contains(param.State));
            }
            if (param.StartDay.HasValue||param.EndDay.HasValue)
            {
                param.EndDay = param.EndDay.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(00);
                datas = datas.Where(d => d.StartTime >= param.StartDay && d.StartTime <= param.EndDay);
            }
            param.Total = datas.Count();
            return datas.OrderByDescending(d => d.InTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
        #region 批量审核开服表
        public int MoreCheckOpenService(IList<int> ids, string checkIsPass, string currentAdmin)
        {
            foreach (var id in ids)
            {
                //先查询出来每个ID对应的数据
                var open = dbSession.OpenServiceDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (checkIsPass == "1")//1   代表审核通过
                {   //然后将对应的值更新到数据库
                    open.State = "1";
                    open.CheckName = currentAdmin;//当前审核人
                    dbSession.OpenServiceDal.Update(open);
                }
                else if (checkIsPass == "0")//0  代表审核未通过
                {
                    open.State = "0";
                    open.CheckName = currentAdmin;
                    dbSession.OpenServiceDal.Update(open);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        public bool ReturnServiceNum(int serviceId, int companyId)
        {
            return dbSession.OpenServiceDal.ReturnServiceNum(serviceId, companyId);
        }     
        #region 厂商开服详情
        /// <summary>
        /// 厂商开服详情
        /// </summary>
        /// <param name="id">当前的id号</param>
        /// <param name="cpy">当前的厂商用户</param>
        /// <returns></returns>    
        public IQueryable<OpenService_Package> GetCpyServiceDetail(int id, Company cpy)
        {
            var allService = dbSession.OpenServiceDal.LoadEntities(s => s.Id > 0).Select(s => new { s.PackageId, s.StartTime, s.GameName, s.State, s.ServerName, s.CompanyId, s.Rec_Hot, s.Id ,s.Url});
            var allpackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0).Select(p => new { p.Id, p.GiftName });
            var data = from s in allService
                       join p in allpackage on s.PackageId equals p.Id into ps
                       from psi in ps.DefaultIfEmpty()
                       where s.CompanyId == cpy.Id && s.Id == id
                       select new OpenService_Package
                       {
                           Id=s.Id,
                           companyId=s.CompanyId,
                           StartDate = s.StartTime,
                           GameName = s.GameName,
                           ServerName = s.ServerName,
                           GiftName = psi!=null?psi.GiftName:null,
                           Rec_Hot = s.Rec_Hot,
                           Url=s.Url
                       };
            return data;
        }
        #endregion
        #region 当天开服总条数          
        public int allCount()
        {
               int state = (short)CheckState.Pass;
                string pass = state.ToString();
           return  dbSession.OpenServiceDal.LoadEntities(s => s.State == pass).Where(s => DbFunctions.DiffDays(s.StartTime, DateTime.Now) == 0).Select(s => s.Id).Count();
        }
        #endregion
        #region 得到开服的信息
        /// <summary>
        /// 得到开服的信息
        /// </summary>
        /// <returns></returns>
        public List<Sql_OpenServer> GetAllServiceInfo(int take, string type)
        {           
            #region old
            //int state = (short)CheckState.Pass;
            //string pass = state.ToString();

            //var allService = dbSession.OpenServiceDal.LoadEntities(s => s.State == pass && s.Rec_Hot == type)
            //     .Where(s => DbFunctions.DiffDays(s.StartTime, DateTime.Now) == 0)

            //    .Select(s => new { s.Id, s.PackageId, s.StartTime, s.GameName, s.State, s.ServerName, s.CompanyId, s.Rec_Hot, s.Url });
            //var allpackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0).Select(p => new { p.Id, p.GiftName });
            //var allCpy = dbSession.CompanyDal.LoadEntities(c => c.State == 1).Select(c => new { c.Id, c.SystemName });

            //var data = (from s in allService
            //            join p in allpackage on s.PackageId equals p.Id into pc
            //            from pci in pc.DefaultIfEmpty()
            //            join c in allCpy on s.CompanyId equals c.Id
            //            //二次再过滤一下
            //            where (s.State == pass && s.Rec_Hot == type)
            //            select (new ServiceViewModel()
            //            {
            //                Id = s.Id,
            //                GameName = s.GameName,
            //                ServerName = s.ServerName,
            //                StartTime = s.StartTime,
            //                GiftName = pci.GiftName,
            //                SystemName = c.SystemName,
            //                Url = s.Url,
            //                Type = s.Rec_Hot,
            //            })).OrderBy(d => d.StartTime).Take(take).ToList();
            //return data; 
            #endregion
            SqlParameter[] pms = {                                 
                                 new SqlParameter("@take",SqlDbType.Int){Value=take},
                                 new SqlParameter("@type",SqlDbType.NVarChar){Value=type},
                                 };
            string sql=null;
            if (type=="1")
            {
                 sql = "select top(@take) s.id,s.gamename,starttime,s.servername,c.systemname,s.packageid,p.GiftName, (select count(1) from packagecard pc where pc.packageid=s.packageid and pc.userid>0) as packagecardcount,s.url ,s.rec_hot as type from OpenService  s inner join company c on s.companyid=c.id left join package p on s.packageid=p.id where cast(starttime as date)=cast(getdate() as date) and s.state=1 and c.state=1 and (p.state in (1,3) or p.state is null) and s.Rec_Hot=@type order by  s.starttime,s.rec_hot desc";
            }
            else if (type=="2")
            {
                sql = "select top(@take) s.id,s.gamename,starttime,s.servername,c.systemname,s.packageid,p.GiftName,(select count(1) from packagecard pc where pc.packageid=s.packageid and pc.userid>0)  as packagecardcount,s.url ,s.rec_hot as type from OpenService  s inner join company c on s.companyid=c.id left join package p on s.packageid=p.id where cast(starttime as date)=cast(getdate() as date) and s.state=1 and c.state=1 and (p.state in (1,3) or p.state is null) and s.Rec_Hot=@type order by NEWID()";
            }          
            return dbSession.ExecuteQuery<Sql_OpenServer>(sql,pms);
        }       
        #endregion
        #region 开服搜索结果

        public IQueryable<ServiceViewModel> GetServiceSearchData(string key)
        {

            var allService = dbSession.OpenServiceDal.LoadEntities(s => DbFunctions.DiffDays(s.StartTime, DateTime.Now) == 0);            
            var allpackage = dbSession.PackageDal.LoadEntities(p =>p.State=="1"||p.State=="3"||p.State==null);
            var allCpy = dbSession.CompanyDal.LoadEntities(c =>c.State==1);
            var  packageCard=dbSession.PackageCardDal.LoadEntities(pc=>true);
            var data = from s in allService                   
                        join c in allCpy on s.CompanyId equals c.Id
                       join p in allpackage on s.PackageId equals p.Id into ps
                       from psi in ps.DefaultIfEmpty()
                       let packagecardcount=(from pc  in packageCard  where s.PackageId==pc.PackageId&&pc.UserId>0 select pc).Count()
                       //where DbFunctions.DiffDays(s.StartTime,DateTime.Now)==0&&s.State=="1"&&c.State==1&&(psi.State=="1"||psi.State=="3"||psi.State==null)  
                       where (s.GameName.Contains(key)||c.SystemName.Contains(key))                               
                        select (new ServiceViewModel()
                        {
                            
                            Id = s.Id,
                            GameName = s.GameName,
                            ServerName = s.ServerName,
                            StartTime = s.StartTime,
                            GiftName = psi!=null?psi.GiftName:null,
                            SystemName = c.SystemName,
                            Url = s.Url,
                            Type = s.Rec_Hot,
                            PackageCardCount=packagecardcount
                        });
           
            return data.OrderBy(d=>d.StartTime).AsNoTracking().AsQueryable();
        } 
        #endregion
        #region 同游戏的相关开服
        public List<Sql_ServerModel> RelatedGameService(int packageId)
        {
            string sql = "select s.gamename,s.starttime as startTime ,s.servername,c.systemname,p1.GiftName ,s.url,s.rec_hot from OpenService s inner join package p on s.gamename=p.gamename inner join company c on s.companyid=c.id left join package p1 on s.packageid=p1.id where s.state=1 and cast(starttime as date)=cast(getdate() as date) and c.state=1 and p.state in (1,3) and p.startdate<getdate() and p.enddate>=cast(getdate() as date) and p.id=@packageId order by s.starttime,rec_hot desc";
            SqlParameter[] pms ={
                                    new SqlParameter("@packageId",SqlDbType.Int){Value=packageId},                                                                    
                                };
            return dbSession.ExecuteQuery<Sql_ServerModel>(sql, pms);
        } 
        #endregion
        #region 按照当天往前7天开服数据，开服条数最多的10个游戏排名(网站首页)
        /// <summary>
        /// 按照当天往前7天开服数据，开服条数最多的10个游戏排名(网站首页)
        /// </summary>
        /// <returns></returns>
        public List<ServiceViewModel> WillSevenDayService()
        {
            int state = (short)CheckState.Pass;
            string pass = state.ToString();
            var allService = dbSession.OpenServiceDal.LoadEntities(s => s.State == pass)
                .Where(s => DbFunctions.DiffDays(s.StartTime, DateTime.Now) <= 7)
                .Select(s => new { s.Id, s.Rec_Hot, s.GameName ,s.StartTime});
            var game = dbSession.GameDal.LoadEntities(g => true).Select(g => new { g.Id, g.Name, g.LikeNum, g.SmallImg, g.Type });
            //1、查出一周内将要开服的游戏名称和游戏库相匹配的游戏名
            //2、根据此信息查出开服的数量、然后倒序排列
            var data = (from s in allService
                        join g in game on s.GameName equals g.Name
                        let ServiceCount = (from ss in allService where ss.GameName == g.Name select ss.Id).Count()                     
                        select (new ServiceViewModel()
                        {
                            Id = g.Id,
                            GameName = s.GameName,
                            LikeNum = g.LikeNum ?? 0,
                            Type = g.Type,
                            SmallImg = g.SmallImg,
                            ServiceCount = ServiceCount//开服条数最多
                        })).Distinct().AsNoTracking().OrderByDescending(d => d.ServiceCount).Take(10).ToList();
            return data;
        } 
        #endregion
        #region 开服历史游戏名称
        /// <summary>
        /// 开服历史游戏名称
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<MyServerGame> MyHistoryGameName(int companyId)
        {
            return dbSession.OpenServiceDal.MyHistoryGameName(companyId);
        } 
        #endregion
        #region 当前厂商开服
        public List<Sql_ServerModel> CurrentCpyService(int companyId)
        {
            return dbSession.OpenServiceDal.CurrentCpyService(companyId);
        } 
        #endregion
        #region 右侧开服
        public List<Sql_OpenServer> IndexCurrntService(string type, int take)
        {
            string sql = "select top(@take) s.id,s.gamename, starttime,s.servername,c.systemname,s.packageid,p.GiftName,(select count(1) from packagecard pc where pc.packageid=s.packageid and pc.userid>0) as packagecardcount,s.url,s.rec_hot as type from OpenService s inner join company c on s.companyid=c.id left join package p on s.packageid=p.id where cast(starttime as date)=cast(getdate() as date) and s.state=1 and c.state=1 and (p.state in (1,3) or p.state is null) and s.Rec_Hot=@type order  by  s.StartTime,s.Rec_Hot desc";
            SqlParameter[] pms = {                                 
                                  new SqlParameter("@take",SqlDbType.Int){Value=take},
                                 new SqlParameter("@type",SqlDbType.NVarChar){Value=type},
                                 };
            return dbSession.ExecuteQuery<Sql_OpenServer>(sql, pms);
        } 
        #endregion



    
    }

}
