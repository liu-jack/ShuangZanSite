using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface ISeoBll : IBaseBll<Seo>
    {
        /// <summary>
        /// seo推荐
        /// </summary>
        /// <param name="top">要查询多少条</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        List<Seo> GetSeoData(int  top,string type);
    }
}
