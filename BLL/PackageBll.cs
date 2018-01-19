using IBLL;
using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
namespace BLL
{
    public class PackageBll : BaseBll<Package>, IPackageBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.PackageDal;
        }
        #region 批量审核
        public int MoreCheckPachages(IList<int> ids, string checkIsPass, string currentAdmin)
        {
            foreach (var id in ids)
            {
                //先查询出来每个ID对应的数据
                var package = dbSession.PackageDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (checkIsPass == "1")//1   代表审核通过
                {   //然后将对应的值更新到数据库
                    package.State = "1";
                    package.CheckName = currentAdmin;//当前审核人
                    dbSession.PackageDal.Update(package);
                }
                else if (checkIsPass == "0")//0  代表审核未通过
                {
                    package.State = "0";
                    package.CheckName = currentAdmin;
                    dbSession.PackageDal.Update(package);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region join联合查询礼包详情返回的结果
        public IQueryable<Package_Card> CpyPackageDetails(int id, Company currentCpy)
        {
          
            var allCard = dbSession.PackageCardDal.LoadEntities(c => true).Select(c => new { c.Code, c.PackageId });
            var allPackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0)
                                                  .Select(p => new { p.CompanyId, p.Type, p.EndDate, p.StartDate, p.State, p.GameName, p.GiftName, p.ServerName, p.Id, p.Url, p.Memo, p.Msg });
            var data = from c in allCard
                       join p in allPackage on c.PackageId equals p.Id
                       where p.Id == id && p.CompanyId == currentCpy.Id
                       select new Package_Card()
                       {
                           GiftType = p.Type,
                           EndDate = p.EndDate,
                           StartDate = p.StartDate,
                           State = p.State,
                           GameName = p.GameName,
                           GiftName = p.GiftName,
                           ServerName = p.ServerName,
                           CompanyId=p.CompanyId,
                           Id = p.Id,
                           Url = p.Url,
                           Memo = p.Memo,
                           Msg = p.Msg,
                           Card = c.Code
                       };
            return data;
        } 
        #endregion
        #region 删除厂商礼包
        public int CpyPackageDelete(int id)
        {
            //拿到当前礼包的id\并且把对应的company表数据必须删除  不然容易出现脏数据
            Package p = new Package() { Id = id };
            dbSession.PackageDal.Delete(p);
            // Company cpy =new Company(){};
            //  dbSession.CompanyDal.Delete(cpy);
            return dbSession.SaveChanges();
        }
        #endregion
        #region 我的礼包
        /// <summary>
        /// 我的礼包
        /// </summary>
        /// <param name="user">当前用户的id</param>
        /// <param name="type">礼包类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">一夜多少条</param>
        /// <param name="count">总数量</param>
        /// <returns></returns>
        public IQueryable<Package_Card> GerMyPackage(PersonalUser user, string type, int pageIndex, int pageSize, out int count)
        {
            var allCard = dbSession.PackageCardDal.LoadEntities(c => true);
            var allPackage = dbSession.PackageDal.LoadEntities(p => p.Id > 0);
            var data = from c in allCard
                       join p in allPackage on c.PackageId equals p.Id
                       where c.UserId == user.Id
                       select new Package_Card()
                       {
                           EndDate = p.EndDate,
                           StartDate = p.StartDate,
                           State = p.State,
                           GameName = p.GameName,
                           InTime = c.InTime,
                           Id = p.Id,
                           Url = p.Url,
                           Card = c.Code,
                           GiftType = p.Type,
                           GiftName=p.GiftName
                       };
                      
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(d => d.GiftType.Contains(type));                         
            }
            count = data.Count();
            return data .OrderByDescending(p => p.InTime)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize).AsQueryable()
                          ; 
        }
        #endregion      
        #region 礼包的搜索及列表           
        public IQueryable<PackageViewModel> LoadPackageData(Models.Params.PackageParam param)
        {
            var packagecard = dbSession.PackageCardDal.LoadEntities(p => true);
            var cpy = dbSession.CompanyDal.LoadEntities(c => true);
            var allPackage = dbSession.PackageDal.LoadEntities(g => true);          
            var game = dbSession.GameDal.LoadEntities(g => true);          
            var data = from p in allPackage
                       join ga in game on p.GameName equals ga.Name
                       join c in cpy on p.CompanyId equals c.Id                    
                       let UsedCode = (from a in packagecard where p.Id == a.PackageId && a.UserId == 0 select a.Code).Count()
                       where p.State == "1"||p.State=="3"                     
                       select (new PackageViewModel()
                       {
                           Id = p.Id,
                           GameName = p.GameName,
                           SystemName = c.SystemName,
                           GiftName = p.GiftName,
                           InTime = p.InTime,
                           StartTime = p.StartDate,
                           EndTime = p.EndDate,
                           Used = UsedCode,
                           Type=p.Type
                       });
            if (!string.IsNullOrEmpty(param.Type))
            {
                data = data.Where(d=> d.Type.Contains(param.Type));
            }
            if (!string.IsNullOrEmpty(param.Key))
            {
                data = data.Where(d => d.GameName.Contains(param.Key) || d.SystemName.Contains(param.Key));
            }
            if (!string.IsNullOrEmpty(param.Systemname))
            {
                data = data.Where(d => d.SystemName.Contains(param.Systemname));
            }     
            param.Total=data.Count();
            return data.AsNoTracking().OrderByDescending(d => d.StartTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable(); 
        }
        #endregion
        #region 校验前台传递的类型是否包含
        /// <summary>
        /// 校验前台传递的类型是否包含
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckPackageType(string type)
        {
            Guid  guid=Guid.NewGuid();
           object   obj= Common.CacheHelper.Get(guid.ToString());
           List<string> newlist = null;
          
           if (obj == null)
           {
               List<Package> list = dbSession.PackageDal.LoadEntities(p => p.State == "1").ToList().Select(p => new Package() { Type = p.Type }).ToList();
              newlist = new List<string>();
               foreach (var item in list)
               {
                   newlist.Add(item.Type);
               }
               try
               {
                   if (newlist != null)
                   {
                       Common.CacheHelper.WriteCache(guid.ToString(), newlist, DateTime.Now.AddMonths(1));
                   }
               }
               catch (Exception ex)
               {
                   throw new Exception("礼包校验类型的缓存抛异常了！" + ex.Message);
               }
           }
           else {
               newlist = obj as List<string>;
           }       
            return newlist.Contains(type);
        } 
        #endregion
        #region 前台礼包详情       
        public PackageViewModel2 GetPackageDetails(int packageId)
        {
            string sql = "select p.id,p.gamename,p.msg,memo,c.systemname,p.type,p.startdate as StartTime ,p.enddate as EndTime,g.DescImg,p.GiftName ,p.url,(select count(1) from PackageCard where packageid=p.id and userid=0) as Used, (select count(1) from PackageCard where packageid=p.id) as Cardcou1 from package p inner join company c on p.companyid=c.id inner join game g on p.gamename=g.name where p.state in (1,3)  and p.id=@packageId and c.state=1";
            //and startdate<getdate() and enddate>=cast(getdate() as date)
            SqlParameter[] pms ={                                 
                                     new SqlParameter("@packageId",SqlDbType.Int){Value=packageId},                                      
                                };
            return dbSession.ExecuteQuery<PackageViewModel2>(sql, pms).FirstOrDefault();
        }
        #endregion
        #region 同游戏名的相关礼包推荐
        /// <summary>
        /// 同游戏名的相关礼包推荐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Sql_PackageModel2> SameGamePackage(int id)
        {          
            string sql = "select top(4) p2.id,p2.gamename,c.systemname as companyname,p2.GiftName,p2.startdate as StartTime,p2.enddate as EndTime,(select count(1) from PackageCard where packageid=p2.id and userid=0) as Used from package p1 inner join package p2 on p1.gamename=p2.gamename inner join company c on p2.companyid=c.id where p1.id=@id and p2.id!=@id and c.state=1 and p1.state in (1,3) --and p1.startdate<getdate() and p1.enddate>cast(getdate() as date) and p2.state in (1,3) --and p2.startdate<getdate() and p2.enddate>cast(getdate() as date)order by p2.startdate desc";
            SqlParameter[] pms ={
                                    new SqlParameter("@id",SqlDbType.Int){Value=id},                                                                    
                                };
            return dbSession.ExecuteQuery<Sql_PackageModel2>(sql, pms);
        } 
        #endregion
        #region 6个最爽礼包
        /// <summary>
        /// 6个最爽礼包
        /// </summary>
        /// <returns></returns>
        public List<PackageViewModel> NewestCoolPackage()
        {
          
            string sql = "select top(6) p.id,g.SmallImg,g.DescImg,c.systemname,p.gamename,p.GiftName from Package p inner join game g on p.gamename=g.name inner join company c on c.id=p.companyid where p.state in (1,3) and c.state=1and p.rec_index=1 order by p.rec_index_time desc";
          return   dbSession.ExecuteQuery<PackageViewModel>(sql);
        }
        
        #endregion
        //领号
        public IList<PackageCodeViewModel> GetPackageNumber(int packageId, int userId)
        {
            return dbSession.PackageDal.GetPackageNumber(packageId, userId);
        }
        public IList<Sql_PackageDetail> GetPackageNumber(int packageId, int userId, out Sql_PackageCode code)
        {
            return dbSession.PackageDal.GetPackageNumber(packageId, userId,out code);
        }
        /// <summary>
        /// 热游礼包
        /// </summary>
        /// <returns></returns>
        public IList<Sql_PackageModel> PackageHot()
        {
            return dbSession.PackageDal.PackageHot();
        }
        public IList<PackageViewModel> PackageRecommend(int top, int type)
        {
            return dbSession.PackageDal.PackageRecommend(top,type);
        }
        public int PackageSingleCheck(int id, string state, string reason, string currentAdmin)
        {
            return dbSession.PackageDal.PackageSingleCheck(id, state, reason, currentAdmin);
        }


    
    }
}
