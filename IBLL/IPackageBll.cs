using Models;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IPackageBll : IBaseBll<Package>
    {
      
        int MoreCheckPachages(IList<int> ids, string checkIsPass, string currentAdmin);
        IQueryable<Package_Card> CpyPackageDetails(int id, Company currentCpy);
        int CpyPackageDelete(int id);
        IQueryable<Package_Card> GerMyPackage(PersonalUser user, string type, int pageIndex, int pageSize, out int count);
       
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IQueryable<PackageViewModel> LoadPackageData(PackageParam param);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckPackageType(string type);
        /// <summary>
        /// 礼包详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
      
       PackageViewModel2 GetPackageDetails(int packageId);
        /// <summary>
        /// 同游戏名的相关礼包
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       List<Sql_PackageModel2> SameGamePackage(int id);
        /// <summary>
        /// 最爽礼包6个
        /// </summary>
        /// <returns></returns>
        List<PackageViewModel> NewestCoolPackage();
        /// <summary>
        /// 礼包领号
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<PackageCodeViewModel> GetPackageNumber(int packageId, int userId);
        IList<Sql_PackageDetail> GetPackageNumber(int packageId, int userId, out Sql_PackageCode code);
        IList<Sql_PackageModel> PackageHot();
        /// <summary>
        /// 礼包首页礼包推荐
        /// </summary>
        /// <param name="top">要查多少条</param>
        /// <param name="type">礼包类型</param>
        /// <returns></returns>
        IList<PackageViewModel> PackageRecommend(int top, int type);
        int PackageSingleCheck(int id, string state, string reason, string currentAdmin);
        
    }
    
}
