using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class HomePageBll : BaseBll<HomePage>,IHomePageBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.HomePageDal;
        }
        #region 网站首页
        /// <summary>
        /// 网站首页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<HomePageViewModel> GetAllHomePageData(Models.Params.HomePageParam param)
        {
            var temp = dbSession.HomePageDal.LoadEntities(d => d.Id > 0);
            var query = from t in temp
                        let RecGame = (from r in temp where r.Type == "11" select r.Id).Count()
                        let greyHeadlines = (from g in temp where g.Type == "9" select g.Id).Count()
                        let redHeadlines = (from h in temp where h.Type == "8" select h.Id).Count()
                        let fiveStick = (from r in temp where r.Type == "10" select r.Id).Count()
                        let directSeeding = (from d in temp where d.Type == "5" select d.Id).Count()
                        let mobileGame = (from d in temp where d.Type == "6" select d.Id).Count()
                        let SlideShow = (from d in temp where d.Type == "7" select d.Id).Count()
                        let JoinCompany = (from d in temp where d.Type == "12" select d).Count()
                        let NewestHotGame = (from d in temp where d.Type == "13" select d).Count()
                        select new HomePageViewModel()
                        {
                            Name = t.Name,
                            Type = t.Type,
                            Image = t.Img,
                            Id = t.Id,
                            Url = t.Url,
                            Intime = t.InTime,
                            RecGame = RecGame,
                            RedLight = SlideShow,
                            redHeadlines = redHeadlines,
                            GreyHeadlines = greyHeadlines,
                            FiveStick = fiveStick,
                            DirectSeeding = directSeeding,
                            MobileGame = mobileGame,
                            JoinCompany = JoinCompany,
                            NewestHotGame = NewestHotGame,
                        };
            if (!string.IsNullOrEmpty(param.Title))
            {
                query = query.Where(s => s.Name.Contains(param.Title));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                query = query.Where(s => s.Type.Contains(param.Type));
            }
            param.Total = query.Count();
            return query.OrderByDescending(u => u.Intime)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize).AsQueryable();
        } 
        #endregion
        /// <summary>
        /// 网站首页推荐数据
        /// </summary>
        /// <param name="type">要推荐的类型</param>
        /// <param name="take">要查询的条数</param>
        /// <returns></returns>
       public List<HomePage> GetAllTypeHomePage(string type, int take)
        {
            return dbSession.HomePageDal.LoadEntities(h => h.Type == type).OrderByDescending(d => d.InTime).Take(take).ToList();
        }
     
    }
}
