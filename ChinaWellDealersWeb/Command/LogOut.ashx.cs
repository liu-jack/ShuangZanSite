using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
 
namespace ShuangZan.Web.Command
{
    /// <summary>
    /// LogOut 的摘要说明
    /// </summary>
   
    public class LogOut : IHttpHandler
    {   
        public void ProcessRequest(HttpContext context)
        {
            
          
            string guid = context.Request["mySessionId"];
            if (!string.IsNullOrEmpty(guid))
            {
                //从分布式缓存拿出来的对象不能进行延迟加载。
                var loginAdmin = Common.CacheHelper.Get(guid) as UserInfo;
                if (loginAdmin != null)
                {
                    //  context.Response.Clear();
                    Common.CacheHelper.Remove(guid, loginAdmin, DateTime.Now.AddDays(-1));
                    context.Response.Redirect("/Login/Index");
                    context.Response.Expires = -1;
                    context.Response.AddHeader("pragma", "no-cache");
                    context.Response.AddHeader("cache-control", "no-cache");
                    context.Response.CacheControl = "no-cache";
                    context.Response.Cache.SetNoStore();
                }
            }       
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}