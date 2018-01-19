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
    public class RoleInfoController : BaseController
    {
        //
        // GET: /RoleInfo/
        public IRoleInfoBll RoleInfoBll { get; set; }
        short delNormal = (short)Models.Enum.DelFlagEnum.Normal;
        #region 查询所有的角色列表            
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllRoleInfos()
        {
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var allRole = RoleInfoBll.LoadPageEntities(pageSize, pageIndex, out total, u => u.DelFlag==delNormal, false, u => u.Id)
                . Select(r => new { r.Id,r.RoleName,r.SubTime});
            var result = new { total = total, rows = allRole };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 批量伪删除角色
        public ActionResult RoleDelete(string Ids)
        {
            if (string.IsNullOrEmpty(Ids))
            {
                return Content("empty");
            }
            string[] allIds = Ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var i in allIds)
            {
                idList.Add(int.Parse(i));
            }
            RoleInfoBll.DeleteRoles(idList);
            return Content("ok");
        }
        #endregion
        #region 添加角色             
        public ActionResult AddRole(FormCollection form)
        {
            RoleInfo role = new RoleInfo() { 
            RoleName=form["roleName"] as string,
            DelFlag=delNormal,
            SubTime=DateTime.Now            
            };
            if (role!= null)
            {
                RoleInfoBll.Add(role);
                return Content("ok");
            }
            else {
                return Content("empty");
            }
        }
        #endregion
    }
}
