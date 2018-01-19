using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface ISeeNewsBll : IBaseBll<SeeNews>
    {
        IQueryable<SeeNewsViewModel> LoadSeeNews(RecSeeNews param);
        /// <summary>
        ///  查询看新闻对应类型的数据
     /// </summary>
     /// <param name="type">类型参数</param>
     /// <param name="take">要查询的条数</param>
     /// <returns></returns>
        List<SeeNews> GetSeeNews(string type, int take);
    
        /// <summary>
        /// 得到精彩的图文
        /// </summary>
        /// <returns></returns>
        //List<SeeNews> GetWonderfulSeeNews();

    }
}
