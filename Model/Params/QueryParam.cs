using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Params
{
    public class QueryParam : BaseParam
    {
        //文本框模糊搜素定义的属性
        public string SearchName { get; set; }//管理登录名
        public string  CompanyUName { get; set; }//厂商登录名
        public string SearchSystemName { get; set; }//平台名
        public string SearchCompanyName { get; set; }//公司名
        public string  ComboName { get; set; }//套餐平台名称
       // public DateTime  TimeSlot { get; set; }//充值时间段
        public string  CpyName { get; set; }//厂商明细表关联的厂商平台
        public DateTime StartDate { get; set; }//
        public DateTime EndDate { get; set; }
        public string  Mobile { get; set; }
       
    }
    public class  RecSeeNews:BaseParam
    {
        public string  Title { get; set; }
        public string  Type { get; set; }
    }
    public class GameKeyWords:BaseParam
    {
        public string KeyWords { get; set; }//游戏关键词
        public string IsLibrary { get; set; }//是否入库
    }
    public class GameParam : BaseParam
    {
        public int ?CompanyId { get; set; }
        public string GameName { get; set; }
        public string Alias { get; set; }//游戏别名
        public string  Type { get; set; }
        public string Theme { get; set; }
        public string Play { get; set; }
        public string  Games { get; set; }
        public string  Letter { get; set; }
       
    }
    public class Rec_GameParam : BaseParam
    {
        public string GameName { get; set; }
        public string Alias { get; set; }//游戏别名
        public string Type { get; set; }
        public string Theme { get; set; }
        public string Play { get; set; }
        public string Rec_Hot { get; set; }
    }
    public class TestParam : BaseParam
    {
        public String  gameName { get; set; }      
    }
    public class FeedbackParam : BaseParam
    {
        public string  Key { get; set; }
    }
    public class GiftParam : BaseParam
    {
        public string  GiftName { get; set; }
        public string    State { get; set; }
    }
    public class DemoAccountsParam:BaseParam
    {
        public string  SystemName { get; set; }//运营商
        public string Accoounts { get; set; }//账号
        public string City { get; set; }
    }
    public class LeaveMsgParam : BaseParam {
        public string UserName { get; set; }
        public string  Content { get; set; }
    }
    public class HomePageParam : BaseParam {
        public string Title { get; set; }
        public string Type { get; set; }
    }
    public class PackageParam :BaseParam
    {
        public string  Key { get; set; }
        public string  Type { get; set; }
        public string  Systemname { get; set; }
    }
    public class GirlsParam : BaseParam
    {
        public string  Title { get; set; }
        public string  AddedBy { get; set; }
    }
    public class AdvertParam:BaseParam {
        public string  Type { get; set; }
    }
    public class DemoCutImgParam :BaseParam{
        public string  UName { get; set; }
        public string SystemName { get; set; }
        public string  GameName { get; set; }
        public string  Type { get; set; }
        public string  Account { get; set; }
        public string  State { get; set; }
        public string  RechargeState { get; set; }
   
    }
    public class DemoParam : BaseParam
    {
        public string  GameName { get; set; }
    }
    public class RechargeParam : BaseParam 
    {
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string comboName { get; set; }
        public DateTime? InTime { get; set; }
        public int? CompanyId { get; set; }
    
    }
    public class CpyGameParam : BaseParam
    {
        public int? CompanyId { get; set; }
        public string  Type { get; set; }
        public string GameName { get; set; }
    }
    public class ServiceParam : BaseParam
    {
        public string GameName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
    }
    public class BaseParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
