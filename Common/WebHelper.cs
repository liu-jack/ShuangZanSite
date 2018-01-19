using Models.MapModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
    public class WebHelper
    {
        public static string RealIP
        {
            get
            {
                string result;
                try
                {
                    HttpRequest request = HttpContext.Current.Request;
                    result = ((request.ServerVariables["HTTP_VIA"] != null) ? request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(new char[]
			{
				','
			})[0].Trim() : request.UserHostAddress);
                }
                catch (NullReferenceException)
                {
                    result = "";
                }
                return result;
            }
        }
        #region 处理传递过来的时间差的值		
        /// <summary>
        /// 处理传递过来的时间差的值
        /// </summary>      
        /// <param name="list">传递过来的集合</param>
        /// <returns>返回新的集合</returns>
        public static List<Reply> ToReplyStrTimeSpan(List<Reply> list)
        {
            // TimeSpan ts = DateTime.Now - bookComment.CreateDateTime;//计算评论时间
            List<Reply> newList = new List<Reply>();
            Reply model = null;
            foreach (var item in list)
            {
                model = new Reply();
                model.SelfId = item.SelfId;
                model.ReplyId = item.ReplyId;
                model.ReplyContent = item.ReplyContent;
                model.ReplyCity = item.ReplyCity;
                model.ReplyName = item.ReplyName;
                model.ReplyUserImg = item.ReplyUserImg;//用户头像
                TimeSpan ts = (TimeSpan)(DateTime.Now - item.ReplyInTime);
                model.ReplyStrTime = WebHelper.GetTimeSpan(ts);
                model.ReplyTip = item.ReplyTip;
                model.ReplyStamp = item.ReplyStamp;
                newList.Add(model);
            }
            return newList;
        } 
	
        public static List<Publish> ToListTimeSpan(List<Publish> list)
        {
            // TimeSpan ts = DateTime.Now - bookComment.CreateDateTime;//计算评论时间
            List<Publish> newList = new List<Publish>();
            Publish model = null;
            foreach (var item in list)
            {
                model = new Publish();
                model.Id = item.Id;
              
                model.Msg = item.Msg;
                model.City = item.City;
                model.UserName = item.UserName;
                model.UserNameImg = item.UserNameImg;//用户头像
                TimeSpan ts = (TimeSpan)(DateTime.Now - item.InTime);
                model.StrTime = WebHelper.GetTimeSpan(ts);
                model.Tip = item.Tip;
                model.Stamp = item.Stamp;
                newList.Add(model);
            }
            return newList;
        }
        #endregion
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static String GetStreamMd5(Stream stream)
        {
            string strResult = "";
            string strHashData = "";
            var oMd5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] arrbytHashValue = oMd5Hasher.ComputeHash(stream);
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;
            return strResult;
        }

        public static string[] GetHtmlImageUrlList(string strMsgImg)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(strMsgImg);
            int i = 0;
            string[] strlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                strlList[i++] = match.Groups["imgUrl"].Value;
            return strlList;
        }
       
        public static string PickupImgUrl(string html)
        {

            //<img[\s]+src[\s]*=[\s]*((['""](?<src>[^'""]*)[\'""])|(?<src>[^\s]*))         
            Regex r = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<src>[^\s\t\r\n""'<>]*)[^<>]*?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            Match m = r.Match(html);
            string img = "";
            if (m.Success)
            {
                img = m.Groups["src"].Value;
            }
            return img;
        }

        /// <summary>
        /// 对时间差进行处理--3天5小时40分钟---》3*24+5+40/60
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string GetTimeSpan(TimeSpan ts)
        {
            if (ts.TotalDays >= 365)
            {
                return Math.Floor(ts.TotalDays / 365) + "年前";
            }
            else if (ts.TotalDays >= 30)
            {
                return Math.Floor(ts.TotalDays / 30) + "月前";
            }
            else if (ts.TotalHours >= 24)
            {
                return Math.Floor(ts.TotalDays) + "天前";
            }
            else if (ts.TotalHours >= 1)
            {
                return Math.Floor(ts.TotalHours) + "小时前";
            }
            else if (ts.TotalMinutes >= 1)
            {
                return Math.Floor(ts.TotalMinutes) + "分钟前";
            }
            else
            {
                return "刚刚";
            }
        }
       
    }
}
