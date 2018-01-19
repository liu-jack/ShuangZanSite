using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IAdvertisementBll : IBaseBll<Advertisement>
    {
        IQueryable<Advertisement> LoadAdvertisementData(AdvertParam param);
        List<Advertisement> GetAllTypeAdvert(string type,int take);
    }
}
