using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace BLL
{
    public class BlackListIPBll : BaseBll<BlackListIP>, IBlackListIPBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.BlackListIPDal;
        }
        public List<string> GetBlackListIP()
        {
          var  data= dbSession.BlackListIPDal.LoadEntities(b => true).ToList().Select(b => new BlackListIP() { 
            IP=b.IP           
            }).ToList();
          List<string> newList = new List<string>();
          foreach (var item in data)
          {
              newList.Add(item.IP);
          }
          return newList;
        }
        /// <summary>
        /// 校验黑名单ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool CheckBlackListIp(string ip)
        {
            List<string>  list=GetBlackListIP();         
           return list.Contains(ip);
           // return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
