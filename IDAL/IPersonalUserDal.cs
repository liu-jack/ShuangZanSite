using Models;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IDAL
{
    public interface IPersonalUserDal : IBaseDal<PersonalUser>
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号 </param>
        /// <param name="vcode">验证码</param>
        /// <param name="ip">真实ip</param>
        /// <returns></returns>
        Sql_UserModel SendVCode(string mobile, string vcode, string ip);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pass"></param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="tjid"></param>
        /// <returns></returns>
        Sql_UserSignInModel SignIn(string uname, string pass, string mobile, string code, int tjid);
    }
}
