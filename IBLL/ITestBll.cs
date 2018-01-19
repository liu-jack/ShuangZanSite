using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IBLL
{
   public interface ITestBll:IBaseBll<Tests>
    {
       IQueryable<Tests> LoadPageTests(TestParam param);

       int TodayTestCount();
       int WillTestCount();
       int HistoryTestCount();
       /// <summary>
       /// 今日开服的数据
       /// </summary>
       /// <returns></returns>
       List<TestViewModel> TodayTestInfo();
       /// <summary>
       /// 将要开服的数据（明天）
       /// </summary>
       /// <returns></returns>
       List<TestViewModel> WillTestInfo();
       List<TestViewModel> HistoryTestInfo();
       /// <summary>
       /// 开测表搜过结果
       /// </summary>
       /// <param name="key">游戏名  和平台名</param>
       /// <returns></returns>
       IQueryable<TestViewModel> GetSearchTestResult(string key);
       List<TestViewModel> TestDataTen();
       /// <summary>
       /// 开测日历历史数据
       /// </summary>
       /// <param name="date"></param>
       /// <returns></returns>
       IList<TestViewModel> TestDate(string date);
      
    }
}
