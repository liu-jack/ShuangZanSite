using System.Web;
using System.Web.Mvc;
using ShuangZan.Web.Controllers;
namespace ShuangZan.Web
{
    public class FilterConfig
    {
        //这个方法是用于注册全局过滤器（在Global中被调用）
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginCheckFilterAttribute() { IsCheck = true });
        }
    }
}