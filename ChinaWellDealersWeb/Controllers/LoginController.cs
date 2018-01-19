using BLL;
using Common;
using IBLL;
using Models;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
namespace ShuangZan.Web.Controllers
{
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class LoginController : Controller
    {
        public ICompanyBll CompanyBll { get; set; }
        public IUserInfoBll UserInfoBll { get; set; }
        public IPersonalUserBll PersonalUserBll { get; set; }
        public IVerificationCodeBll VerificationCodeBll{ get; set; }
        public ILeaveMsgBll LeaveMsgBll { get; set; }
        public IOrderBll OrderBll { get; set; }
        public IFeedbackBll FeedbackBll{ get; set; }
        #region 管理员后台登录    
        public ActionResult Index()
        {
            string returnUrl = Request.Params["returnurl"];
            returnUrl = returnUrl ?? Url.Action("Default", "AdminData");
            ViewBag.TaskUrl = returnUrl;
            return View();
        }
        public ActionResult login()
        {
            string validatecode = Session["validateCode"] == null ? string.Empty : Session["validateCode"].ToString();
            if (string.IsNullOrEmpty(validatecode))
            {
                return Content("no:验证码错误！");
            }
            Session["validateCode"] = null;//清空session保存的验证码
            string txtCode = Request["vCode"];
            //如果两个字符串不想等
            if (!validatecode.Equals(txtCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return Content("errorCode:验证码错误!");
            }
            //校验用户名和密码
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];
            string pwd = Md5Helper.GetMd5(userPwd);
            if (userName==""||userPwd=="")
            {
                return Content("no:用户名或密码不能为空！");
            }
            else
            {               
                var users = UserInfoBll.LoadEntities(u => u.UName == userName && u.Pwd == pwd&&u.State=="1"&&u.DelFlag==0).FirstOrDefault();
                if (users != null)
                {
                    //  --------------------- 用户memcached模拟Session-----------------------------
                    Guid guid = Guid.NewGuid();
                    //guid为key,以登录用户为value放到memcached里面去
                    Common.CacheHelper.WriteCache(guid.ToString(), users, DateTime.Now.AddMinutes(250));
                    //把guid写到cookies里面去
                    Response.Cookies["mySessionId"].Value = guid.ToString();
                    users.Last_login_Time = DateTime.Now;
                    users.Login_Num = users.Login_Num + 1;
                    UserInfoBll.Update(users);                  
                    return Content("ok:登录成功！");
                }
                else {
                    return Content("fail:用户名或密码错误！");
                }
            }
        }
        #endregion
        #region 获取验证码
        public ActionResult GetValidateCode()
        {
            //创建验证码类的对象
            ValidateCodeHelper code = new ValidateCodeHelper();
            string strCode = code.CreateValidateCode(4);//生成验证码的长度
            Session["validateCode"] = strCode;
            byte[] buffer = code.CreateValidateGraphic(strCode);
            return File(buffer, "image/jpeg");
        }
        #endregion
        #region 管理员当前用户密码修改
        public ActionResult EditPwdPage()
        {

            return View();
        }
        [HttpPost]
        public ActionResult EditPwd()
        {
            //1.采集用户的输入
            string oldPwd = Request.Form["oldPwd"].Trim() as string;
            string Md5oldPwd = Md5Helper.GetMd5(oldPwd);
            string newPwd = Request.Form["newPwd"].Trim() as string;
            string confirmPwd = Request.Form["confirmPwd"].Trim() as string;
            string Md5confirmPwd = Md5Helper.GetMd5(confirmPwd);
            //2.校验两次输入的新密码是否正确
            if (newPwd == confirmPwd)
            {
                //3.校验旧密码是否正确
                //根据guid拿到当前登录的用户
                string guid = Request["mySessionId"];
                if (!string.IsNullOrEmpty(guid))
                {
                    //从分布式缓存拿出来的对象不能进行延迟加载。
                    var loginAdmin = Common.CacheHelper.Get(guid) as UserInfo;
                    if (loginAdmin != null)
                    {
                        UserInfo user = UserInfoBll.LoadEntities(u => u.Id == loginAdmin.Id).FirstOrDefault();
                        if (user.Pwd == Md5oldPwd)
                        {
                            //4.修改密码
                            user.Pwd = Md5confirmPwd;
                            if (UserInfoBll.Update(user))
                            {
                                return Content("ok");
                            }
                            else
                            {
                                return Content("fail");
                            }
                        }
                        else
                        {
                            return Content("oldPwdError");
                        }
                    }

                }
            }
            return Content("different");

        }
        #endregion
        #region 管理系统默认欢迎页面
        public ActionResult Welcome()
        {
          ViewBag.todayAllLeaveMsg=LeaveMsgBll.LoadEntities(m => true).Where(m => DbFunctions.DiffDays(m.InTime, DateTime.Now) == 0).Count();
          ViewBag.todayAllOrder = OrderBll.LoadEntities(r => true).Where(r => DbFunctions.DiffDays(r.InTime, DateTime.Now) == 0).Count();
          ViewBag.todayYj = FeedbackBll.LoadEntities(f => true).Where(f => DbFunctions.DiffDays(f.InTime, DateTime.Now) == 0).Count();
      
         ViewBag.total=HttpContext.Application["total"];
         ViewBag.onLine = HttpContext.Application["onLine"];
            
            return View();
        }
        #endregion
        #region 厂商登录
        public ActionResult LoginCompany()
        {
            return View();
        }
        public ActionResult LoginCpy()
        {
            //1、 先判断check  有没有选中、如果选中、创建cookie对象、将值存入cookie
            //   如果已经选中的、判断cookie有没有值、有值让它自动登录、否则执行2
            //2、去数据查询当前接受的用户名状态是否正常、不正常给出提示
            //3、在2的步骤通过以后判断用户名和密码是否匹配、匹配则让其登录、将信息存入到mm中。
            string uName = Request["uname"];
            string pwd = Request["pass"];
            string check = Request["check"];
            var cpyUser = CompanyBll.LoadEntities(c => c.UName == uName && c.Pwd == pwd).FirstOrDefault();
            if (cpyUser != null)
            {
                if (cpyUser.State == 1)
                {
                    Guid guid = Guid.NewGuid();
                    if (check == "true")
                    {
                        #region cookie的运用
                        //HttpCookie cookie = new HttpCookie("uname");
                        //cookie.Expires = DateTime.Now.AddYears(1);
                        //cookie.Value = uName;
                        //System.Web.HttpContext.Current.Response.Cookies.Add(cookie); 
                        #endregion                     
                        Common.CacheHelper.WriteCache(guid.ToString(), cpyUser, DateTime.Now.AddDays(365));
                        //把guid写到cookies里面去
                        Response.Cookies["cpyUserId"].Value = guid.ToString();
                        return Content("ok:登录成功！");
                    }
                    else {
                        Common.CacheHelper.WriteCache(guid.ToString(), cpyUser, DateTime.Now.AddMinutes(50));
                        //把guid写到cookies里面去
                        Response.Cookies["cpyUserId"].Value = guid.ToString();
                        return Content("ok:登录成功！");
                    }                              
                }
                else
                {
                    return Content("errorState:您的用户信息正在审核中,请稍候再试！");
                }
            }
            else
            {
                return Content("no:用户名或密码错误！");
            }

            #region MyRegion
                      
            //if (check == "true")
            //{
            //    var cpyUser = CompanyBll.LoadEntities(c => c.UName == uName && c.Pwd == pwd).FirstOrDefault();
            //    if (cpyUser != null)
            //    {
            //        if (cpyUser.State == 1)
            //        {
            //            #region cookie的运用
            //            //HttpCookie cookie = new HttpCookie("uname");
            //            //cookie.Expires = DateTime.Now.AddYears(1);
            //            //cookie.Value = uName;
            //            //System.Web.HttpContext.Current.Response.Cookies.Add(cookie); 
            //            #endregion
            //            Guid guid = Guid.NewGuid();
            //            Common.CacheHelper.WriteCache(guid.ToString(), cpyUser, DateTime.Now.AddMinutes(100));
            //            //把guid写到cookies里面去
            //            Response.Cookies["cpyUserId"].Value = guid.ToString();
            //            return Content("ok:登录成功！");
            //        }
            //        else
            //        {

            //            return Content("errorState:您的用户信息正在审核中,请稍候再试！");
            //        }
            //    }
            //    else
            //    {
            //        return Content("no:用户名或密码错误！");
            //    }
            //}
            //else
            //{
            //    var cpyUser = CompanyBll.LoadEntities(c => c.UName == uName && c.Pwd == pwd).FirstOrDefault();
            //    if (cpyUser != null)
            //    {
            //        if (cpyUser.State == 1)
            //        {
            //            Guid guid = Guid.NewGuid();
            //            Common.CacheHelper.WriteCache(guid.ToString(), cpyUser, DateTime.Now.AddMinutes(100));
            //            //把guid写到cookies里面去
            //            Response.Cookies["cpyUserId"].Value = guid.ToString();
            //            return Content("ok:登录成功！");
            //        }
            //        else
            //        {

            //            return Content("errorState:您的用户信息正在审核中,请稍候再试！");
            //        }
            //    }
            //    else
            //    {
            //        return Content("no:用户名或密码错误！");
            //    }
            //}
            #endregion
        }
        #endregion
        #region 个人玩家中心登录
        public ActionResult PersonalLogin()
        {
            return View();
        }
        public ActionResult GetPersonUserLogin()
        {
            string uName = Request["uname"];
            string pwd = Request["pass"];
            string check = Request["check"];
            if (check == "true")
            {
                var user = PersonalUserBll.LoadEntities(c => c.UName == uName||c.Mobile==uName&&c.Password == pwd).FirstOrDefault();
                if (user != null)
                {
                    if (user.State == 1)
                    {
                        HttpCookie cookie = new HttpCookie("personUser");
                        cookie.Expires = DateTime.Now.AddYears(1);
                        cookie.Value = new PersonalUser()
                            {
                                Id = int.Parse(user.Id.ToString()),
                                UName = user.UName.ToString(),
                                Password = user.Password.ToString(),
                            }.ToString();
                        // System.Web.HttpContext.Current.Response.Cookies.Add(cookie); 
                        System.Web.HttpContext.Current.Response.SetCookie(cookie);
                        Guid guid = Guid.NewGuid();
                        Common.CacheHelper.WriteCache(guid.ToString(), user, DateTime.Now.AddMinutes(100));
                        //把guid写到cookies里面去
                        Response.Cookies["userId"].Value = guid.ToString();
                        return Content("ok:登录成功！");
                    }
                    else
                    {

                        return Content("errorState:您的用户信息正在审核中,请稍候再试！");
                    }
                }
                else
                {
                    return Content("no:登陆失败了！您的输入信息有误,请重新输入！");
                }
            }
            else
            {
                var user = PersonalUserBll.LoadEntities(c => c.UName == uName || c.Mobile == uName && c.Password == pwd).FirstOrDefault();
                if (user != null)
                {
                    if (user.State == 1)
                    {
                        Guid guid = Guid.NewGuid();
                        Common.CacheHelper.WriteCache(guid.ToString(), user, DateTime.Now.AddMinutes(100));
                        //把guid写到cookies里面去
                        Response.Cookies["userId"].Value = guid.ToString();
                        return Content("ok:登录成功！");
                    }
                    else
                    {
                        return Content("errorState:您的用户信息正在审核中,请稍候再试！");
                    }
                }
                else
                {
                    return Content("no:登陆失败了！您的输入信息有误,请重新输入！");
                }
            }
        }
        #endregion   
        #region 厂商注册
        public ActionResult RegCompany()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            try
            {
                Company cpy = new Company()
                {
                    UName = Request["uname"],
                    Email = Request["email"],
                    Pwd = Request["pass"],
                    SystemName = Request["systemname"],
                    WebSite = Request["website"],
                    CompanyName = Request["companyname"],
                    Phone = Request["phone"],
                    Address = Request["address"],
                    Contacts = Request["contacts"],
                    Office = Request["office"],
                    Moblie = Request["mobile"],
                    Sex = int.Parse(Request["sex"]),
                    QQ = Request["qq"],
                    Head = "3.jpg",
                    // Logo="ssdss",
                    State = 0,
                    // CopanyMsg="sddsdsdsdsds",
                    Rec_Forum_Index = "0",
                    Rec_Index_Time = DateTime.Now,
                    Rec_Index = "0",
                    Rec_Forum_Index_Time = DateTime.Now,
                    DelFlag = 0,
                    InTime = DateTime.Now
                };
                cpy = CompanyBll.Add(cpy);
                return Content("ok");
            }
            catch (DbEntityValidationException dbex)
            {

                throw new Exception("注册出错了！！" + dbex.Message);
            }
        } 
        #endregion
        #region 厂商用户名验证
        public ActionResult CheckUName()
        {
            string uName = Request["uname"];
            var cpyUname = CompanyBll.LoadEntities(c => c.UName == uName);
            return Json(cpyUname, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 厂商邮箱验证
        public ActionResult CheckEMail(string email)
        {
            var cpyEmail = CompanyBll.LoadEntities(c => c.Email == email);
            return Json(cpyEmail, JsonRequestBehavior.AllowGet);
        }
        #endregion  
        #region 获取手机验证码进行匹配
        public ActionResult GetMobileVerifyCode(string mobile,string vcode)
        {
          
            #region 1.0版本获取验证码
            string realIp="";
            string validatecode = Session["validateCode"] == null ? string.Empty : Session["validateCode"].ToString();
            if (string.IsNullOrEmpty(validatecode))
            {
                return Content("no:验证码错误！");
            }
            Session["validateCode"] = null;
            if (!validatecode.Equals(vcode, StringComparison.CurrentCultureIgnoreCase))
                 return Json(new{success="false",str="验证码输入不正确"},JsonRequestBehavior.AllowGet);             
            if (mobile.Length != 11)
                 return Json(new{success="false",str="手机号码长度不正确"},JsonRequestBehavior.AllowGet);             
            if (mobile.ToString()==null)               
             return Json(new{success="false",str="手机号码不正确"},JsonRequestBehavior.AllowGet);
            Random  r=  new Random();
            string code2 = r.Next(100000, 999999).ToString();
            //往数据库发送验证码，手机号
           var code = PersonalUserBll.SendVCode(mobile, code2, realIp);           
            if (code.OutCode!=null)
            {
                try
                {
                    ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23450194", "52b5a70bc10ba56ccfe6e50bdb9fa8d4");
                    AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                    req.Extend = "";
                    req.SmsType = "normal";
                    req.SmsFreeSignName = "爽赞游戏网"; //签名,将来要改
                    req.SmsParam = "{\"code\":\"" + code.OutCode + "\",\"product\":\"爽赞游戏网(www.shuangzan.com)\"}";
                    req.RecNum = mobile;
                    req.SmsTemplateCode = "SMS_14720884"; //短信模板
                  //  CS.Config.SaveErr(req.SmsParam);
                    AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                   // CS.Config.SaveErr(rsp.Body);
                    if (rsp.Result.Success)                   
                        return Json(new{success="true",str="验证码发送成功"},JsonRequestBehavior.AllowGet);
                    else
                    {
                        //CS.Config.SaveErr(rsp.Body);
                          return Json(new{success="true",str="验证码发送失败"},JsonRequestBehavior.AllowGet);                      
                    }
                }
                catch (Exception e) 
                {
                      return Json(new{success="false",str="短信发送失败，原因未知"+e.Message},JsonRequestBehavior.AllowGet);                     
                }
            }
            else
                return Json(new { success = "false", str = "您的验证码短信仍在30分钟有效期内" }, JsonRequestBehavior.AllowGet);  
            
            #endregion
        }
        #endregion
        #region 修改绑定手机
        public ActionResult GetCode(string mobile)
        {
           
             string realIp="";
            if (mobile.Length != 11)
                return Json(new { success = "false", str = "手机号码长度不正确" }, JsonRequestBehavior.AllowGet);
            if (mobile.ToString() == null)
                return Json(new { success = "false", str = "手机号码不正确" }, JsonRequestBehavior.AllowGet);
            Random r = new Random();
            string code2 = r.Next(100000, 999999).ToString();
            //往数据库发送验证码，手机号
            var code = PersonalUserBll.SendVCode(mobile, code2, realIp);
            if (code.OutCode != null)
            {
                try
                {
                    ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23450194", "52b5a70bc10ba56ccfe6e50bdb9fa8d4");
                    AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                    req.Extend = "";
                    req.SmsType = "normal";
                    req.SmsFreeSignName = "爽赞游戏网"; //签名,将来要改
                    req.SmsParam = "{\"code\":\"" + code.OutCode + "\",\"product\":\"爽赞游戏网(www.shuangzan.com)\"}";
                    req.RecNum = mobile;
                    req.SmsTemplateCode = "SMS_14720884"; //短信模板
                    //  CS.Config.SaveErr(req.SmsParam);
                    AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                    // CS.Config.SaveErr(rsp.Body);
                    if (rsp.Result.Success)
                        return Json(new { success = "true", str = "验证码发送成功" }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        //CS.Config.SaveErr(rsp.Body);
                        return Json(new { success = "true", str = "验证码发送失败" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { success = "false", str = "短信发送失败，原因未知" + e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { success = "false", str = "您的验证码短信仍在30分钟有效期内" }, JsonRequestBehavior.AllowGet);           
        }
        #endregion
        #region 个人玩家注册
        public ActionResult RegPersonalUser()
        {
            return View();
        }
        public ActionResult SignInPersonalUser(string uname, string pass, string mobile, string code)
        {
            int tjid =int.Parse(Request["tjid"]);
             var  data= PersonalUserBll.SignIn(uname, pass, mobile, code, tjid);
             return Json(data, JsonRequestBehavior.AllowGet);                 
        }
        #endregion
        #region 验证码与手机号是否匹配
        // 验证码与手机号是否匹配
        public ActionResult CheckMobileCode(string mobile, string code)
        {
            int i = PersonalUserBll.CheckMobileCode(mobile, code);
            if (i > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        } 
        #endregion
        #region 个人玩家找回密码
        public ActionResult FindUserPwd()
        {
            return View();
        }
        public ActionResult UserGetPass(string mobile, string code)
        {

            var model = PersonalUserBll.UserGetPass(mobile, new Random().Next(100000, 999999).ToString());
            try
            {
                ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23450194", "52b5a70bc10ba56ccfe6e50bdb9fa8d4");
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.Extend = "";
                req.SmsType = "normal";
                req.SmsFreeSignName = "爽赞网"; //签名,将来要改
                req.SmsParam = "{\"code\":\"" + model.Pass + "\",\"product\":\"爽赞\"}";
                req.RecNum = mobile;
                req.SmsTemplateCode = "SMS_35430038"; //短信模板

                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);

                if (rsp.Result.Success)
                    return Json(new { success = "true", str = "验证码发送成功" }, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(new { success = "false", str = "验证码发送失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e) { return Json(new { success = "false", str = "短信发送失败，原因未知" + e.Message }, JsonRequestBehavior.AllowGet); }
        } 
        #endregion     
        #region 厂商找回密码
        public ActionResult CpyFindPwd()
        {
            return View();
        }
        public ActionResult FindPwd(string email)
        {
            var cpy = CompanyBll.LoadEntities(c => email.Equals(c.Email) || email.Equals(c.UName)).FirstOrDefault();

            if (CompanyGetPass(cpy))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public bool CompanyGetPass(Company cpy)
        {          
                if (cpy == null)
                    return false;
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("kf@shuangzan.com", "爽赞游戏网", Encoding.UTF8);//源邮件地址 
                mailMsg.To.Add(new MailAddress(cpy.Email, cpy.UName, Encoding.UTF8));//目的邮件地址。可以有多个收件人
                mailMsg.Subject = "【爽赞网】—会员找回密码通知 请注意查收！";//发送邮件的标题 
                mailMsg.Body = "亲爱的爽赞会员您好：您的爽赞厂商自助系统登录密码为：" + cpy.Pwd + "。请点击以下链接，进行登录：http://www.shuangzan.com/Login/LoginCompany <br /> 如果以上链接不能点击，你可以复制网址URL, 然后粘贴到浏览器地址栏打开，完成登录。（这是一封自动发送的邮件，请不要直接回复）";
                //指定Smtp服务地址。
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.exmail.qq.com";
                client.Port = 25;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;             
                client.Credentials = new NetworkCredential("kf@shuangzan.com", "Nancy123456");              
                try
                {   
                    client.Send(mailMsg);
                }
                catch (Exception ex)
                {
                  
                    throw ex;
                  
                }
                finally { client.Dispose(); }
                return true;             
          
           
        }  
        #endregion
        #region 服务条款
        public ActionResult TermsService()
        {
            return View();
        }
        #endregion
      
    }
}
