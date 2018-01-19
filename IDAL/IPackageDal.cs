using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
   public interface IPackageDal: IBaseDal<Package>
    {
       /// <summary>
       /// 礼包领号
       /// </summary>
       /// <param name="packageId"></param>
       /// <param name="userId"></param>
       /// <returns></returns>
       IList<PackageCodeViewModel> GetPackageNumber(int packageId, int userId);
       IList<Sql_PackageDetail> GetPackageNumber(int packageId, int userId, out Sql_PackageCode code);
       /// <summary>
       /// 热游礼包
       /// </summary>
       /// <returns></returns>
       IList<Sql_PackageModel> PackageHot();
       /// <summary>
       /// 礼包首页礼包推荐
       /// </summary>
       /// <param name="top"></param>
       /// <param name="type"></param>
       /// <returns></returns>
       IList<PackageViewModel> PackageRecommend(int top, int type);
       int PackageSingleCheck(int id,string state,string reason,string currentAdmin);
    }
}
