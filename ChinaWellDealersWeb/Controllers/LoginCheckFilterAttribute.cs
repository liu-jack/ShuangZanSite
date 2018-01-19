using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShuangZan.Web.Controllers
{
    //--------MVC下可以自定义特性类为控制器或控制器中的Action打上[特性]，这里只需要ActionFilter过滤器（Action方法执行前后执行），
    //MVC提供了IActionFilter接口。（为了方便我们可以用微软提供好的ActionFilterAttribute类，他是筛选器特性的基类，也是一个抽象类，其实这个抽象类实现了IActionFilter和IResultFilter）
    /// <summary>
    /// 用户玩家验证
    /// </summary>
    public class LoginCheckFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public bool IsCheck { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (IsCheck)
            {
                string userid= filterContext.HttpContext.Request["userId"];
                if (!string.IsNullOrEmpty(userid))
                {
                    //从分布式缓存拿出来的对象不能进行延迟加载。
                    var personalUser = Common.CacheHelper.Get(userid) as PersonalUser;
                    if (personalUser != null)
                    {
                       // CurrentAdmin = loginAdmin;
                        //滑动：Session滑动，再分配50分钟
                        Common.CacheHelper.WriteCache(userid, personalUser, DateTime.Now.AddMinutes(150));                       
                        return;
                    }
                }
                filterContext.HttpContext.Response.Redirect("/Login/PersonalLogin");                   
            }
        }
    }
}