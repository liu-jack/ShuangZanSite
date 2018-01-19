using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface IForbiddenListBll : IBaseBll<ForbiddenList>
    {
        /// <summary>
        /// 查询出所有的禁用词放到缓存memcached中去
        /// </summary>
        /// <returns></returns>
        List<string> GetAllForbiddenWord();
       /// <summary>
       /// 匹配是否包含禁用词
       /// </summary>
       /// <param name="msg">传递过来的评论内容</param>
       /// <returns></returns>
        bool CheckBannd(string msg);
    }
}
