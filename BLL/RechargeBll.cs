using IBLL;
using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data.Objects.SqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
namespace BLL
{
    public class RechargeBll : BaseBll<Recharge>, IRechargeBll
    {                  
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.RechargeDal;
        }
        public IQueryable<Recharge> LoadRechargeTotal(RechargeParam param)
        {
            var temp = dbSession.RechargeDal.LoadEntities(r=>r.CompanyId==param.CompanyId);
            if (!string.IsNullOrEmpty(param.comboName))
            {
                temp = temp.Where(t => t.ComboName.Contains(param.comboName));
            }
            if (param.InTime.HasValue)
            {
                temp = temp.Where(t =>DbFunctions.DiffDays(t.InTime,param.InTime)==0);
               // temp = temp.Where(t => SqlFunctions.DateName(param.InTime,t.InTime).ToString().Contains(param.InTime));
            }
            param.Total = temp.Count();
            return temp.OrderByDescending(d => d.InTime).Skip(param.PageSize*(param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        /// <summary>
        /// 置顶开服剩余条数
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int TopUsedNum(int companyId)
        {
            string sql = "select ISNULL(sum(num),0)- ISNULL( sum(used),0) as TopUsedNum  from Recharge  where CompanyId=@companyid 	and (ComboName='K1套餐'or ComboName='K2套餐'or ComboName='K3套餐'or ComboName='K4套餐' or ComboName='K5套餐'or ComboName='K6套餐'or ComboName='K7套餐'or ComboName='K8套餐'or ComboName='K9套餐'or ComboName='K10套餐'or ComboName='K11套餐'or ComboName='K12套餐')";
            SqlParameter[] pms = { 
                              new SqlParameter("@companyid",SqlDbType.Int){Value=companyId},                         
                          };
            return dbSession.ExecuteQuery<int>(sql,pms).SingleOrDefault();
        }
        /// <summary>
        /// 全天开服剩余条数
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int allDayUsedNum(int companyId)
        {
            string sql = "select ISNULL( sum(num),0)- ISNULL( sum(used),0) as allDayUsedNum from recharge where CompanyId=@companyid and	ComboName='全天置顶'";
            SqlParameter[] pms = { 
                              new SqlParameter("@companyid",SqlDbType.Int){Value=companyId},                         
                          };
            return dbSession.ExecuteQuery<int>(sql, pms).SingleOrDefault();
        }
    }
}
