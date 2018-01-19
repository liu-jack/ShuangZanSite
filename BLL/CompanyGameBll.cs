using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class CompanyGameBll : BaseBll<CompanyGame>, ICompanyGameBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.CompanyGameDal;
        }
    
    public IQueryable<CompanyGame> LoadCpyGame(Models.Params.CpyGameParam  param)
    {
        var temp = dbSession.CompanyGameDal.LoadEntities(c => c.CompanyId == param.CompanyId);
        if (!string.IsNullOrEmpty(param.Type))
        {
            temp = temp.Where(t => t.Type.Contains(param.Type));
        }
        if (!string.IsNullOrEmpty(param.GameName))
        {
            temp = temp.Where(t => t.GameName.Contains(param.GameName)); 
        }
        param.Total = temp.Count();      
        return temp.OrderByDescending(d => d.InTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
    }
   }
}
