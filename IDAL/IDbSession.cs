using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Entity;
namespace IDAL
{
    /// <summary>
    /// 干的抽象工厂的活、拿到所有的dal、数据库统一访问入口
    /// </summary>
    /// 
    public partial interface IDbSession
    {
        DbContext Db { get; }   
        int SaveChanges();
        int ExecuteSql(string sql, params SqlParameter[] pars);
        List<T> ExecuteQuery<T>(string sql, params SqlParameter[] pars);
        
        IUserAdressDal UserAdressDal { get; set; }
        IUserInfoDal UserInfoDal { get; set; }
        IRoleInfoDal RoleInfoDal { get; set; }
        IActionInfoDal ActionInfoDal { get; set; }
        IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal { get; set; }
        ICompanyDal CompanyDal { get; set; }
        INewsDal NewsDal { get; set; }
        IPackageDal PackageDal { get; set; }
        IPackageCardDal PackageCardDal { get; set; }
        IRechargeDal RechargeDal { get; set; }
        ILeaveMsgDal LeaveMsgDal { get; set; }
        IPersonalUserDal PersonalUserDal { get; set; }
        IUserMessageDal UserMessageDal { get; set; }
        IOpenServiceDal OpenServiceDal { get; set; }
        IUserRaidersDal UserRaidersDal { get; set; }
        IGameDal GameDal { get; set; }
        IKeyWordsDal KeyWordsDal { get; set; }
        ITestDal TestDal { get; set; }
        IBeautifulGirlsDal BeautifulGirlsDal { get; set; }
        IOrderDal OrderDal { get; set; }
        IFeedbackDal FeedbackDal { get; set; }
        IGiftDal GiftDal { get; set; }
        IGameDemoDal GameDemoDal { get; set; }
        IGameDemoRechargeDal GameDemoRechargeDal { get; set; }
        IGameDemoAccountsDal GameDemoAccountsDal { get; set; }
        IGameDemoRequiresDal GameDemoRequiresDal { get; set; }
        ICompanyGameDal CompanyGameDal { get; set; }
        IRechargedUsedDal RechargedUsedDal { get; set; }
        IAuditLogDal AuditLogDal { get; set; }
        IVerificationCodeDal VerificationCodeDal { get; set; }//存放验证码的表
        IDemoUserDal DemoUserDal { get; set; }
        //PersonalUserSign
        IPersonalUserSignDal PersonalUserSignDal { get; set; }
        IPersonalUserSignDetailDal PersonalUserSignDetailDal { get; set; }
        ISeeNewsDal SeeNewsDal { get; set; }
        IForbiddenListDal ForbiddenListDal { get; set; }
        IHomePageDal HomePageDal { get; set; }
        IBlackListIPDal BlackListIPDal { get; set; }
        IWonderfulTxtImgDal WonderfulTxtImgDal { get; set; }
        //Advertisement
        IAdvertisementDal AdvertisementDal { get; set; }
     
        ISeoDal SeoDal { get; set; }
       
    }
}
