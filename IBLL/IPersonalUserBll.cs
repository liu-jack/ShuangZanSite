using Models;
using Models.Params;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IPersonalUserBll : IBaseBll<PersonalUser>
    {
        IQueryable<PersonalUser> LoadPagePersonalUser(QueryParam param);
       /// <summary>
       /// 发送验证码
       /// </summary>
       /// <param name="mobile">手机号</param>
       /// <param name="vcode">验证码</param>
       /// <param name="ip">真实ip</param>
       /// <returns></returns>
        Sql_UserModel SendVCode(string mobile, string vcode, string ip);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="uname">用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="tjid">推荐人id</param>
        /// <returns></returns>
        Sql_UserSignInModel SignIn(string uname, string pass, string mobile, string code, int tjid);
        /// <summary>
        /// 验证码与手机号是否匹配
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        int CheckMobileCode(string mobile, string code);
        int UpdateUserMobile(string mobile,int id);
        int UpdateUserPassword(string pwd, int id);
        Sql_UserGetPwd UserGetPass(string mobile, string code);
       
    }
}
