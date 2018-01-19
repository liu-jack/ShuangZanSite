using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Enum
{
    public enum Reccommend
    {
        /// <summary>
        /// 首页推荐
        /// </summary>
        RecIndex=1,
        /// <summary>
        ///未推荐到首页
        /// </summary>
        RecNoIndex=0,
        /// <summary>
        /// 板块首页推荐
        /// </summary>
       RecForumIndex=1,
        /// <summary>
        /// 板块未推荐到首页
        /// </summary>
       RecNoForumIndex=0,
    }
}
