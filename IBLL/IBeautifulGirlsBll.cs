using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IBeautifulGirlsBll : IBaseBll<BeautifulGirls>
    {
        IQueryable<GirlsViewModel> LoadGirlSInfo(GirlsParam param);
        bool CheckIsContains(string tag);
        List<BeautifulGirls> GetAllGirlTags();
        IQueryable<BeautifulGirls> GetAllGirlsInfo(string type, string game, int pageIndex, int pageSize);
        List<BeautifulGirls> NewestIndexRecGirls();
        IQueryable<GirlsViewModel> GetGirlDetails(int id);
       
    }
}
