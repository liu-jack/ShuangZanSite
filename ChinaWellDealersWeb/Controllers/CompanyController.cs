using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models.Params;
using Models;
namespace ShuangZan.Web.Controllers
{
    //厂商管理控制器
    [LoginCheckFilterAttribute(IsCheck = false)]
    public class CompanyController : BaseController
    {
       
        public ICompanyBll CompanyBll { get; set; }
        public ICompanyGameBll CompanyGameBll { get; set; }
        public IRechargeBll RechargeBll { get; set; }//厂商充值明细
        #region 厂商信息管理
        [OutputCache(Duration = 5)]
        public ActionResult Index()
        {
            return View();
        }     
        public ActionResult GetAllCompanyInfos()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var cpy = CompanyBll.LoadEntities(C => C.DelFlag == null||C.DelFlag==0).Select(c => new { c.Id, c.UName, c.DelFlag, c.SystemName, c.Email, c.CompanyName, c.Phone, c.InTime, c.State, c.Rec_Index, c.Rec_Index_Time, c.Rec_Forum_Index, c.Rec_Forum_Index_Time });
            var cpygame = CompanyGameBll.LoadEntities(c => c.Id > 0);
            var rech = RechargeBll.LoadEntities(r => r.Id > 0);
            var query = from c in cpy
                        let Games = (from g in cpygame where c.Id == g.CompanyId select g).Count()//id对应的游戏总个数                   
                        let Total = (from r in rech where c.Id == r.CompanyId select (int?)r.Num - r.Used).Sum()//剩下的开服条数
                                      
