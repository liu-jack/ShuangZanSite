using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
namespace BLL
{
    public class AdvertisementBll : BaseBll<Advertisement>, IAdvertisementBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.AdvertisementDal;
        }
        #region 广告管理
        /// <summary>
        /// 广告管理
        /// </summary>
        /// <param name="param">分页及查询条件等参数</param>
        /// <returns></returns>
        public IQueryable<Advertisement> LoadAdvertisementData(Models.Params.AdvertParam param)
        {
            var temp = dbSession.AdvertisementDal.LoadEntities(a => true);
            //.Select(a => new { a.Id,a.Title,a.Type,a.Url,a.State,a.Path,a.StartTime,a.EndTime,a.InTime});
            if (!string.IsNullOrEmpty(param.Type))
            {
                temp = temp.Where(t => t.Type.Contains(param.Type));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(t => t.InTime).Take(param.PageSize).Skip(param.PageSize * (param.PageIndex - 1)).AsQueryable();
        } 
        #endregion
        public List<Advertisement> GetAllTypeAdvert(string type,int take)
        {        
            //0：状态正常
            var data = dbSession.AdvertisementDal.LoadEntities(a => a.Type == type && a.State == 0 && a.StartTime <= DateTime.Now && DateTime.Now <= a.EndTime)
                            .OrderByDescending(d => d.InTime)
                            .Take(take).AsNoTracking().ToList();
            return data;

        }
    }
}
