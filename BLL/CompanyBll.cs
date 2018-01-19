using IBLL;
using Models;
using Models.Enum;
using Models.MapModel;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class CompanyBll : BaseBll<Company>, ICompanyBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.CompanyDal;
        }
        #region 批量删除
        public int DeleteCompanys(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var cpy = dbSession.CompanyDal.LoadEntities(c => c.Id == id).FirstOrDefault();
                if (cpy != null)
                {
                    cpy.DelFlag = (short)Models.Enum.DelFlagEnum.Deleted;
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 一键清空设置的状态
        public int ClearSetState(IList<int> ids)
        {
            foreach (var id in ids)
            {
                var cpy = dbSession.CompanyDal.LoadEntities(c => c.Id == id).FirstOrDefault();
                if (cpy != null)
                {
                    cpy.Rec_Forum_Index = "0";
                    cpy.Rec_Index = "0";
                    cpy.Rec_Forum_Index_Time = DateTime.Now;
                    cpy.Rec_Index_Time = DateTime.Now;
                    dbSession.CompanyDal.Update(cpy);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 首页厂商推荐
        /// <summary>
        /// 首页厂商推荐
        /// </summary>
        /// <returns></returns>
        public List<CompanyViewModel> RecCompany()
        {
            var cpy = dbSession.CompanyDal.LoadEntities(c => c.State == 1 && c.Rec_Index == "1").OrderByDescending(c => c.Rec_Index_Time).Select(c => new CompanyViewModel()
            {
                Id = c.Id,
                Logo = c.Logo,
                Systemname = c.SystemName
            }).AsNoTracking().ToList();
            return cpy;
        }
        #endregion
        #region 频道首页
        /// <summary>
        /// 频道首页
        /// </summary>
        /// <returns></returns>
        public List<CompanyViewModel> ForumRecCompany()
        {
            var cpy = dbSession.CompanyDal.LoadEntities(c => c.State == 1 && c.Rec_Forum_Index == "1").OrderByDescending(c => c.Rec_Forum_Index_Time).Select(c => new CompanyViewModel()
            {
                Id = c.Id,
                Logo = c.Logo,
                Systemname = c.SystemName
            }).AsNoTracking().Take(8).ToList();

            return cpy;
        }
        #endregion
        #region 当前厂商的详情
        /// <summary>
        /// 当前厂商的详情
        /// </summary>
        /// <param name="id">当前厂商的Id</param>
        /// <returns></returns>
        public Company GetCpyDetails(int id, out int gameCount)
        {
            var cpy = dbSession.CompanyDal.LoadEntities(c => c.Id == id).FirstOrDefault();
            gameCount = dbSession.CompanyGameDal.LoadEntities(cg => cg.CompanyId == id).Count();
            return cpy;
        }
        #endregion
        #region 当前厂商对应的相关新游、热游、手游的新闻
        /// <summary>
        /// 当前厂商对应的相关新游、热游、手游的新闻
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<NewsViewModel> CurrentCpyNews(int companyId)
        {
            var data = dbSession.NewsDal.LoadEntities(n => n.CompanyId == companyId && n.State == "1").Where(d => d.Type == "1" || d.Type == "2" || d.Type == "3").Select(n => new NewsViewModel()
            {
                Id = n.Id,
                EditTitle = n.EditTitle,
                InTime = n.InTime,
                Memo = n.Memo,
                Title=n.Title
            }).OrderByDescending(n => n.InTime).Take(3).AsNoTracking().ToList();
            return data;
        }
        #endregion
      
        #region 前台厂商相关礼包
        public List<Sql_CpyPackageModel> CurrnetCpyPackage(int companyId)
        {
            #region EF查询
            //var package = dbSession.PackageDal.LoadEntities(p => p.State == "1" && p.CompanyId == companyId).Select(p => new
            //{
            //    p.GameName,
            //    p.GiftName,
            //    p.Id,
            //    p.InTime,
            //    p.Type,
            //});
            //var game = dbSession.GameDal.LoadEntities(g => g.State == "1").Select(g => new { g.LoGo, g.Name });
            //var data = (from p in package
            //            join g in game on p.GameName equals g.Name
            //            select (new PackageViewModel()
            //            {
            //                GiftName = p.GiftName,
            //                GameName = p.GameName,
            //                Logo = g.LoGo,
            //                Id = p.Id,
            //                InTime = p.InTime,
            //                Type = p.Type
            //            })).OrderByDescending(d => d.InTime).Take(5).ToList(); 
            #endregion
            string sql = "select top(5) p.id,gamename,p.GiftName,g.descimg  from Package p inner join game g on p.gamename=g.name where companyid=@id and p.State in (1,3) and p.startdate<getdate() and p.enddate>cast(getdate() as date)";
            SqlParameter[] pms = { 
                                 new SqlParameter("id",SqlDbType.Int){Value=companyId}
                                 };
            return dbSession.ExecuteQuery <Sql_CpyPackageModel>(sql,pms).ToList();
        }
        #endregion
        #region 该厂商下的联运、研发、独代游戏
        /// <summary>
        /// 该厂商下的联运、研发、独代游戏
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IQueryable<GameViewModel> currentCpyGame(GameParam param)

        {
            
            var game = dbSession.GameDal.LoadEntities(g => g.Id > 0);
          
            var cpyGame = dbSession.CompanyGameDal.LoadEntities(c => true);
            var data = (from c in cpyGame
                        join g in game on c.GameName equals g.Name
                        where c.Type == param.Type && c.CompanyId == param.CompanyId
                        select new GameViewModel()
                        {
                            GameName = c.GameName,
                            Play = g.Play,
                            Theme = g.Theme,
                            Type = g.Type,
                            Company = g.Company,
                            Url = g.Url,
                            Id = g.Id,
                            SmallImg = g.SmallImg,
                              Msg = g.Msg,
                            InTime = c.InTime,
                            DescImg=g.DescImg
                        });
            param.Total = data.Count();
            return data.AsNoTracking().OrderByDescending(c => c.InTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable(); 
           
         
          
        }
       
        #endregion
        public IList<CompanyViewModel> CompanyList(int pageIndex, int pageSize, out int total, string key, string py)
        {

            pageIndex = (pageIndex - 1) * pageSize;
            key = key == null ? null : key;
            py = py == null ? null : py;
            return dbSession.CompanyDal.CompanyList(pageIndex, pageSize, out total, key, py);
        }
    }
}
