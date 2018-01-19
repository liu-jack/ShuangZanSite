using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShuangZan.Web.Controllers
{
       [LoginCheckFilterAttribute(IsCheck = false)]
    public class ForbiddenListController : BaseController
    {        
        // GET: /ForbiddenList/    
        public IForbiddenListBll  ForbiddenListBll { get; set; }
               IBlackListIPBll BlackListIPBll{get;set;}
        public ActionResult Index()
        {
            return View();
        }
        #region 添加禁用词
        //添加禁用词
        public ActionResult AddContent(FormCollection form)
        {
            string msg = form["msg"];
            msg = msg.Trim();
            string[] words = msg.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in words)
            {
                string[] word = str.Split('=');
                ForbiddenList forbid = new ForbiddenList();
                forbid.WordPattern = word[0];
                forbid.Id = Guid.NewGuid();
                if (word[1] == "{BANNED}")
                {
                    forbid.IsForbid = true;
                }
                else if (word[1] == "{MOD}")
                {
                    forbid.IsMod = true;
                }
                else
                {
                    forbid.ReplaceWord = word[1];
                }

                ForbiddenListBll.Add(forbid);
            }
            return Content("ok");
        } 
        #endregion
        #region 禁用词管理
        public ActionResult bannedListPage()
        {
            return View();
        }
        public ActionResult BannedList()
        {
            string name = Request["name"];
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var allWords = ForbiddenListBll.LoadEntities(w => true);
            if (!string.IsNullOrEmpty(name))
            {
                allWords = allWords.Where(w => w.WordPattern.Contains(name));
            }
            total = allWords.Count();
            allWords = allWords.OrderByDescending(w => w.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            var result = new { total = total, rows = allWords.ToList() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BannedDel(Guid id)
        {
            ForbiddenList b = new ForbiddenList() { Id = id };
            if (ForbiddenListBll.Delete(b))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        } 
        #endregion
        public ActionResult BannedIpPage()
        {
         
            return View();
        }
        public ActionResult BannedIpList()
        { 
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var data = BlackListIPBll.LoadPageEntities(pageSize, pageIndex, out total, u => true, false, u => u.Id)
                .Select(r => new { r.Id, r.IP, r.InTime });
            var result = new { total = total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BannedIpDel(Guid id)
        {
            BlackListIP ip = new BlackListIP() { Id=id};
            if (BlackListIPBll.Delete(ip))
            {
                return Content("ko");
            }
            else {
                return Content("no");
            }
        }
                     
    }
}
