using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IHomePageBll : IBaseBll<HomePage>
    {
        /// <summary>
        /// 网站首页管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IQueryable<HomePageViewModel> GetAllHomePageData(HomePageParam param);
        List<HomePage> GetAllTypeHomePage(string type,int take);
    
      
    }
}
