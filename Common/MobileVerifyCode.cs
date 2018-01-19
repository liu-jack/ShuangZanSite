using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Common
{
  public  static  class MobileVerifyCode
    {

      /// <summary>
      /// 发送手机验证码
      /// </summary>
      /// <param name="mobile"></param>
      /// <returns></returns>
      public static string VerificationCode(string mobile)
      {
          if (IsHandset(mobile))
          {
              string code = new Random().Next(111111, 999999).ToString();
              string content = "亲，本次操作的验证码为:" + code + " 切勿泄漏此验证码信息给他人，如非本人操作，请忽略此条信息。【爽赞网】";
              //c存入数据库
         
              bool ret = SendFast(mobile, content);

              if (ret)
              {
                  return code;
              }
              else
              {
                  return "发送失败";
              }
          }
          else
          {
              return "手机号码格式不正确";
          }
      }
        /// <summary>
        /// 验证手机号码是否合法
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^1[3|4|5|7|8][0-9]\d{8}$");
        }
        public static bool SendFast(string mobile, string content)
        {
            bool flag = false;
            string url = "http://gw.api.taobao.com/router/rest";//手机短信平台给过来的api
            string con = HttpUtility.UrlEncode(content, Encoding.UTF8);//以utf8解码拿过来的信息内容
            string data = "action=send&userId=117&account=23450194&password=52b5a70bc10ba56ccfe6e50bdb9fa8d4&mobile=" + mobile + "&content=" + con;
            string text = SendVerificationMsg(url, data);
            bool result;
            if (text == "")
            {
                result = false;
            }
            else 
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);
                XmlNode node = xml.ChildNodes[1].ChildNodes[0];
                if (node.InnerText.ToLower() == "success")
                {
                    flag = true;
                }
                else {
                    flag = false;
                }
                result = flag;
            }
            return result;
        }
        public static string SendVerificationMsg(string url,string param)
        {
            string result = string.Empty;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(param);
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "post";
            webRequest.KeepAlive = false;//持久性链接
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent = "";
            webRequest.ContentLength = data.Length;
            webRequest.Timeout = 15000;
            try
            {
                System.IO.Stream stream = webRequest.GetRequestStream();
                stream.Write(data,  0, data.Length);
                stream.Close();

                WebResponse webResponse = webRequest.GetResponse();//返回来自网络的响应
                StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), encoding);
                result = streamReader.ReadToEnd();
                webResponse.Close();          
            }
            catch (Exception)
            {

                return "";
            }

            return result;
        }
    
     
    }
}
