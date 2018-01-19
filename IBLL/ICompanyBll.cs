using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IBLL
{
    public interface ICompanyBll : IBaseBll<Company>
    {
        //IQueryable<Company> LoadPageCompany(QueryParam param);
        int DeleteCompanys(IList<int> ids);
        int ClearSetState(IList<int> ids);
        /// <summary>
        /// 首页推荐厂商
        /// </summary>
        /// <returns></returns>
        List<CompanyViewModel> RecCompany();
        /// <summary>
        /// 板块列表页推荐
        /// </summary>
        /// <returns></returns>
        List<CompanyViewModel> ForumRecCompany();
        /// <summary>
        /// 前台厂商详情
        /// </summary>
        /// <param name="id">当前厂商的id</param>
        /// <param name="GameCount">总数游戏数量</param>
        /// <returns></returns>
        Company GetCpyDetails(int id,out int gameCount);
       /// <summary>
       /// 前台该厂商的热游、手游、产业新闻
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>
        List<NewsViewModel> CurrentCpyNews(int companyId);
       /// <summary>
       /// 前台该厂商的开服信息
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>

      List<Sql_CpyPackageModel> CurrnetCpyPackage(int companyId);
        /// <summary>
        /// 该厂商下的联运、研发、独代游戏
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
      IQueryable<GameViewModel> currentCpyGame(GameParam param);
    
     /// <summary>
     /// 厂商列表大全
     /// </summary>
     /// <param name="pageIndex"></param>
     /// <param name="pageSize"></param>
     /// <param name="total"></param>
     /// <param name="key"></param>
     /// <param name="py"></param>
     /// <returns></returns>
     IList<CompanyViewModel> CompanyList(int pageIndex, int pageSize, out int total, string key, string py);
     
    }
}
