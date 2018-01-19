using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DAL
{
    /// <summary>
    /// 保证EF的上下文。线程内实例唯一，一次请求一个实例、
    /// </summary>
  public  class DbContextFactory
    {
      /// <summary>
      /// 拿到当前的数据模型的上下文  如果没有就去创建
      /// </summary>
      /// <returns></returns>
      public static DbContext GetCurrentDbContext()
      {
          //线程内实例唯一
          DbContext db = HttpContext.Current.Items["DbContext"] as DbContext;
          if (db == null)
          {
              //  SZEntities
              db = new ShuangZanModelDBContainer();
              HttpContext.Current.Items.Add("DbContext", db);
          }
          return db;
      }
    }
}
