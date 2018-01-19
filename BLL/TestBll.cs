using IBLL;
using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace BLL
{
    public class TestBll : BaseBll<Tests>,ITestBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.TestDal;
            
        }
        #region 后台开测数据模糊查询               
        public IQueryable<Tests> LoadPageTests(TestParam  param)
        {
             var temp=dbSession.TestDal.LoadEntities(t => t.Id > 0);
            if (!string.IsNullOrEmpty(param.gameName))
            {
                temp = temp.Where(t => t.GameName.Contains(param.gameName));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(d=>d.InTime)
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize).AsQueryable();
        }
        #endregion
        #region 开测数量           
        public int TodayTestCount()
        {
            string sql = "select count(*)from tests t inner join game g on t.gamename=g.name where cast(starttime as date)=cast(getdate() as date)";
         return dbSession.ExecuteQuery<int>(sql).SingleOrDefault();
        }
        
        public int WillTestCount()
        {
            string sql = "select count(*)from tests t inner join game g on t.gamename=g.name where cast(starttime as date)>cast(getdate() as date)";
            return dbSession.ExecuteQuery<int>(sql).SingleOrDefault();
        }
        public int HistoryTestCount()
        {
            string sql = "select count(*) from tests t inner join game g on t.gamename=g.name where datediff(d,starttime,getdate())<30 and cast(starttime as date)<cast(getdate() as date)";
            return dbSession.ExecuteQuery<int>(sql).SingleOrDefault();
        }
        #endregion
        #region 今日要开测的信息 
        public List<Models.MapModel.TestViewModel> TodayTestInfo()
        {
            string sql = "select g.id,starttime,t.gamename,g.SmallImg,t.state,g.company,g.Type,g.Play,t.package as packageUrl,t.url from tests t inner join game g on t.gamename=g.name where cast(starttime as date)=cast(getdate() as date) order by starttime";
            return dbSession.ExecuteQuery<TestViewModel>(sql);
        }
        #endregion
        #region 将要开测的信息
        /// <summary>
        /// 将要开测的信息
        /// </summary>
        /// <returns></returns>
        public List<TestViewModel> WillTestInfo()
        {
            string sql = "select g.id,starttime,t.gamename,g.SmallImg,t.state,g.company,g.Type,g.Play,t.package as packageUrl,t.url from tests t inner join game g on t.gamename=g.name where cast(starttime as date)>cast(getdate() as date) order by starttime";
            return dbSession.ExecuteQuery<TestViewModel>(sql);         
        } 
        #endregion       
        #region 曾经的开服数据
        public List<TestViewModel> HistoryTestInfo()
        {
            string sql = "select g.id,starttime,t.gamename,g.SmallImg,t.state,g.company,g.Type,g.Play,t.package as packageUrl,t.url  from tests t inner join game g on t.gamename=g.name where datediff(d,starttime,getdate())<30 and cast(starttime as date)<cast(getdate() as date) order by starttime desc";
             return dbSession.ExecuteQuery<TestViewModel>(sql);
        } 
        #endregion
        #region 得到搜索结果
        /// <summary>
        /// 得到搜索结果
        /// </summary>
        /// <param name="key">游戏名和平台名</param>
        /// <returns></returns>
        public IQueryable<TestViewModel> GetSearchTestResult(string key)
        {
            var allTest = dbSession.TestDal.LoadEntities(t => t.Id > 0);
            var allGame = dbSession.GameDal.LoadEntities(g => g.Id > 0);              
            var data = from t in allTest
                        join g in allGame on t.GameName equals g.Name
                        where t.GameName.Contains(key) || g.Company.Contains(key)
                        select (new TestViewModel()
                        {
                            Id = t.Id,
                            GameName = t.GameName,
                            Url = t.Url,
                            PackageUrl = t.Package,
                            StartTime = t.StartTime,
                            State = t.State,
                            Play = g.Play,
                            Theme = g.Theme,
                            Company = g.Company,
                            Type = g.Type,
                            SmallImg = g.SmallImg
                        }); 
            return data.AsNoTracking().OrderByDescending(d=>d.StartTime).AsQueryable();
        }   
        #endregion
        #region 当天+即将开测信息，最多显示10条
        public List<TestViewModel> TestDataTen()
        {
            var allTest = dbSession.TestDal.LoadEntities(t => t.Id > 0);
            //<0:即将开测，==0今天开测
            var data = (from t in allTest
                        where (DbFunctions.DiffDays(t.StartTime, DateTime.Now) <0)||(DbFunctions.DiffDays(t.StartTime, DateTime.Now) == 0)
                        select (new TestViewModel()
                        {
                            Id = t.Id,
                            GameName = t.GameName,
                            Url = t.Url,
                            PackageUrl = t.Package,
                            StartTime = t.StartTime,
                            State = t.State,
                        })).AsNoTracking().Take(10).ToList();

            return data;
        } 
        #endregion
        public IList<TestViewModel> TestDate(string date)
        {
            return dbSession.TestDal.TestDate(date);
        }


     
    }
}
