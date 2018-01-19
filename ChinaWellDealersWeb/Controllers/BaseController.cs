using IBLL;
using Models;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ShuangZan.Web.Controllers
{  
    public class BaseController : Controller
    {          
        // GET: /Base/
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {          
                base.OnActionExecuted(filterContext);
              //启用分布式缓存mm模拟session的写法，校验用户是否登录
                string loginguid = Request["mySessionId"];
                string returnUrl = Request.Url.ToString();                             
                if (!string.IsNullOrEmpty(loginguid))
                {
                    //从分布式缓存拿出来的对象不能进行延迟加载。
                    var loginAdmin = Common.CacheHelper.Get(loginguid) as UserInfo;
                    if (loginAdmin != null)
                    {                      
                        //滑动：Session滑动，再分配50分钟
                        Common.CacheHelper.WriteCache(loginguid, loginAdmin, DateTime.Now.AddMinutes(50));
                        //校验用户是否拥有此地址的访问权限
                       // ValidateUserCheck(loginAdmin);
                        return;
                    }
                }             
               filterContext.HttpContext.Response.Redirect("/Login/Index");
             
                                
        }
        #region 校验用户是否拥有此地址的访问权限
        public void ValidateUserCheck(UserInfo user)
        {
            //如果是静态的属性的话，如果想让它有注入的值，那么必须先创建一个实例后，才会注入。
            IApplicationContext ctx = ContextRegistry.GetContext();
            var ActionInfoBll = ctx.GetObject("ActionInfoBll") as IActionInfoBll;
            var UserInfoBll = ctx.GetObject("UserInfoBll") as IUserInfoBll;
            var userInfo = UserInfoBll.LoadEntities(u => u.Id == user.Id).FirstOrDefault();

            //给我们自己开一个后门。
            //if (userInfo.UName == "admin")
            //{
            //    return;
            //}
            //校验当前用户是否拥有访问此地址的权限。
            //拿到当前请求的action对应的 权限表里的数据
            //拿到当前请求的url地址
            string strPath = Request.Url.AbsolutePath.ToLower();// "/Home/Index"
            string httpMethod = Request.HttpMethod.ToLower();

            var tempAction = ActionInfoBll.LoadEntities(a => a.Url.ToLower() == strPath
                                             && a.HttpMethod.ToLower() == httpMethod)
                                            .FirstOrDefault();
            if (tempAction == null)
            {
                Response.Redirect("/Home/Error");
                return;
            }
            //走1号线：校验用户的特殊权限表中是否允许此用户访问此地址
            var temp = (from r in userInfo.R_UserInfo_ActionInfo
                        where r.ActionInfoId == tempAction.Id
                        select r).FirstOrDefault();
            if (temp != null)
            {
                if (temp.IsPass)
                {
                    return;
                }
                else
                {
                    Response.Redirect("/Home/Error");
                    return;
                }
            }
            //走2 号线：校验用户对ying角色关联的所有的权限
            //data：放当前用户所有的可以访问的权限了。
            var data = from r in userInfo.RoleInfo
                       from a in r.ActionInfo
                       select a;
            var result = (from a in data
                          where a.Id == tempAction.Id
                          select a.Id).FirstOrDefault();

            if (result <= 0)
            {
                Response.Redirect("/Home/Error");
            }

        } 
        #endregion

    }
}
