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
    public class ActionInfoController : BaseController
    {
        //
        // GET: /ActionInfo/
        public IActionInfoBll ActionInfoBll { get; set; }
        short delNormal = (short)Models.Enum.DelFlagEnum.Normal;
        #region 权限信息列表           
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetActionInfos()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var allAction = ActionInfoBll.LoadPageEntities(pageSize, pageIndex, out total, a => a.DelFlag == delNormal, false, a => a.Id)
                         .Select(
                             a => new { a.Id, a.ActionName, a.ActionType, a.SubTime, a.HttpMethod, a.MenuName, a.Url }
                             );
            var result=new {total=total,rows=allAction};
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 添加权限
        public ActionResult AddAction(FormCollection form)
     {
        //short actionType     
        ActionInfo action = new ActionInfo() { 
        ActionName=form["actionName"] as string ,
        Url=form ["url"] as    string,
        HttpMethod =form["httpMethod"]  as string,
       ActionType =1 ,
        MenuName=form["remark"],
        DelFlag=delNormal,
        SubTime=DateTime.Now,
        };
        if (action != null)
        {
            ActionInfoBll.Add(action);
            return Content("ok");
        }
        else {
            return Content("empty");
        }          
      }
        #endregion
        #region 删除权限 真删除
        [HttpPost]
        public ActionResult ActionDelete(int   ids)
        {
            ActionInfo action = new ActionInfo()
            {
                Id = ids,
            };
            if (action != null)
            {
                ActionInfoBll.Delete(action);
                return Content("ok");
            }
            else
            {
                return Content("empty");
            }
            #region 批量伪删除                      
            //if (string.IsNullOrEmpty(ids))
            //{
            //    return Content("empty");
            //}
            //string[] allIds = ids.Split(',');
            //List<int> idList = new List<int>();
            //foreach (var i in allIds)
            //{
            //    idList.Add(int.Parse(i));
            //}
            //ActionInfoBll.DeleteActions(idList);
            //return Content("ok");
            #endregion
        }
        #endregion
    }
}
 