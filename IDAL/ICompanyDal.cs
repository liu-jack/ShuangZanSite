using Models;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IDAL
{
    public interface ICompanyDal : IBaseDal<Company>
    {
        /// <summary>
        /// 厂商列表大全
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="key"></param>
        /// <param name="py">拼音</param>
        /// <returns></returns>
        IList<CompanyViewModel> CompanyList(int pageIndex, int pageSize, out int total, string key, string py);
    }
}
