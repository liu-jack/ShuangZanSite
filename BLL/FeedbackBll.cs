using IBLL;
using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class FeedbackBll : BaseBll<Feedback>, IFeedbackBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.FeedbackDal;
        }

        public IQueryable<Feedback> LoadFeedback(FeedbackParam  param)
        {
            var temp = dbSession.FeedbackDal.LoadEntities(f => true);
            if (!string.IsNullOrEmpty(param.Key))
            {
                temp = temp.Where(t => t.Msg.Contains(param.Key));  
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(u => u.Id)
                       .Skip(param.PageSize * (param.PageIndex - 1))
                       .Take(param.PageSize).AsQueryable();
        }
    }
}
