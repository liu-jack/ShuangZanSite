using Common;
using IBLL;
using Models;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShuangZan.Web.Controllers
{
     [LoginCheckFilterAttribute(IsCheck = false)]
    public class ManageUsersController : BaseController// Controller//
    {
        //管理员控制器
        // GET: /ManageUsers/
        public IUserInfoBll UserInfoBll { get; set; }
        public IRoleInfoBll RoleInfoBll { get; set; }
        public IActionInfoBll  ActionInfoBll { get; set; }
    
        public IR_UserInfo_ActionInfoBll R_UserInfo_ActionInfoBll { get; set; }
        short delNormal = (short)Models.Enum.DelFlagEnum.Normal;
        //测试数据
        public ActionResult Index()
        {
            ViewData["users"] = UserInfoBll.LoadEntities(u => true).ToList();
            return View();
        }
        #region 管理员数据展示及搜索
         [OutputCache(CacheProfile="ItemConfigCache")]
        public ActionResult UserShow()
        {
            return View();
        }       
        public ActionResult GetAllUserInfos()
        {
            //page:1
            //rows:20
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            string serachName = Request["searchName"];
            var para = new QueryParam()
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Total = total,
                SearchName = serachName
            };
            var data = UserInfoBll.LoadPageUserInfos(para).Select(u => new { u.Id, u.UName, u.DelFlag, u.Tel, u.Name, u.Last_login_Time, u.State ,u.InTime,u.Login_Num}).ToList();        
            var result = new { total = para.Total, rows = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 管理用户伪删除
        [HttpPost]
        public ActionResult UsersDelete(string  ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var i in allIds)
            {
                idList.Add(int.Parse(i));
            }
         
           if (UserInfoBll.DeleteUsers(idList)>0)
            {
                return Content("ok"); 
            }
            return Content("no");
        }
        #endregion
        #region 修改用户
         [HttpGet]
        public ActionResult Edit(int id)
        {
            //int id = int.Parse(Request["id"]);
            ViewData.Model = UserInfoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult Edit(string UName,string Pwd,string Name,string Tel,int id)
        {          
    
         var userUpdate= UserInfoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
         if (userUpdate!=null)
         {
             userUpdate.UName =UName;
             if (!string.IsNullOrEmpty(Pwd))
             {
                 string MD5Pwd = Md5Helper.GetMd5(Pwd);
                 userUpdate.Pwd = MD5Pwd; 
             }            
             userUpdate.Name = Name;
             userUpdate.Tel = Tel;
             if (UserInfoBll.Update(userUpdate))
             {
                 return Content("ok");
             }
             else
             {
                 return Content("修改失败");
             }
         }
         return Content("数据为空");
        }
      
        #endregion
        #region 改变用户状态事件
        public ActionResult ChangeUsersState(int id)
        {
            UserInfo user = new UserInfo()
            {
                Id = id,
            };
            if (user != null)
            {
                var userState = UserInfoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前用户状态等于0的改变为1
                if (userState.State == "0")
                {
                    userState.State = "1";
                   
                        if (UserInfoBll.Update(userState))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content("fail");
                        }
                   
                }
                else
                {
                    userState.State = "0";
                    if (UserInfoBll.Update(userState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }

            }
            else
            {
                return Content("empty");
            }

        }
        #endregion
        #region 添加管理员
        public ActionResult AddAdminUser(FormCollection form)
        {
            string pwd=form["pwd"];
            string MD5Pwd = Md5Helper.GetMd5(pwd);
            UserInfo user = new UserInfo() {
                UName = form["uName"].ToString(),
                Pwd=MD5Pwd,
                Name=form["name"].ToString(),
                Tel=form["tel"].ToString(),  
                InTime=DateTime.Now,
                Last_login_Time = DateTime.Now
            };
            if (user != null)
            {
                UserInfoBll.Add(user);

                string guid = Request["actionKey"];
                if (!string.IsNullOrEmpty(guid))
                {
                    var data = Common.CacheHelper.Get(guid) as List<ActionInfo>;
                    Common.CacheHelper.Remove(guid, data, DateTime.Now.AddDays(-1));
                }
                return Content("ok");
            }
            else {
                return Content("fail");
            }         
        }
        #endregion
        #region 设置角色
        public ActionResult SetRole(int id)
        {
            //int id = int.Parse(Request["id"] ?? "9");
            //id：就是要设置角色的用户的id int id
            var user = UserInfoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            ViewData.Model = user;
           // 前台需要所有的角色
            ViewBag.AllRoles = RoleInfoBll.LoadEntities(r => r.DelFlag == delNormal).ToList();
            //把要设置角色的用户的已经关联的角色的id拿到。
            ViewBag.ExistRolesId = user.RoleInfo.Select(r => r.Id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SetRole(int id, FormCollection collection)
        {
            int userId = id;
            //拿到所有选中的角色的id
            //ckb_1:1
            List<int> allSelectRoleIds = new List<int>();
            foreach (var key in Request.Form.AllKeys)
            {               
                if (key.StartsWith("ckb_"))
                {
                    allSelectRoleIds.Add(int.Parse(Request[key]));
                }
            }
            UserInfoBll.SetUserRole(userId, allSelectRoleIds);
            return Content("ok");
        }
        #endregion
        #region 设置用户的权限
        public ActionResult SetAction(int id)
        {                     
            //TODO 根据id查出当前用户
            var user = UserInfoBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            //把所有的权限发送到前台
            ViewBag.AllActions = ActionInfoBll.LoadEntities(a => a.DelFlag == delNormal).ToList();
            //把当前用户所有的特殊权限查询出来，发送到前台
            ViewBag.AllExistActions = user.R_UserInfo_ActionInfo.ToList();
            return View(user);
        }
        [HttpPost]
        public ActionResult SetAction(int id, int actionId, string isPass)
        {
            //在中间表通过用户表id和权限表id查询对应的数据权限
            var temp = R_UserInfo_ActionInfoBll.LoadEntities(r =>
                               r.UserInfoId == id
                               &&
                               r.ActionInfoId == actionId).FirstOrDefault();

            if (isPass == "true" || isPass == "false")//点击了允许按钮
            {
                bool pass = isPass == "true";
                if (temp != null)
                {
                    temp.IsPass= pass;
                    R_UserInfo_ActionInfoBll.Update(temp);
                }
                else//无则添加之
                {
                    R_UserInfo_ActionInfo rUserInfoActionInfo = new R_UserInfo_ActionInfo();
                    rUserInfoActionInfo.UserInfoId= id;
                    rUserInfoActionInfo.ActionInfoId = actionId;
                    rUserInfoActionInfo.IsPass = pass;
                    R_UserInfo_ActionInfoBll.Add(rUserInfoActionInfo);
                }
            }
            else //删除
            {
                //如果有中间表数据，修改true
                if (temp != null)
                {
                    R_UserInfo_ActionInfoBll.Delete(temp);
                }
            }
            return Content("ok");
        }
        #endregion
        #region 加载用户的所有的菜单
        //public ActionResult LoadUserMenus()
        //{
        //    //根据当前登录用户的id查出当前用户的信息
        //    var user = UserInfoBll.LoadEntities(u => u.Id==CurrentAdmin.Id).FirstOrDefault();

        //    short menuType = (short)Models.Enum.ActionInfoTypeEnum.MenuAction;
        //    //拿到用户的所有的权限
        //    //通过角色拿到所有 的权限
        //    var temp = (from r in user.RoleInfo
        //                from a in r.ActionInfo                        
        //               where a.ActionType == menuType
        //                where a.DelFlag == delNormal
        //                select a).ToList();
        //    //通过特殊中间表拿到权限数据  
        //    var userTemp = (from r in user.R_UserInfo_ActionInfo
        //                    where r.IsPass == true
        //                    select r.ActionInfo).ToList();
        //    //把所有角色权限里面的特殊拒绝的权限清除掉
        //    temp.AddRange(userTemp);

        //    //TO移除拒绝的
        //    var userNoPass = (from r in user.R_UserInfo_ActionInfo
        //                      where r.IsPass == false
        //                      select r.ActionInfo.Id).ToList();
        //    //拒绝的权限数据里面不包含所有权限的id将其查询出来
        //    var result = (from t in temp
        //                  where !userNoPass.Contains(t.Id)
        //                  where t.ActionType == menuType
        //                  select t).ToList();

        //    ////{ icon: '/Content/images/3DSMAX.png', title: '用户列表', url: '/UserDemo/Index' }
        //    var data = (from r in result
        //                select new {  title = r.ActionName, url = r.Url }).ToList();

        //    return Json(data, JsonRequestBehavior.AllowGet);


        //}
        #endregion
    }
}
