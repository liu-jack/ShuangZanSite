using Common;
using IBLL;
using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace BLL
{
    public class SeeNewsBll : BaseBll<SeeNews>, ISeeNewsBll
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.SeeNewsDal;
        }
        #region 看新闻 后台数据输出             
        /// <summary>
        /// 看新闻后台数据输出
        /// </summary>
        /// <param name="param">要查询数据的参数</param>
        /// <returns></returns>
        public IQueryable<SeeNewsViewModel> LoadSeeNews(RecSeeNews param)
        {
          
            var temp = dbSession.SeeNewsDal.LoadEntities(s => s.Id > 0);
            var query = from t in temp
                        let redLight = (from d in temp where d.Type == "7" select d.Id).Count()
                        let greyHeadlines = (from d in temp where d.Type == "9" select d.Id).Count()
                        let redHeadlines = (from d in temp where d.Type == "8" select d.Id).Count()
                        let fiveStick = (from d in temp where d.Type == "10" select d.Id).Count()
                        let directSeeding = (from d in temp where d.Type == "5" select d.Id).Count()
                        let mobileGame = (from d in temp where d.Type == "6" select d.Id).Count()
                        let newGame = (from d in temp where d.Type == "1" select d.Id).Count()
                        let hotGame = (from d in temp where d.Type == "2" select d.Id).Count()
                        let industry = (from d in temp where d.Type == "3" select d.Id).Count()
                        select new SeeNewsViewModel() {
                            Title=t.Title,
                            Type=t.Type,
                            Image=t.Image,
                            Id=t.Id,
                            Url=t.Url,
                            Intime=t.Intime,
                            RedLight=redLight,
                            GreyHeadlines=greyHeadlines,
                            redHeadlines=redHeadlines,
                            FiveStick=fiveStick,
                            DirectSeeding=directSeeding,
                            MobileGame=mobileGame,
                            NewGame=newGame,
                            HotGame=hotGame,
                            Industry=industry,                    
                        };          
            if (!string.IsNullOrEmpty(param.Title))
            {
                query = query.Where(s => s.Title.Contains(param.Title));
            }
            if (!string.IsNullOrEmpty(param.Type))
            {
                query = query.Where(s => s.Type==param.Type);
            }
            param.Total = query.Count();
            return query.OrderByDescending(u => u.Intime)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize).AsQueryable();
        }
        #endregion    
     
        #region 查询看新闻对应类型的数据
        /// <summary>
        ///  查询看新闻对应类型的数据
        /// </summary>
        /// <param name="type">类型参数</param>
        /// <param name="take">要查询的条数</param>
        /// <returns></returns>
        public List<SeeNews> GetSeeNews(string type, int take)
        {
            return dbSession.SeeNewsDal.LoadEntities(n => n.Type == type).OrderByDescending(n => n.Intime).Take(take).ToList();
        } 
        #endregion
    }
}

