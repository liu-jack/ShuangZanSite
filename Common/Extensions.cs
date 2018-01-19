using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Common
{
    public static class Extensions
    {
        public static string getPostData(this HttpRequest req)
        {
            byte[] array = new byte[req.InputStream.Length];
            req.InputStream.Read(array, 0, array.Length);
            return req.ContentEncoding.GetString(array);
        }
        /// <summary>
        /// 转出md5字符串
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string Md5(this string str)
        {
            bool flag = string.IsNullOrEmpty(str);
            string result;
            if (flag)
            {              
                result = "";
            }
            else
            {
                result = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
            }
            return result;
        }
        #region 获取POST数据返回值
        public static string Post(this string data, string url)
        {
            try
            {
                HttpWebRequest reqHttpWeb = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(data);

                reqHttpWeb.Method = "POST";
                reqHttpWeb.ContentType = "application/x-www-form-urlencoded";
                reqHttpWeb.ContentLength = requestBytes.Length;
                reqHttpWeb.ServicePoint.Expect100Continue = false;
                reqHttpWeb.Timeout = 5000;

                using (Stream requestStream = reqHttpWeb.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    using (HttpWebResponse resHttpWeb = (HttpWebResponse)reqHttpWeb.GetResponse())
                    {
                        //using (StreamReader sReader = new StreamReader(resHttpWeb.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312")))
                        using (StreamReader sReader = new StreamReader(resHttpWeb.GetResponseStream(), System.Text.Encoding.UTF8))
                        {
                            var bytes = default(byte[]);
                            using (var memstream = new MemoryStream())
                            {
                                sReader.BaseStream.CopyTo(memstream);
                                bytes = memstream.ToArray();
                            }
                            if (bytes.Length <= 0)
                                return "";
                            else
                                return Encoding.UTF8.GetString(bytes); //sReader.ReadToEnd(); 
                        }
                    }
                }
            }
            catch (Exception e) { LogHelper.WriteLog(e.ToString()); return ""; }
        }
        #endregion
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T current in source)
            {
                act(current);
            }
            return source;
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static int toString(this object input, int dvalue)
        {
            bool flag = input == null;
            int result;
            if (flag)
            {
                result = dvalue;
            }
            else
            {
                int num = 0;
                result = (int.TryParse(Convert.ToString(input), out num) ? num : dvalue);
            }
            return result;
        }
    }
}
