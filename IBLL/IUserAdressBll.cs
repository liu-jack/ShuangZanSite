using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IUserAdressBll : IBaseBll<UserAdress>
    {
        bool UpdateUserAddress(int id,int userId);
        /// <summary>
        /// 添加收货地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool AddUserAddress(int userId, int isdefault, string name, string address, string phone);
    }
}