                        select new
                        {
                            c.Id,
                            c.UName,
                            c.DelFlag,
                            c.SystemName,
                            c.Email,
                            c.CompanyName,
                            c.Phone,
                            c.InTime,
                            c.State,
                            Games = Games,
                            Total = Total ?? 0,
                            c.Rec_Index,
                            c.Rec_Index_Time,
                            c.Rec_Forum_Index,
                            c.Rec_Forum_Index_Time,
                            //  g.CompanyId,                       
                        };
            string uName = Request["uName"];
            string systemName = Request["systemName"];
            string companyName = Request["companyName"];
            string rec = Request["rec"];
            string rec_index = Request["rec_index"];
            if (!string.IsNullOrEmpty(uName))
            {
                query = query.Where(q => q.UName.Contains(uName));
            }
            if (!string.IsNullOrEmpty(systemName))
            {
                query = query.Where(q => q.SystemName.Contains(systemName));
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                query = query.Where(q => q.CompanyName.Contains(companyName));
            }
            if (!string.IsNullOrEmpty(rec))//频道
            {
                query = query.Where(q => q.Rec_Forum_Index.Contains(rec));
            }
            if (!string.IsNullOrEmpty(rec_index))//shouye
            {
                query = query.Where(q => q.Rec_Index.Contains(rec_index));
            }
            total = query.Count();
            var allData = query.OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 改变厂商管理的状态事件
        public ActionResult ChangeCompanyState(int id)
        {
            Company cpy = new Company()
             {
                 Id = id,
             };
            if (cpy != null)
            {
                var cpyState = CompanyBll.LoadEntities(C => C.Id == id).FirstOrDefault();
                //当前用户状态等于0的改变为1
                if (cpyState.State == 0)
                {
                    cpyState.State = 1;

                    if (CompanyBll.Update(cpyState))
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
                    cpyState.State = 0;
                    if (CompanyBll.Update(cpyState))
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
        #region 厂商管理的删除伪事件
        public ActionResult CompanyDelete(int id)
        {
            var cpy = CompanyBll.LoadEntities(c => c.Id == id).FirstOrDefault();
            cpy.DelFlag = (short)Models.Enum.DelFlagEnum.Deleted;
            if (CompanyBll.Update(cpy))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
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
            //if (CompanyBll.DeleteCompanys(idList) > 0)
            //{
            //    return Content("ok");
            //}
            //else {
            //    return Content("no");
            //}        
        }
        #endregion
        #region 厂商管理用户的编辑事件
        public ActionResult Edit(int id)
        {
            // int id
            //int id = int.Parse(Request["id"] ?? "1");
            ViewData.Model = CompanyBll.LoadEntities(c => c.Id == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form, int id)
        {
            int sex = int.Parse(Request["sex"].ToString());
           
           var  cpy = CompanyBll.LoadEntities(u => u.Id == id).FirstOrDefault();
            if (cpy != null)
            {

                if (form["logoImg"] == null)
                {
                    cpy.Email = form["email"];
                    cpy.SystemName = form["systemName"];
                    cpy.WebSite = form["webSite"];
                    cpy.CompanyName = form["cpyName"];
                    cpy.Phone = form["phone"];
                    cpy.Address = form["address"];
                    cpy.CopanyMsg = form["companyMsg"];
                    cpy.Contacts = form["contacts"];
                    cpy.Office = form["office"];
                    cpy.Moblie = form["moblie"];
                    cpy.QQ = form["qq"];
                    cpy.Sex = sex;
                    cpy.InTime = DateTime.Now;
                }
                else
                {

                    cpy.Email = form["email"];
                    cpy.SystemName = form["systemName"];
                    cpy.WebSite = form["webSite"];
                    cpy.CompanyName = form["cpyName"];
                    cpy.Phone = form["phone"];
                    cpy.Address = form["address"];
                    cpy.CopanyMsg = form["companyMsg"];
                    cpy.Contacts = form["contacts"];
                    cpy.Office = form["office"];
                    cpy.Moblie = form["moblie"];
                    cpy.QQ = form["qq"];
                    cpy.Sex = sex;
                    cpy.Logo = form["logoImg"];
                    cpy.InTime = DateTime.Now;
                }


                if (CompanyBll.Update(cpy))
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
        #region 图片上传的方法

        public ActionResult Upload()
        {

            string msg = string.Empty;
            string filePath = "/Content/CompanyImg/";
            HttpPostedFileBase file = Request.Files["iconFile"];
            string str = Common.UploadImgs.CommonUploadImg(file, out msg, filePath);
            return Content(str);          
        }

        #endregion
        #region 厂商推荐
        public ActionResult Rec_Company()
        {
            return View();
        }
        public ActionResult GetRecCompanyInfos()
        {
            int pageSize = Request["rows"] == null ? 20 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;
            var cpy = CompanyBll.LoadEntities(C => (C.DelFlag == null || C.DelFlag == 0) && C.State == 1).Select(c => new { c.Id, c.UName, c.DelFlag, c.SystemName, c.Email, c.CompanyName, c.Phone, c.InTime, c.State, c.Rec_Index, c.Rec_Index_Time, c.Rec_Forum_Index, c.Rec_Forum_Index_Time });
            var cpygame = CompanyGameBll.LoadEntities(c => c.Id > 0);
            var rech = RechargeBll.LoadEntities(r => r.Id > 0);
            var query = from c in cpy
                        let Games = (from g in cpygame where c.Id == g.CompanyId select g).Count()//id对应的游戏总个数                   
                        let Total = (from r in rech where c.Id == r.CompanyId select (int?)r.Num - r.Used).Sum()//开服条数
                        // join g in cpygame on   c.Id equals g.CompanyId                  
                        select new
                        {
                            c.Id,
                            c.UName,
                            c.DelFlag,
                            c.SystemName,
                            c.Email,
                            c.CompanyName,
                            c.Phone,
                            c.InTime,
                            c.State,
                            Games = Games,
                            Total = Total ?? 0,
                            c.Rec_Index,
                            c.Rec_Index_Time,
                            c.Rec_Forum_Index,
                            c.Rec_Forum_Index_Time,
                            //  g.CompanyId,                       
                        };
            string uName = Request["uName"];
            string systemName = Request["systemName"];
            string companyName = Request["companyName"];
            string rec = Request["rec"];
            string rec_index = Request["rec_index"];
            if (!string.IsNullOrEmpty(uName))
            {
                query = query.Where(q => q.UName.Contains(uName));
            }
            if (!string.IsNullOrEmpty(systemName))
            {
                query = query.Where(q => q.SystemName.Contains(systemName));
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                query = query.Where(q => q.CompanyName.Contains(companyName));
            }
            if (!string.IsNullOrEmpty(rec))//频道
            {
                query = query.Where(q => q.Rec_Forum_Index.Contains(rec));
            }
            if (!string.IsNullOrEmpty(rec_index))//shouye
            {
                query = query.Where(q => q.Rec_Index.Contains(rec_index));
            }
            total = query.Count();
            var allData = query.OrderByDescending(d => d.Id)
                                        .Skip(pageSize * (pageIndex - 1))
                                        .Take(pageSize).ToList();
            var result = new { total = total, rows = allData };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 关于厂商频道及首页推荐
        public ActionResult Rec_CpyChangeState(int id, string recParam)
        {
            //id: strId, recParam: "rec"
            Company cpy = new Company()
             {
                 Id = id,
             };
            if (recParam == "rec")
            {
                var cpyState = CompanyBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (cpyState.Rec_Index == "0")
                {
                    cpyState.Rec_Index = "1";
                    cpyState.Rec_Index_Time = DateTime.Now;
                    if (CompanyBll.Update(cpyState))
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
                    cpyState.Rec_Index = "0";
                    cpyState.Rec_Index_Time = DateTime.Now;
                    if (CompanyBll.Update(cpyState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            else if (recParam == "recForum")
            {
                var cpyState = CompanyBll.LoadEntities(u => u.Id == id).FirstOrDefault();
                //当前状态等于0的改变为1
                if (cpyState.Rec_Forum_Index == "0")
                {
                    cpyState.Rec_Forum_Index = "1";
                    cpyState.Rec_Forum_Index_Time = DateTime.Now;
                    if (CompanyBll.Update(cpyState))
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
                    cpyState.Rec_Forum_Index = "0";
                    cpyState.Rec_Forum_Index_Time = DateTime.Now;
                    if (CompanyBll.Update(cpyState))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }
                }
            }
            return Content("no");
        }
        #endregion
        #region 关于厂商一键清空设置的频道及首页状态事件
        public ActionResult Clear_CpyState(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Content("empty");
            }
            string[] allIds = ids.Split(',');
            List<int> idList = new List<int>();
            foreach (var item in allIds)
            {
                idList.Add(int.Parse(item));
            }
            if (CompanyBll.ClearSetState(idList) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 厂商对应(充值表)开服条数列表及模糊搜索      
        public ActionResult RechargeTotal(DateTime?time)
        {
            int id = int.Parse(Request["id"]);
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");           
            int total = 0;
            string comboName = Request["comboName"];          
            var param = new RechargeParam()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                CompanyId = id,
                comboName = comboName,
                InTime = time
            };
            ViewData.Model = CompanyBll.LoadEntities(r => r.Id == id).FirstOrDefault();
            var list = RechargeBll.LoadRechargeTotal(param).ToList();
            ViewBag.CurrentId = id;
            ViewData["Recharge"] = list;
            ViewData["pageCount"] = param.Total;
            ViewData["pageSize"] = param.PageSize;
            ViewData["pageIndex"] = param.PageIndex;
            return View();
        }
      
        public ActionResult CompanyGameNum(int id)
        {
            int pageIndex = int.Parse(Request["pageIndex"]??"1");
            int pageSize = int.Parse(Request["pageSize"] ?? "20");
            string gameName = Request["gamename"];
            string type = Request["type"];
            int total = 0;
            var  param=new CpyGameParam(){
            PageIndex=pageIndex,
            PageSize=pageSize,
            Total=total,
            GameName=gameName,
            CompanyId=id,
            Type=type            
            };
            var list = CompanyGameBll.LoadCpyGame(param).ToList();
                     
            ViewData["CompanyGame"] = list;
            ViewData["pageCount"] = param.Total;
            ViewData["pageSize"] = param.PageSize;
            ViewData["pageIndex"] =param.PageIndex;
            ViewBag.CurrentId = id;
            ViewData.Model = CompanyBll.LoadEntities(r => r.Id == id).FirstOrDefault();
            return View();
        }
        #endregion
        #region 删除厂商游戏
        public ActionResult DelCpyGame(int id)
        {
            CompanyGame game = new CompanyGame() { Id = id };
            if (CompanyGameBll.Delete(game))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 删除厂商充值
        public ActionResult DelRecharge(int id)
        {
            Recharge rec = new Recharge() { Id = id };
            if (RechargeBll.Delete(rec))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 厂商添加游戏
        public ActionResult AddCpyName(int companyId, FormCollection form)
        {
            CompanyGame cpyGame = new CompanyGame()
            {
                CompanyId = companyId,
                GameName = form["gameName"],
                Type = form["type"],
                Url = form["url"],
                InTime = DateTime.Now
            };
            if (CompanyGameBll.Add(cpyGame) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 厂商修改游戏

        public ActionResult EditcpyNames(int id, FormCollection form)
        {
            CompanyGame cpy = new CompanyGame() { Id = id };
            cpy = CompanyGameBll.LoadEntities(c => c.Id == id).FirstOrDefault();
            cpy.GameName = form["gameName"];
            cpy.Type = form["type"];
            cpy.Url = form["url"];
            if (CompanyGameBll.Update(cpy))
            {
                return Content("ok");
            }
            else
            {

                return Content("no");
            }

        }
        public ActionResult CpyNameEdit(int id)
        {

            ViewData.Model = CompanyGameBll.LoadEntities(r => r.Id == id).FirstOrDefault();
            return View();
        }
        #endregion
        #region 设置开服条数
        public ActionResult SetRechargeTotal(int companyId, FormCollection form)
        {
            Recharge recharge = new Recharge()
            {
                CompanyId = companyId,
                ComboName = form["comboName"],
                Money = int.Parse(form["money"]),
                Num = int.Parse(form["num"]),
                Remark = form["remark"],
                InTime = DateTime.Now
            };
            if (RechargeBll.Add(recharge) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
    }
}
