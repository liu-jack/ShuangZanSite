using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace WxPayAPI
{
    public class WxPayApi
    {      
        #region 根据当前系统时间加随机序列来生成订单号
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ran.Next(10));
        } 
        #endregion     
        #region 生成随机串，随机串包含字母或数字
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        } 
        #endregion
    }
}