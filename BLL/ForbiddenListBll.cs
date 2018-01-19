using IBLL;
using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class ForbiddenListBll : BaseBll<ForbiddenList>, IForbiddenListBll
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.ForbiddenListDal;
        }
        /// <summary>
        /// 查询出所有的禁用词放到缓存memcached中去
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllForbiddenWord()
        {

            List <ForbiddenList> list = dbSession.ForbiddenListDal.LoadEntities(f => true).ToList().Select(f => new ForbiddenList()
            {
                WordPattern = f.WordPattern
            }).ToList();
            List<string> newList = new List<string>();
            foreach (var item in list)
            {
                newList.Add(item.WordPattern);
            }
            
             return newList;
        }
        /// <summary>
        /// 匹配是否存在禁用词
        /// </summary>
        /// <param name="msg">传递过来的评论内容</param>
        /// <returns></returns>

        public bool CheckBannd(string msg)
        {
           object obj = Common.CacheHelper.Get("banned");
           List<string > list = null;
           if (obj == null)
           {
               Common.CacheHelper.WriteCache("banned", list, DateTime.Now.AddMinutes(10));
           }
           else
           {
               list = obj as List<string>;
           }
           list = obj as List<string>;
            list = GetAllForbiddenWord();         
            string regex = string.Join("|", list.ToArray());//aa|bb|cc|
            return Regex.IsMatch(msg, regex);
        }
    }
}
