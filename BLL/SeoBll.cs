using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BLL
{
  public class SeoBll:BaseBll<Seo>,ISeoBll
  {
      public override void SetCurrentDal()
      {
          CurrentDal = dbSession.SeoDal;
      }
    /// <summary>
    /// 厂商seo推荐
    /// </summary>
    /// <returns></returns>
      public List<Seo> GetSeoData(int top, string type)
      {
          var data = dbSession.SeoDal.LoadEntities(s => s.Type == type).OrderByDescending(s=>s.InTime).Take(top).ToList();
          return data;
      }
  }
}
