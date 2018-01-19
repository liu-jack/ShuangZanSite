using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface INewsDal : IBaseDal<News>
    {
        /// <summary>
        /// 娱乐八卦新闻详情推荐
        /// </summary>
        /// <param name="top"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Sql_NewsModel> GetRelatedFunNews(int id,int top, string type);
    }
}
