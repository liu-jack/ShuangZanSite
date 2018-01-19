using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ShuangZan.Web.App_Start
{
    public class UrlRewriteModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (s, e) =>
            {
                HttpApplication app = s as HttpApplication;

                #region 重定向域名
                //string strUrl = app.Context.Request.Url.ToString().Trim().ToLower();
                //if (strUrl.Contains("http://www.shuangzan.com/skill_4630.html"))
                //{
                //    app.Context.Response.RedirectPermanent(strUrl.Replace("http://jb51.net", "http://www.jb51.net"));
                //} 
                #endregion
             
               
                Match match;
               var url = app.Context.Request.AppRelativeCurrentExecutionFilePath;
               match = Regex.Match(url.ToLower(), "~/index.html");
               if (match.Success)
               {
                   app.Response.Redirect("/Home/Index");  
               }
              //攻略
                     match = Regex.Match(url, @"~/skill.html");
               if (match.Success)
               {
                   app.Response.Redirect("/Raiders/RaidersIndex");
                 
               }
                
                match = Regex.Match(url, @"~/skill_(\d+).html");
               if (match.Success)
               {
                   app.Response.Redirect("/Raiders/RaidersDetail/" + match.Groups[1].Value);
                  // app.Context.RewritePath(@"~/Raiders/RaidersDetail/"+match.Groups[1].Value);
               }
               //-----------------游戏重定向-----------------
               //games
               match = Regex.Match(url, "~/games.html");
               if (match.Success)
               {
                   app.Response.Redirect("/Game/GameIndex");
               }
               match = Regex.Match(url, @"~/games/detail_(\d+).html");
               if (match.Success)
               {
                   app.Response.Redirect("/Game/GameDetail/" + match.Groups[1].Value);
               }
                //-----------------新闻重定向----------------------
               match = Regex.Match(url, "~/news.html");
               if (match.Success)
               {
                   app.Response.Redirect("/NewsInfo/News");
                  
               }
               match = Regex.Match(url, @"~/ylbg/detail_(\d+).html");
               if (match.Success)
               {
                   app.Response.Redirect("/NewsInfo/FunNewsDetail/" + match.Groups[1].Value);
               }
               string[] news = { @"~/news/cy/(\d+).html", @"~/news/xy/(\d+).html", @"~/news/ry/(\d+).html" };
                for (int i = 0; i < news.Count(); i++)
			    {
                    match = Regex.Match(url, news[i]);
			         if (match.Success)
                       {
                           app.Response.Redirect(@"/NewsInfo/NewsDetail/" + match.Groups[1].Value);
                       }
			    }               
                //----------------------礼包重定向---------------
                ///packages
                match = Regex.Match(url, "~/packages.html");
                if (match.Success)
                {
                    app.Response.Redirect("/Package/PackageIndex");
                }
               string[] package = { @"~/packages/all/detail_(\d+).html", @"~/packages/kf/detail_(\d+).html", @"~/packages/dj/detail_(\d+).html", @"~/packages/jh/detail_(\d+).html" };
               for (int i = 0; i < package.Count(); i++)
               {
                   match = Regex.Match(url, package[i]);
                   if (match.Success)
                   {
                       app.Response.Redirect(@"/Package/PackageDetails/" + match.Groups[1].Value);
                   }
               }
                //淘福利试玩和礼品,厂商详情重定向
                 match = Regex.Match(url, @"~/Amoywel/demo/detail_(\d+).html");
                 if (match.Success)
                 {
                     app.Response.Redirect(@"/Home/DemoDetails/" + match.Groups[1].Value); 
                 }
                 match = Regex.Match(url, @"~/Amoywel/gifts/order/detail_(\d+).html");
                 if (match.Success)
                 {
                     app.Response.Redirect(@"/Gift/GiftDetails/" + match.Groups[1].Value);
                 }
                 match = Regex.Match(url, @"~/csdq/detail_(\d+).html");
                 if (match.Success)
                 {
                     app.Response.Redirect(@"/Home/CpyDetails/" + match.Groups[1].Value);
                 }
                //开服,开测  
                 match = Regex.Match(url, "~/service.html");
                 if (match.Success)
                 {
                     app.Response.Redirect(@"/OPenServices/ServiceIndex");
                 }
                 match = Regex.Match(url.ToLower(), @"~/test.html");
                 if (match.Success)
                 {
                     app.Response.Redirect(@"/Openservices/TestIndex");
                 }
                #region 要重写的伪静态
                //target:/NewsInfo/NewsSearchResult?key=nba             
                //Regex nRg = new Regex(@"~/news/search/{\w}.html");
                //if (nRg.IsMatch(exePath))
                //{
                //    var realPath = nRg.Replace(exePath, @"~/NewsInfo/NewsSearchResult?key=$1");
                //    app.Context.RewritePath(realPath);
                //}
                //Regex regex = new Regex(@"~/news/search/{\d+}/{\d+}/{\w}.html");
                //if (regex.IsMatch(exePath))
                //{
                //    // 当前是伪静态格式
                //    // 翻译成真实路径
                //    var realPath = regex.Replace(exePath, @"~/NewsInfo/NewsSearchResult?pageIndex=$1&pageSize=$2&key=$3");

                //    //Response.Redirect(realPath);
                //    //Server.Transfer(realPath);
                //    // 重写当前请求地址
                //    app.Context.RewritePath(realPath);
                //} 
                #endregion
            };
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}