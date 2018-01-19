using DAL;
using IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DalFactory
{
    /// <summary>
    /// 干的抽象工厂的活、拿到所有的dal、数据库统一访问入口
    /// </summary>
    public class DbSession : IDbSession
    {
        public DbContext Db
        {
            get
            {
                return DbContextFactory.GetCurrentDbContext();
            }
        }
        #region 批处理  一次提交程序
        public int SaveChanges()
        {
            //让当前的上下文（一次一个上下文。）进行提交。
            return DbContextFactory.GetCurrentDbContext().SaveChanges();        
        }
        #endregion

        #region 处理sql语句
        public int ExecuteSql(string sql, params SqlParameter[] pars)
        {
            
            return Db.Database.ExecuteSqlCommand(sql, pars);
        }
        public List<T> ExecuteQuery<T>(string sql, params SqlParameter[] pars)
        {
            return Db.Database.SqlQuery<T>(sql, pars).ToList();
            
            

        } 
        #endregion

        #region 定义一个私有字段、通过其属性赋值。
        private IUserInfoDal _UserInfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if (_UserInfoDal == null)
                {
                    _UserInfoDal = DalFactory.GetUserInfoDal();
                }
                return _UserInfoDal;
            }
            set
            {

            }
        }
        #region 角色类
        private IRoleInfoDal _RoleInfoDal;
        public IRoleInfoDal RoleInfoDal
        {


            get
            {
                if (_RoleInfoDal == null)
                {
                    _RoleInfoDal = DalFactory.GetRoleInfoDal();
                }
                return _RoleInfoDal;
            }
            set { }

        }
        #endregion
        private IActionInfoDal _ActionInfoDal;
        public IActionInfoDal ActionInfoDal
        {
            get
            {
                if (_ActionInfoDal == null)
                {
                    _ActionInfoDal = DalFactory.GetActionInfoDal();
                }
                return _ActionInfoDal;
            }

            set { }
        }
        private IR_UserInfo_ActionInfoDal _R_UserInfo_ActionInfoDal;
        public IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal
        {
            get {
                if (_R_UserInfo_ActionInfoDal==null)
                {
                    _R_UserInfo_ActionInfoDal = DalFactory.GetR_UserInfo_ActionInfoDal(); 
                }
                return _R_UserInfo_ActionInfoDal;
            }
            set { }
        }

        private ICompanyDal _CompanyDal;
        public ICompanyDal CompanyDal
        {
            get
            {
                if (_CompanyDal==null)
                {
                    _CompanyDal = DalFactory.GetCompanyDal(); 
                }
                return _CompanyDal;
            }
            set
            {
              
            }
        }

        private INewsDal _NewsDal;
        public INewsDal NewsDal
        {
            get
            {
                if (_NewsDal==null)
                {
                    _NewsDal = DalFactory.GetNewsDal();
                }
                return _NewsDal;
            }
            set
            {
                
            }
        }

        private IPackageDal _PackageDal;
        public IPackageDal PackageDal
        {
            get
            {
                if (_PackageDal==null)
                {
                    _PackageDal = DalFactory.GetPackageDal(); 
                }
                return _PackageDal;
            }
            set
            {
              
            }
        }

        private IPackageCardDal _PackageCardDal;
        public IPackageCardDal PackageCardDal
        {
            get
            {
                if (_PackageCardDal==null)
                {
                    _PackageCardDal = DalFactory.GetPackageCardDal();
                }
                return _PackageCardDal;
            }
            set
            {
               
            }
        }

        private IRechargeDal _RechargeDal;
        public IRechargeDal RechargeDal
        {
            get
            {
                if (_RechargeDal==null)
                {
                    _RechargeDal = DalFactory.GetRechargeDal(); 
                }
                return _RechargeDal;
            }
            set
            {
            
            }
        }

        private ILeaveMsgDal _LeaveMsgDal;
        public ILeaveMsgDal LeaveMsgDal
        {
            get
            {
                if (_LeaveMsgDal==null)
                {
                    _LeaveMsgDal = DalFactory.GetLeaveMsgDal();
                }
                return _LeaveMsgDal;
            }
            set
            {
               
            }
        }

        private IPersonalUserDal _PersonalUserDal;
        public IPersonalUserDal PersonalUserDal
        {
            get
            {
                if (_PersonalUserDal==null)
                {
                    _PersonalUserDal = DalFactory.GetPersonalUserDal();
                }
                return _PersonalUserDal;
            }
            set
            {
               
            }
        }

        private IUserMessageDal _UserMessageDal;
        public IUserMessageDal UserMessageDal
        {
            get
            {
                if (_UserMessageDal==null)
                {
                    ; _UserMessageDal = DalFactory.GetUserMessageDal();
                }
                return _UserMessageDal;
            }
            set
            {
               
            }
        }

        private IOpenServiceDal _OpenServiceDal;
        public IOpenServiceDal OpenServiceDal
        {
            get
            {
                if (_OpenServiceDal==null)
                {
                    _OpenServiceDal = DalFactory.GetOpenServiceDal();
                }
                return _OpenServiceDal;
            }
            set
            {
              
            }
        }

        private IUserRaidersDal _UserRaidersDal;
        public IUserRaidersDal UserRaidersDal
        {
            get
            {
                if (_UserRaidersDal==null)
                {
                    _UserRaidersDal = DalFactory.GetUserRaidersDal();
                }
                return _UserRaidersDal;
            }
            set
            {
               
            }
        }
        #endregion

        private IGameDal _GameDal;
        public IGameDal GameDal
        {
            get
            {
                if (_GameDal==null)
                {
                    _GameDal = DalFactory.GetGameDal();  
                }
                return _GameDal; 
            } 
            set
            {
                
            }
        }

        private IKeyWordsDal _KeyWordsDal;
        public IKeyWordsDal KeyWordsDal
        {
            get
            {
                if (_KeyWordsDal==null)
                {
                    _KeyWordsDal = DalFactory.GetKeyWordsDal();  
                }
                return _KeyWordsDal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private ITestDal _TestDal;
        public ITestDal TestDal
        {
            get
            {
               if (_TestDal==null)
	            {
		            _TestDal=DalFactory.GetTestDal();
	            }
                return _TestDal;
            }
            set
            {
              
            }
        }

        private IBeautifulGirlsDal _BeautifulGirlsDal;
        public IBeautifulGirlsDal BeautifulGirlsDal
        {
            get
            {
                if (_BeautifulGirlsDal==null)
                {
                    _BeautifulGirlsDal = DalFactory.GetBeautifulGirlsDal();  
                }
                return _BeautifulGirlsDal;
            }
            set
            {
                
            }
        }

        private IOrderDal _OrderDal;
        public IOrderDal OrderDal
        {
            get
            {
                if (_OrderDal==null)
                {
                    _OrderDal = DalFactory.GetOrderDal(); 
                }
                return _OrderDal;
            }
            set
            {
              
            }
        }

        private IFeedbackDal _FeedbackDal;
        public IFeedbackDal FeedbackDal
        {
            get
            {
                if (_FeedbackDal==null)
                {
                    _FeedbackDal = DalFactory.GetFeedbackDal();                   
                }
                return _FeedbackDal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private IGiftDal _GiftDal;
        public IGiftDal GiftDal
        {
            get
            {
                if (_GiftDal==null)
                {
                    _GiftDal = DalFactory.GetGiftDal(); 
                }
                return _GiftDal;
            }
            set
            {
               
            }
        }

        private IGameDemoDal _GameDemoDal;
        public IGameDemoDal GameDemoDal
        {
            get
            {
                if (_GameDemoDal==null)
                {
                    _GameDemoDal = DalFactory.GetGameDemoDal();
                }
                return _GameDemoDal;
            }
            set
            {
               
            }
        }

        private IGameDemoRechargeDal _GameDemoRechargeDal;
        public IGameDemoRechargeDal GameDemoRechargeDal
        {
            get
            {
                if (_GameDemoRechargeDal==null)
                {
                    _GameDemoRechargeDal = DalFactory.GetGameDemoRechargeDal();
                }
                return _GameDemoRechargeDal;
            }
            set
            {
               
            }
        }
        private IGameDemoAccountsDal _GameDemoAccountsDal;
        public IGameDemoAccountsDal GameDemoAccountsDal
        {
            get
            {
                if (_GameDemoAccountsDal==null)
                {
                    _GameDemoAccountsDal = DalFactory.GetGameDemoAccountsDal();
                }
                return _GameDemoAccountsDal;
            }
            set
            {               
            }
        }

        private IGameDemoRequiresDal _GameDemoRequiresDal;
        public IGameDemoRequiresDal GameDemoRequiresDal
        {
            get
            {
                if (_GameDemoRequiresDal==null)
                {
                    _GameDemoRequiresDal = DalFactory.GetGameDemoRequiresDal();
                }
                return _GameDemoRequiresDal;
            }
            set
            {               
            }
        }

        private ICompanyGameDal _CompanyGameDal;
        public ICompanyGameDal CompanyGameDal
        {
            get
            {
                if (_CompanyGameDal ==null)
                {
                    _CompanyGameDal = DalFactory.GetCompanyGameDal();
                }
                return _CompanyGameDal;

            }
            set
            {
                
            }
        }

        private IRechargedUsedDal _RechargedUsedDal;
        public IRechargedUsedDal RechargedUsedDal
        {
            get
            {
                if (_RechargedUsedDal == null)
                {
                    _RechargedUsedDal = DalFactory.GetRechargedUsedDal();  
                }
                return _RechargedUsedDal;
            }
            set
            {               
            }
        }

        private IAuditLogDal _AuditLogDal;
        public IAuditLogDal AuditLogDal
        {
            get
            {
                if (_AuditLogDal==null)
                {
                    _AuditLogDal = DalFactory.GetAuditLogDal();
                }
                return _AuditLogDal;
            }
            set
            {             
            }
        }
        private IVerificationCodeDal _VerificationCodeDal;
        public IVerificationCodeDal VerificationCodeDal {

            get
            {
                if (_VerificationCodeDal == null)
                {
                    _VerificationCodeDal = DalFactory.GetVerificationCodeDal();
                }
                return _VerificationCodeDal;
            }
            set
            {

            }
        }

        private IDemoUserDal _DemoUserDal;
        public IDemoUserDal DemoUserDal
        {
            get
            {
                if (_DemoUserDal==null)
                {
                    _DemoUserDal = DalFactory.GetDemoUserDal();
                }
                return _DemoUserDal;
            }
            set
            {
               
            }
        }

        private IPersonalUserSignDal _PersonalUserSignDal;
        public IPersonalUserSignDal PersonalUserSignDal
        {
            get
            {
                if (_PersonalUserSignDal==null)
                {
                    _PersonalUserSignDal = DalFactory.GetPersonalUserSignDal(); 
                }
                return _PersonalUserSignDal;
            }
            set
            {
               
            }
        }

        private IPersonalUserSignDetailDal _PersonalUserSignDetailDal;
        public IPersonalUserSignDetailDal PersonalUserSignDetailDal
        {
            get
            {
                if (_PersonalUserSignDetailDal==null)
                {
                    _PersonalUserSignDetailDal = DalFactory.GetPersonalUserSignDetailDal();  
                }
                return _PersonalUserSignDetailDal;
            }
            set
            {
               
            }
        }

        private ISeeNewsDal _SeeNewsDal;
        public ISeeNewsDal SeeNewsDal
        {
            get
            {
                if (_SeeNewsDal==null)
                {
                    _SeeNewsDal = DalFactory.GetSeeNewsDal(); 
                }
                return _SeeNewsDal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private IForbiddenListDal _ForbiddenListDal;
        public IForbiddenListDal ForbiddenListDal
        {
            get
            {
                if (_ForbiddenListDal==null)
                {
                    _ForbiddenListDal = DalFactory.GetForbiddenListDal();
                }
                return _ForbiddenListDal;
            }
            set
            {
               
            }
        }
        private IHomePageDal _HomePageDal;
        public IHomePageDal HomePageDal
        {
            get
            {
                if (_HomePageDal == null)
                {
                    _HomePageDal = DalFactory.GetHomePageDal();
                }
                return _HomePageDal;
            }
            set
            {
                
            }
        }

        private IBlackListIPDal _BlackListIPDal;
        public IBlackListIPDal BlackListIPDal
        {
            get
            {
                if (_BlackListIPDal==null)
                {
                    _BlackListIPDal = DalFactory.GetBlackListIPDal();
                }
                return _BlackListIPDal;
            }
            set
            {
                
            }
        }

        private IWonderfulTxtImgDal _WonderfulTxtImgDal;
        public IWonderfulTxtImgDal WonderfulTxtImgDal
        {
            get
            {
                if (_WonderfulTxtImgDal==null)
                {
                    _WonderfulTxtImgDal = DalFactory.GetWonderfulTxtImgDal();
                }
                return _WonderfulTxtImgDal;
            }
            set
            {
              
            }
        }
        //Seo
        private ISeoDal _SeoDal;
        public ISeoDal SeoDal
        {
            get
            {
                if (_SeoDal == null)
                {
                    _SeoDal = DalFactory.GetSeoDal();
                }
                return _SeoDal;
            }
            set
            {
               
            }
        }
        private IAdvertisementDal _AdvertisementDal;
        public IAdvertisementDal AdvertisementDal
        {
            get
            {
                if (_AdvertisementDal == null)
                {
                    _AdvertisementDal = DalFactory.GetAdvertisementDal();
                }
                return _AdvertisementDal;
            }

            set { }
        }

        private IUserAdressDal _UserAdressDal;
        public IUserAdressDal UserAdressDal
        {
            get
            {
                if (_UserAdressDal==null)
                {
                   _UserAdressDal = DalFactory.GetUserAdressDal();
                }
                return _UserAdressDal;
            }
            set
            {
                
            }
        }

     
    }
}
