using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShuangZan.Web.Command
{
    /// <summary>
    /// PersonalUserLogOut 的摘要说明
    /// </summary>
    public class PersonalUserLogOut : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string guid = context.Request["userId"];
            if (!string.IsNullOrEmpty(guid))
            {
                //从分布式缓存拿出来的对象不能进行延迟加载。
                var userAdmin = Common.CacheHelper.Get(guid) as PersonalUser;
                if (userAdmin != null)
                {
                    //  context.Response.Clear();
                    Common.CacheHelper.Remove(guid, userAdmin, DateTime.Now.AddDays(-1));
                    context.Response.Redirect("/Login/PersonalLogin");
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