using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;

namespace ShuangZan.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : SpringMvcApplication//System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          // Application["total"] = 0;
            HttpContext.Current.Application["onLine"] = 0;
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //让log4net配置节点起作用
            log4net.Config.XmlConfigurator.Configure();
        }      
        public void Application_Error(object send, EventArgs e)
        {            
            //记录日志
         Exception ex = Server.GetLastError();
         string errorMsg = ex.ToString();
         //   //日志可能写到多个地方去。那么可能产生  变化点。
         Common.LogHelper.WriteLog(errorMsg);

         //   //转到错误页或者跳转。
        //  Response.Redirect("/Home/Error");
        }
        public void Session_Start(object sender, EventArgs e)
        {
          
           Application.Lock();                                        //锁定Application  
      
         Application["onLine"] = (int)Application["onLine"] + 1;    //在线人数加1  
         Application.UnLock();
        }
        protected void Session_End(object sender, EventArgs e)
        {
           Application.Lock();                                          //锁定Application  
           Application["onLine"] = (int)Application["onLine"] - 1;      //总访问数量不变，在线人数减1  
           Application.UnLock();                                        //解除锁定  
        }  
    }
}