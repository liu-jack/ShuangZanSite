using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Xml.Linq;
using IBLL;
using BLL;
namespace ShuangZan.Web.Command
{
    /// <summary>
    /// WXPay_Notify 的摘要说明
    /// </summary>
    public class WXPay_Notify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Cache-Control", "no-cache");
            try
            {
                string xml = context.Request.getPostData();
                XElement xe = XElement.Parse(xml);
                if (xe.Element("attach") != null && xe.Element("result_code").Value == "SUCCESS" && xe.Element("return_code").Value == "SUCCESS")
                {
                    string[] x = xe.Element("attach").Value.Split(',');
                    /// <param name="trade_no">订单号</param>
                    /// <param name="fee">充值金额人民币（元）</param>
                    /// <param name="memo">爽币条数</param>
                    /// <param name="feetype">微信支付|支付宝支付</param>
                    //当前用户ID,订单号，
                    IUserMessageBll UserMessageBll = new UserMessageBll();
                    decimal rmb =decimal.Parse(xe.Element("total_fee").Value) / 100;
                      UserMessageBll.UserRechargeCoolCoins(x[0], xe.Element("out_trade_no").Value, rmb.ToString(), x[1], "微信支付");
                  
                }
                //注意：同样的通知可能会多次发送给商户系统。商户系统必须能够正确处理重复的通知。
                //特别提醒：商户系统对于支付结果通知的内容一定要做签名验证，防止数据泄漏导致出现“假通知”，造成资金损失。
                context.Response.Write(@"<xml>
                  <return_code><![CDATA[SUCCESS]]></return_code>
                  <return_msg><![CDATA[OK]]></return_msg>
                </xml>");
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("WXPay_Notify出错：" + e.Message);
                context.Response.Write(@"<xml>
                <return_code><![CDATA[FAIL]]></return_code>
            <return_msg><![CDATA[未知原因]]></return_msg>
            </xml>");

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