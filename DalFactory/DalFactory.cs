using DAL;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Configuration;
using System.Reflection;
namespace DalFactory
{
    /// <summary>
    /// 工厂模式创建实例
    /// </summary>
  public  class DalFactory
    {
        public static readonly string AssemblyName;
        static DalFactory()//静态构造函数：clr，而且处理多线程并发的问题。
        {
            AssemblyName = System.Configuration.ConfigurationManager.AppSettings["AssemblyName"];
            //<add key="AssemblyName" value="DAL"/>
        }           
        public static IUserInfoDal GetUserInfoDal()
        {
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserInfoDal", true);
            return obj as IUserInfoDal;
        }
        public static IRoleInfoDal GetRoleInfoDal()
        {
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".RoleInfoDal", true);
            return obj as IRoleInfoDal;
        }
        public static IActionInfoDal GetActionInfoDal()
        {
            object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".ActionInfoDal", true);
            return obj as IActionInfoDal;
        }
      public static IR_UserInfo_ActionInfoDal GetR_UserInfo_ActionInfoDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".R_UserInfo_ActionInfoDal", true);
          return obj as IR_UserInfo_ActionInfoDal;
      }
      public static ICompanyDal GetCompanyDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".CompanyDal", true);
          return obj as ICompanyDal;
      }
      public static INewsDal GetNewsDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".NewsDal", true);
          return obj as INewsDal;
      }
      public static IPackageDal GetPackageDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PackageDal", true);
          return obj as IPackageDal;
      }
        //PackageCard
      public static IPackageCardDal GetPackageCardDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PackageCardDal", true);
          return obj as IPackageCardDal;
      }
      public static IRechargeDal GetRechargeDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".RechargeDal", true);
          return obj as IRechargeDal;
      }
        //LeaveMsg
      public static ILeaveMsgDal GetLeaveMsgDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".LeaveMsgDal", true);
          return obj as ILeaveMsgDal;
      }
      public static IPersonalUserDal GetPersonalUserDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PersonalUserDal", true);
          return obj as IPersonalUserDal;
      }
        //UserMessage
      public static IUserMessageDal GetUserMessageDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserMessageDal", true);
          return obj as IUserMessageDal;
      }
        //OpenService  开福类
      public static IOpenServiceDal GetOpenServiceDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".OpenServiceDal", true);
          return obj as IOpenServiceDal;
      }
        //UserRaiders  攻略审核类
      public static IUserRaidersDal GetUserRaidersDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserRaidersDal", true);
          return obj as IUserRaidersDal;
      }
        //Game
      public static IGameDal GetGameDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GameDal", true);
          return obj as IGameDal;
      }
        //KeyWords
      public static IKeyWordsDal GetKeyWordsDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".KeyWordsDal", true);
          return obj as IKeyWordsDal;
      }
        //Tests
      public static ITestDal GetTestDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".TestDal", true);
          return obj as ITestDal;
      }
      public static IBeautifulGirlsDal GetBeautifulGirlsDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".BeautifulGirlsDal", true);
          return obj as IBeautifulGirlsDal;
      }
      public static IOrderDal GetOrderDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".OrderDal", true);
          return obj as IOrderDal;
      }
      public static IFeedbackDal GetFeedbackDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".FeedbackDal", true);
          return obj as IFeedbackDal;
      }
        //Gift
      public static IGiftDal GetGiftDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GiftDal", true);
          return obj as IGiftDal;
      }
        //GameDemo
      public static IGameDemoDal GetGameDemoDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GameDemoDal", true);
          return obj as IGameDemoDal;
      }
        //GameDemoRecharge
      public static IGameDemoRechargeDal GetGameDemoRechargeDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GameDemoRechargeDal", true);
          return obj as IGameDemoRechargeDal;
      }
        //GameDemoAccounts
      public static IGameDemoAccountsDal GetGameDemoAccountsDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GameDemoAccountsDal", true);
          return obj as IGameDemoAccountsDal;

      }
        //GameDemoRequires
      public static IGameDemoRequiresDal GetGameDemoRequiresDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".GameDemoRequiresDal", true);
          return obj as IGameDemoRequiresDal;
      }
        //CompanyGame
      public static ICompanyGameDal GetCompanyGameDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".CompanyGameDal", true);
          return obj as ICompanyGameDal;
      }
        //RechargedUsed
      public static IRechargedUsedDal GetRechargedUsedDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".RechargedUsedDal", true);
          return obj as IRechargedUsedDal;
      }
        //AuditLog
      public static IAuditLogDal GetAuditLogDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".AuditLogDal", true);
          return obj as IAuditLogDal;
      }
        //VerificationCode

      public static IVerificationCodeDal GetVerificationCodeDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".VerificationCodeDal", true);
          return obj as IVerificationCodeDal;
      }
        //DemoUser
      public static IDemoUserDal GetDemoUserDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".DemoUserDal", true);
          return obj as IDemoUserDal;
      }
        //PersonalUserSign
      public static IPersonalUserSignDal GetPersonalUserSignDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PersonalUserSignDal", true);
          return obj as IPersonalUserSignDal;
      }
      public static IPersonalUserSignDetailDal GetPersonalUserSignDetailDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".PersonalUserSignDetailDal", true);
          return obj as IPersonalUserSignDetailDal;
      }
      public static ISeeNewsDal GetSeeNewsDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".SeeNewsDal", true);
          return obj as ISeeNewsDal;
      }
        //ForbiddenList
      public static IForbiddenListDal GetForbiddenListDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".ForbiddenListDal", true);
          return obj as ForbiddenListDal;
      }
        //HomePage
      public static IHomePageDal GetHomePageDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".HomePageDal", true);
          return obj as HomePageDal;
      }
        //BlackListIP
      public static IBlackListIPDal GetBlackListIPDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".BlackListIPDal", true);
          return obj as BlackListIPDal;
      }
        //WonderfulTxtImg
      public static IWonderfulTxtImgDal GetWonderfulTxtImgDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".WonderfulTxtImgDal", true);
          return obj as WonderfulTxtImgDal;
      }
        //_SeoDal
      public static ISeoDal GetSeoDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".SeoDal", true);
          return obj as SeoDal;
      }
        //AdvertisementDal
      public static IAdvertisementDal GetAdvertisementDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".AdvertisementDal", true);
          return obj as AdvertisementDal;
      }
      public static IUserAdressDal GetUserAdressDal()
      {
          object obj = Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserAdressDal", true);
          return obj as UserAdressDal;
      }
    
     

        #region  //-----------------------------简单工厂实例创建---------------------------------------
        //public static INewsInfoDal GetNewsInfoDal()
      //{
      //    return new NewsInfoDal();
      //   // return new AdoNetNewsInfoDal();---------AdoNet数据库操作、创建AdoNet对象
      //}
     
      //public static IProductDal GerProductDal()
      //{
      //    return new ProductDal();
      //}
      //public static IEngineeringCaseDal GetEngineeringCaseDal()
      //{
      //    return new EngineeringCaseDal();

      //}
      //public static ILeaveMsgDal GetLeaveMsgDal()
      //{
      //    return new LeaveMsgDal();
        //}
        #endregion
    }
}
