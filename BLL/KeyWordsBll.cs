using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class KeyWordsBll : BaseBll<KeyWords>, IKeyWordsBll
       
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.KeyWordsDal;
        }
        public IQueryable<KeyWords> LoadKeyWords(Models.Params.GameKeyWords param)
        {
            var temp = dbSession.KeyWordsDal.LoadEntities(g => g.Id > 0);

            if (!string.IsNullOrEmpty(param.KeyWords))
            {
                temp = temp.Where(g => g.Key.Contains(param.KeyWords));
            }
            if (!string.IsNullOrEmpty(param.IsLibrary))
            {
                temp = temp.Where(g => g.IsLibrary.Contains(param.IsLibrary));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(d=>d.LibraryTime)
                       .Skip(param.PageSize * (param.PageIndex - 1))
                       .Take(param.PageSize).AsQueryable();
        }

       
    }
}
