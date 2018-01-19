using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShuangZan.Web.Controllers
{
    
    public  class CpyBaseController : Controller
    {
        //厂商用户权限验证
        
        // GET: /CpyBase/
        public Company CurrrentUser { get; set; }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            string guid = Request["cpyUserId"];
            if (!string.IsNullOrEmpty(guid))
            {
                //从分布式缓存拿出来的对象不能进行延迟加载。
                var loginAdmin = Common.CacheHelper.Get(guid) as Company;
                if (loginAdmin != null)
                {
                    CurrrentUser = loginAdmin;
                    //滑动：Session滑动，再分配50分钟
                    Common.CacheHelper.WriteCache(guid, loginAdmin, DateTime.Now.AddDays(100));                 
                    return;
                }
            }
            filterContext.HttpContext.Response.Redirect("/Login/LoginCompany");
        }

    }
}
