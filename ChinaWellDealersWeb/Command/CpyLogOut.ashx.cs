using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShuangZan.Web.Command
{
    /// <summary>
    /// CpyLogOut 的摘要说明
    /// </summary>
    public class CpyLogOut : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
        
            string guid = context.Request["cpyUserId"];
            if (!string.IsNullOrEmpty(guid))
            {
                //从分布式缓存拿出来的对象不能进行延迟加载。
                var currentUser = Common.CacheHelper.Get(guid) as Company;
                if (currentUser != null)
                {
                   // context.Response.Clear();
                    Common.CacheHelper.Remove(guid, currentUser, DateTime.Now.AddDays(-1));
                    context.Response.Redirect("/Login/LoginCompany");
                   //context. Response.Buffer = true;                
                   //context. Response.Expires = 0 ;
                   //context. Response.CacheControl = "no-cache " ;
                   //context.Response.Cache.SetNoStore();
                 

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