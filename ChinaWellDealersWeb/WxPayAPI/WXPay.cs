using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Xml.Linq;
using IBLL;
using BLL;
using System.Reflection;
using WxPayAPI;


namespace ShuangZan.Web.WxPayAPI
{
    /// <summary>
    /// WXPay 的摘要说明
    /// </summary>
    public class WXPay
    {
        public WXPay() { }
        public string xml()
        {
            return string.Format(@"<xml>
        <appid>{0}</appid>
        <mch_id>{1}</mch_id>
        <nonce_str>{2}</nonce_str>
        <body>{3}</body>
        <detail>{4}</detail>
        <out_trade_no>{5}</out_trade_no>
        <spbill_create_ip>{6}</spbill_create_ip>
        <total_fee>{7}</total_fee>
        <notify_url>{8}</notify_url>
        <trade_type>{9}</trade_type>
        <attach>{10}</attach>
        <sign>{11}</sign>
        </xml>", this.appid, this.mch_id, this.nonce_str, this.body, this.detail, this.out_trade_no, this.spbill_create_ip, this.total_fee, this.notify_url, this.trade_type, this.attach, this.sign());
        }
        public string pay()
        {
            return this.xml().Post("https://api.mch.weixin.qq.com/pay/unifiedorder");
        }

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get { return "wx25fcf0405e1e4860"; } }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get { return "1393586502"; } }
        /// <summary>
        /// 微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        public string key { get { return "Ftwlyxgs520Whnskjyxgs1314szw2016"; } }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str
        {
            get { return WxPayApi.GenerateOutTradeNo(); }
        }
        public string trade_type
        {
            get { return "NATIVE"; }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign()
        {
            Dictionary<string, string> m = new Dictionary<string, string>();
            try
            {
                Type type = typeof(WXPay);
                object obj = Activator.CreateInstance(type);               
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ForEach(t => m.Add(t.Name, t.GetValue(this, null).ToString()));
                m.Remove("key"); //这个已经内置了
                m.Remove("sign");
                return WXSign(m);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.ToString());
                return "";
            }
        }
        public string WXSign(Dictionary<string, string> m)
        {
            string stra = string.Join("&",
                    m.Where(t => !t.Value.IsNullOrEmpty()).OrderBy(t => t.Key).Select(t => string.Format("{0}={1}", t.Key, t.Value))
                );
            string strb = string.Format("{0}&key={1}", stra, this.key);
            return strb.Md5().ToUpper();
        }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据(商户携带订单的自定义数据)
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip
        {
            get { return WebHelper.RealIP; }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 通知地址,接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
        /// </summary>
        public string notify_url
        {
            get { return "http://www.shuangzan.com/Command/WXPay_Notify.ashx"; }
        }
    }
}