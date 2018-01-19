using BLL;
using Common;
using IBLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.IO;
using System.Net.Mail;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
namespace ShuangZan.Web.Controllers
{
     [LoginCheckFilterAttribute(IsCheck = false)]
    public class FrontCompanyController : CpyBaseController//Controller//   
    {
        // GET: /FrontCompany/
        #region 给属性注册对象             
        public ICompanyBll CompanyBll { get; set; }
        public IRechargedUsedBll RechargedUsedBll { get; set; }
        public IRechargeBll RechargeBll { get; set; }
        public IOpenServiceBll OpenServiceBll { get; set; }
        public IGameBll GameBll { get; set; }
        public INewsBll NewsBll { get; set; }
        public IPackageBll PackageBll { get; set; }//礼包
        public IPackageCardBll PackageCardBll { get; set; }
        public IAuditLogBll AuditLogBll { get; set; }
        #endregion
        #region 默认的厂商主页：消息通知
        [OutputCache(CacheProfile = "ItemConfigCache")]
        public ActionResult Company_Index()
        {
            //礼包审核是1、新闻审核2 、开服审核0、对应AuditLog表中的type表类型，tableId为当前表id
            var currenCpy = GetCurrentUser();
            var listLog = AuditLogBll.LoadEntities(a => true).ToList()
                 .Where(a => a.CompanyId == currenCpy.Id)
                 .OrderByDescending(a => a.InTime)
                .Select(a => new AuditLog() { Title = a.Title, Reason = a.Reason, InTime = a.InTime, CompanyId = a.CompanyId, TableId = a.TableId, Type = a.Type }).Take(5)
                                             .ToList();
            ViewData["listLog"] = listLog;

            return View();
        }
        #endregion            
        #region 拿到当前厂商用户的信息
        public Company GetCurrentUser()
        {
            //Done 根据guid拿到当前登录的用户。
            string guid = Request["cpyUserId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var loginAdmin = Common.CacheHelper.Get(guid) as Company;
            if (loginAdmin != null)
            {
                ViewData.Model = loginAdmin;
            }
            return ViewData.Model as Company;
        }
        #endregion            
        #region 厂商的主页
        public ActionResult Index()
        {          
                 var currentCpy= GetCurrentUser();
                 ViewData.Model = currentCpy as Company;
             //--------------------当前的厂商获取置顶条数------------------------
            //Hack  区分置顶和开服的剩余条数                   
            ViewBag.recCount =RechargeBll.TopUsedNum(currentCpy.Id);
            //--------------------当前的厂商获取全天开服条数------------------------
            var serviceCount = RechargeBll.allDayUsedNum(currentCpy.Id);
            ViewBag.serviceCount = serviceCount;           
            return View();
        }
        #endregion      
        #region 修改当前厂商的邮箱
        public ActionResult ModifyEmail()
        {
            return View();
        }
        public ActionResult UpdateEmail(FormCollection form)
        {
            //1.采集用户的输入
            string oldemail = form["oldemail"].Trim();
            string newemail = form["newemail"].Trim();
            Company cpy = GetCurrentUser();
            cpy = CompanyBll.LoadEntities(c => c.Id == cpy.Id).FirstOrDefault();
            if (cpy.Email==oldemail)
            {
                //3.校验旧密码是否正确
                //根据guid拿到当前登录的用户
               
                if (cpy.Email == newemail)
                {
                    return Content("exist");
                }
                else
                {
                    //4.修改邮箱
                    cpy.Email = newemail;
                    if (CompanyBll.Update(cpy))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("fail");
                    }

                }              
            }
            return Content("different");

        }
        #endregion
        #region 修改厂商密码
        public ActionResult EditCpyPwdPage()
        {
            return View();
        }
        public ActionResult EditCpyPwd()
        {
            //1.采集用户的输入
            string oldPwd = Request.Form["oldPwd"].Trim() as string;
            //  string Md5oldPwd = Md5Helper.GetMd5(oldPwd);
            string newPwd = Request.Form["newPwd"].Trim() as string;
            string confirmPwd = Request.Form["confirmPwd"].Trim() as string;
               Company cpy = GetCurrentUser();
            // string Md5confirmPwd = Md5Helper.GetMd5(confirmPwd);
            //2.校验两次输入的新密码是否正确         
                //3.校验旧密码是否正确
                //根据guid拿到当前登录的用户             
                cpy = CompanyBll.LoadEntities(c => c.Id == cpy.Id).FirstOrDefault();
                if (cpy.Pwd != oldPwd)
                {
                    return Content("different");
                }
                else
                {

                    cpy.Pwd = confirmPwd;
                    if (CompanyBll.Update(cpy))
                    {
                       
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }                         
        }
        #endregion
        #region 修改头像
        public ActionResult ModifyHeadImg()
        {
            var currentCpy = GetCurrentUser();
            ViewData.Model = CompanyBll.LoadEntities(c => c.Id == currentCpy.Id).FirstOrDefault();              
            return View();
        }
        public ActionResult UpdateHeadImg(FormCollection form)
        {
            var bigFile = Request.Files["headImg"];
            string imgPath = form["real_headimg"];
            var currentCpy = GetCurrentUser();
            if (bigFile.ContentLength>0)
            {
                // var file = Request.Files[0];
                //拿到文件的扩展名
                string extName = Path.GetExtension(bigFile.FileName);
             if (bigFile.ContentType == "image/jpeg" || bigFile.ContentType == "image/jpg" || bigFile.ContentType == "image/png"
                )
                {
                    Random r = new Random();
                    //给文件取新名字
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd") + r.Next(100, 1000) + extName;
                    //用户文件夹的物理路径（绝对路径）                   
                    String virthPath = "/Content/UserImg/" + fileName;
                    String name = Server.MapPath(virthPath);
                    bigFile.SaveAs(name);
                   
                  var  cpy  =CompanyBll.LoadEntities(c => c.Id == currentCpy.Id).FirstOrDefault();
                        cpy.Head =fileName;
                        CompanyBll.Update(cpy);
                    return Content(fileName);//返回文件名
                }
                else
                {
                    return Content("typeError");
                }
            }
            else if (imgPath != "")
            {
                var cpy = CompanyBll.LoadEntities(c => c.Id == currentCpy.Id).FirstOrDefault();
               // string[] data =;
                cpy.Head = imgPath.Substring(13);
                CompanyBll.Update(cpy);
                return Content(imgPath.Substring(13));
            }
            else
            {
                return Content("empty");
            }
           
        }
        #endregion
        #region 修改厂商平台信息
        public ActionResult ModifyCpyInfo(int companyId)
        {
            ViewData.Model = CompanyBll.LoadEntities(c => c.Id == companyId).FirstOrDefault();
            return View();
        }
        public ActionResult UpdateCpyInfo(int id, string systemname, string website, string companyname, string phone, string address)
        {
            var cpy = CompanyBll.LoadEntities(c => c.Id == id).FirstOrDefault();
            cpy.SystemName = systemname;
            cpy.WebSite = website;
            cpy.CompanyName = companyname;
            cpy.Phone = phone;
            cpy.Address = address;
            cpy.State = 2;
            CompanyBll.Update(cpy);
            return Json(cpy, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 修改厂商个人信息
        public ActionResult CpyPersonalInfo(int companyId)
        {
          ViewData.Model=CompanyBll.LoadEntities(c => c.Id == companyId).FirstOrDefault();
            return View();
        }
        public ActionResult UdateCpyPersonalInfo(int id, string contacts, string office, string mobile, string qq, int sex)
        {
            var cpy = CompanyBll.LoadEntities(c => c.Id == id).FirstOrDefault();
            cpy.Contacts = contacts;
            cpy.Office = office;
            cpy.Moblie = mobile;
            cpy.QQ = qq;
            cpy.Sex = sex;
            CompanyBll.Update(cpy);
            return Json(cpy, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 消费明细表
        //三表关联 充值表跟充值剩余表管理  取充值剩余表的时间、充值表的时间（买），然后是 充值剩余表和开服表关联
        //过滤的条件是充值表中的companyId==开服表中的companyId  取值是开服表游戏名、开服表时间、开服表的服务器名称
        [OutputCache(Duration = 20)]
        public ActionResult ExpenseDetail()
        {
            return View();
        }

        public JsonResult GetExpenseDetail(int limit, int offset, string gameName, DateTime? startday, DateTime? endday)
        {
            //
            string guid = Request["cpyUserId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var currentCpy = Common.CacheHelper.Get(guid) as Company;
            var Rused = RechargedUsedBll.LoadEntities(ru => true);
            var recharge = RechargeBll.LoadEntities(r => r.Id > 0);
            var service = OpenServiceBll.LoadEntities(s => s.Id > 0);
            var data = from ru in Rused
                       join r in recharge on ru.RechargeId equals r.Id
                       join s in service on ru.OpenServiceId equals s.Id
                       where r.CompanyId == s.CompanyId && r.CompanyId == currentCpy.Id
                       //intime  消费日期    buytime购买日期, BuyTime = r.InTime 
                       select new { InTime = ru.InTime, Id = s.Id, GameName = s.GameName, ServerName = s.ServerName, StartTime = s.StartTime, BuyTime = r.InTime ,Rec_hot=s.Rec_Hot};
            var total = data.Count();
            if (!string.IsNullOrEmpty(gameName))
            {
                data = data.Where(d => d.GameName.Contains(gameName));
            }
            if (startday.HasValue||endday.HasValue)
            {
                endday = endday.Value.AddDays(1);
                data = data.Where(d => d.InTime >=startday && d.InTime <= endday);
             
            }
            var rows = data.OrderByDescending(d => d.InTime)            
                       .Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 厂商充值记录
        [OutputCache(Duration = 20)]
        public ActionResult Recharge_Record()
        {
            return View();
        }
        /// <summary>
        /// 当前厂商的companyid对应充值表中的数据（充值记录）
        /// </summary>
        /// <param name="limit">//页面大小</param>
        /// <param name="offset">页码</param>
        /// <returns></returns>
        [OutputCache(Duration = 5)]
        public ActionResult GetRecharge_Record(int limit, int offset, DateTime? startday, DateTime ?endday)
        {           
            string guid = Request["cpyUserId"];
        
            var currentCpy = Common.CacheHelper.Get(guid) as Company;
            var recharge = RechargeBll.LoadEntities(r => r.CompanyId == currentCpy.Id).Select(r => new { r.Id, r.CompanyId, r.Money, r.Num, Total = (r.Num - r.Used), r.InTime, r.Remark,r.ComboName });
            if (startday.HasValue || endday.HasValue)
            {
                endday = endday.Value.AddDays(1);
                recharge = recharge.Where(d => d.InTime >startday && d.InTime <= endday);
             
            }              
               var total = recharge.Count();
                  var rows = recharge.OrderByDescending(d => d.InTime)         
                  .Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 查出游戏表中所有的游戏名称
        public ActionResult AllGameName(string gameName)
        {
            //数据匹配  有数据 不提示、不匹配直接提示

            var allGameName = GameBll.LoadEntities(g => g.Name == gameName).Select(g => new { g.Name }).FirstOrDefault();

            if (allGameName != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }    
        public ActionResult GameName()
        {
            var allGameName = GameBll.LoadEntities(g => true).Select(g => new { g.Name });
            return Json(allGameName, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  <根据游戏名称与开服时间选择合适的礼包>
        public ActionResult GetPackage(string gameName,DateTime? startday)
         {
             var  cpy=GetCurrentUser();
           var  data=  PackageBll.LoadEntities(p=>p.CompanyId==cpy.Id&&p.GameName==gameName&&p.StartDate<=startday&&p.EndDate>=startday&&(p.State=="1"||p.State=="3")).Select(p=>new{p.Id,p.GameName,p.StartDate,p.EndDate,p.ServerName,p.Type,p.GiftName});
             return Json(data,JsonRequestBehavior.AllowGet);
         } 
	#endregion
        #region 发布开服       
        public ActionResult SendOpenService()
        {
            //---------------------查出当前厂商曾经发布过的开服游戏名称------------------------
            var currentCpy = GetCurrentUser();       
            var goGame = OpenServiceBll.MyHistoryGameName(currentCpy.Id);
            ViewBag.goGame = goGame;
            return View();
        }
        public ActionResult AddOpenService()
        {
            var currenCpy = GetCurrentUser();
            string type = Request["rec_hot"];
          
            OpenService service = new OpenService()
            {
                CompanyId = currenCpy.Id,
                GameName = Request["gamename"],
                ServerName = Request["serverName"],
                Url = Request["url"],
                PackageId = int.Parse(Request["packageId"].ToString()),
                Rec_Hot = Request["rec_hot"],
                InTime = DateTime.Now,
                Rec_Datetime=DateTime.Now,
                State="2",
                CheckName=null,
                StartTime = DateTime.Parse(Request["starttime"])
            };
            Recharge recharge;
            if (type == "2")//全天开服
            {
                recharge = RechargeBll.LoadEntities(r => r.CompanyId == currenCpy.Id && r.Num > r.Used && r.ComboName == "全天置顶").OrderBy(r => r.InTime).FirstOrDefault();
                if (recharge == null)
                {
                    return Content("noNum,全天开服点数不足，请先充值");
                }
                if (OpenServiceBll.Add(service) != null)
                {
                    RechargedUsed ru = new RechargedUsed() { RechargeId = recharge.Id, OpenServiceId = service.Id, InTime = DateTime.Now };
                    RechargedUsedBll.Add(ru);
                    recharge.Used = recharge.Used + 1;
                    RechargeBll.Update(recharge);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }//置顶开服
            else if (type == "1")
            {
                recharge = RechargeBll.LoadEntities(r => r.CompanyId == currenCpy.Id && r.Num > r.Used && r.ComboName != "全天置顶").OrderBy(r => r.InTime).FirstOrDefault();
                if (recharge == null)
                {
                    return Content("noNum,点数不足，请先充值");
                }
                if (OpenServiceBll.Add(service) != null)
                {
                    RechargedUsed ru = new RechargedUsed() { RechargeId = recharge.Id, OpenServiceId = service.Id, InTime = DateTime.Now };
                    RechargedUsedBll.Add(ru);
                    recharge.Used = recharge.Used + 1;
                    RechargeBll.Update(recharge);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else {
                return Content("typeError,提示：请先选择开服类型");
            }
            
          
        }
        #endregion      
        #region  厂商发布新闻
        public ActionResult AddCpyNews()
        {

            return View();
        }
        [ValidateInput(false)]
        public ActionResult AddCpyNewsInfo()
        {
            // var currentCpy= GetCurrentUser();
            string guid = Request["cpyUserId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var currentCpy = Common.CacheHelper.Get(guid) as Company;
            try
            {
                News news = new News()
                {
                    CompanyId = currentCpy.Id,
                    Title = Request["title"],
                    Game = Request["gameName"],
                    Kewords = Request["keyword"],
                    Type = Request["type"],
                    Memo = Request["memo"],
                    Msg = Request["msg"],
                    State = "2",
                    InTime = DateTime.Now,
                    Rec_Forum_Index = "0",
                    Rec_Forum_Time = DateTime.Now,
                    Rec_Hot_Index = "0",
                    Rec_Hot_Time = DateTime.Now,
                    Editor = null,
                    EditTitle = null,
                    CheckName = null,
                    AddedBy = null,
                    Source = null,
                    Views = 0,
                    LeaveMsgId = 0
                };
                if (NewsBll.Add(news) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("发布新闻时抛异常啦" + ex.Message);
            }

        }
        #endregion
        #region 管理新闻
        public ActionResult CpyNews()
        {
            return View();
        }
        public ActionResult GetCpyNews(int limit, int offset, string title, DateTime? startday, DateTime? endday)
        {
            var currentCpy = GetCurrentUser();
            var data = NewsBll.LoadEntities(n => n.CompanyId == currentCpy.Id).Select(n => new { n.Title, n.InTime, n.Kewords, n.State, n.Id });
          
            if (!string.IsNullOrEmpty(title))
            {
                data = data.Where(d => d.Title.Contains(title));
            }
            if (startday.HasValue || endday.HasValue)
            {
                endday = endday.Value.AddDays(1);
                data = data.Where(d => d.InTime >= startday && d.InTime <= endday);
            }
            var total = data.Count();
            var rows = data.OrderByDescending(d =>d.InTime)
           
                    .Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 查看新闻详情
        [OutputCache(Duration = 10)]
        public ActionResult SeeCpyNewsDetail(int id)
        {
            ViewData.Model = NewsBll.LoadEntities(n => n.Id == id).FirstOrDefault();
            return View();
        }
        #endregion
        #region 删除新闻
        public ActionResult CpyNewsDel(int id)
        {

            if (NewsBll.NewsDelete(id))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion     
        #region 编辑新闻
        public ActionResult CpyNewsEdit(int id)
        {
            var currentCpy = GetCurrentUser();
            ViewData.Model = NewsBll.LoadEntities(n => n.Id == id && n.CompanyId == currentCpy.Id).FirstOrDefault();
            return View();
        }
        [ValidateInput(false)]
        public ActionResult CpyNewsInfoEdit(int newsId, int companyId)
        {

            var news = NewsBll.LoadEntities(n => n.Id == newsId).FirstOrDefault();
            news.CompanyId = int.Parse(Request["companyId"].ToString());
            news.Title = Request["title"];
            news.Game = Request["gameName"];
            news.Kewords = Request["keyword"];
            news.Type = Request["type"];
            news.Memo = Request["memo"];
            news.Msg = Request["msg"];
            news.State = "2";
            if (NewsBll.Update(news))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion    
        #region 管理开服     
        public ActionResult CpyOpenService()
        {
            return View();
        }    
        public ActionResult Get_CpyOpenService(int limit, int offset, string gameName, DateTime? startday, DateTime ?endday)
        {
            var currentCpy = GetCurrentUser();
            var allService = OpenServiceBll.LoadEntities(s => s.Id > 0).Select(s => new {s.Id, s.PackageId, s.StartTime, s.GameName, s.State, s.ServerName, s.CompanyId, s.Rec_Hot ,s.InTime});
            var allpackage = PackageBll.LoadEntities(p => p.Id > 0).Select(p => new { p.Id, p.GiftName });
            var data = from s in allService
                       join p in allpackage on s.PackageId equals p.Id into ps
                       from psi in ps.DefaultIfEmpty()
                       where s.CompanyId == currentCpy.Id
                       select new {s.Id, s.StartTime, s.GameName, s.State, s.ServerName, GiftName=psi!=null?psi.GiftName:null, s.Rec_Hot ,s.InTime};
            if (!string.IsNullOrEmpty(gameName))
            {
                data = data.Where(d => d.GameName.Contains(gameName));
            }
         
            if (startday.HasValue || endday.HasValue)
            {
                endday = endday.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(00);
               // startday = startday.Value.AddSeconds(-1);
               data = data.Where(d => d.StartTime >= startday && d.StartTime <= endday);
            } 
            var total = data.Count();
            var rows = data.OrderByDescending(d => d.InTime)
               
                .Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 删除开服表
        public ActionResult OpenServiceDel(int id)
        {
             var  cpy=  GetCurrentUser();
             OpenService op = new OpenService()
             {
                 Id = id,
                 CompanyId=cpy.Id
             };
            //先删开服表
            if (OpenServiceBll.Delete(op))
            {
                //归还条数
                if (OpenServiceBll.ReturnServiceNum(id, cpy.Id))
                {
                    return Content("ok,恭喜：删除成功！条数已归还！");
                }
                else {
                    return Content("no,提示：开服表删除成功！条数归还失败！");
                }                                      
            }
            else
            {
                return Content("no,开服表删除失败！");
            }

        }
        #endregion
        #region  管理礼包
        public ActionResult CpyPackage()
        {
            return View();
        }
      
        public ActionResult Get_CpyPackage(int limit, int offset, string gameName, DateTime? startday, DateTime? endday)
        {
            var currentCpy = GetCurrentUser();
            var allCpy = CompanyBll.LoadEntities(c => c.Id > 0).Select(c => new { c.Id, c.CompanyName });
            var allPackage = PackageBll.LoadEntities(p => p.Id > 0).Select(p => new { p.CompanyId, p.Type, p.EndDate, p.StartDate, p.State, p.GameName, p.GiftName, p.ServerName, p.Id });
            var data = from c in allCpy
                       join p in allPackage on c.Id equals p.CompanyId
                       where p.CompanyId == currentCpy.Id
                       select new { p.EndDate, p.StartDate, p.State, p.GameName, p.GiftName, p.ServerName, p.Id, c.CompanyName,p.Type };
            if (!string.IsNullOrEmpty(gameName))
            {
                data = data.Where(d => d.GameName.Contains(gameName));
            }
            if (startday.HasValue || endday.HasValue)
            {
                endday = endday.Value.AddDays(1);
                data = data.Where(d => d.StartDate >= startday && d.EndDate <= endday);
            } 
            var total = data.Count();
            var rows = data.OrderByDescending(d => d.Id)
             
                 .Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 厂商删除礼包
        public ActionResult CpyPackageDel(int id)
        {
            var currentCpy = GetCurrentUser();
            if (PackageBll.CpyPackageDelete(id) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion
        #region 礼包详情  
        [OutputCache(Duration=10)]
        public ActionResult SeeCpyPackageDetail(int id)
        {
           // 礼包表和礼包卡号表连接查询、并且礼包不仅对应自己的id，也要对应当前厂商的id
            var currentCpy = GetCurrentUser();
            ViewData.Model = PackageBll.CpyPackageDetails(id, currentCpy)
                              .Select(p => new Package_Card()
                              {
                                  GiftType = p.GiftType,
                                  EndDate = p.EndDate,
                                  StartDate = p.StartDate,
                                  State = p.State,
                                  GameName = p.GameName,
                                  GiftName = p.GiftName,
                                  ServerName = p.ServerName,
                                  CompanyId = p.CompanyId,
                                  Id = p.Id,
                                  Url = p.Url,
                                  Memo = p.Memo,
                                  Msg = p.Msg,
                                  Card = p.Card
                              }).FirstOrDefault();
         List<Package_Card>  list =PackageBll.CpyPackageDetails(id, currentCpy)
                              .Select(p => new Package_Card()
                              {
                                  GiftType = p.GiftType,
                                  EndDate = p.EndDate,
                                  StartDate = p.StartDate,
                                  State = p.State,
                                  GameName = p.GameName,
                                  GiftName = p.GiftName,
                                  ServerName = p.ServerName,
                                  CompanyId = p.CompanyId,
                                  Id = p.Id,
                                  Url = p.Url,
                                  Memo = p.Memo,
                                  Msg = p.Msg,
                                  Card = p.Card
                              }).ToList();
         ViewData["currentAllCode"] = list;
         ViewBag.allCountCode = list.Count();          
            return View();
        }
        #endregion
        #region 添加礼包      
        public ActionResult SendPackage()
        {
            //-----------------查出当前厂商曾经发布过的礼包游戏---------------------------
            var currentCpy = GetCurrentUser();
           var goGame = OpenServiceBll.MyHistoryGameName(currentCpy.Id);           
            ViewBag.goGame = goGame;
            return View();
        }     
        public ActionResult AddPackage()
        {
            var currentCpy = GetCurrentUser();
            string type = Request["type"];
            string state;           
            if (type == "3")//独家礼包要审核的
            {                             
               state="2";    
            }else{
                state = "3";
            } 
            Package package = new Package();                  
                  package. CompanyId = currentCpy.Id;
                   package. GameName = Request["gamename"];
                   package. ServerName = Request["servername"];
                   package. Type = Request["type"];
                   package. GiftName = Request["name"];
                   package. StartDate = DateTime.Parse(Request["startdate"]);
                   package. EndDate = DateTime.Parse(Request["enddate"]);
                   package. Url = Request["url"];
                   package. Memo = Request["memo"];
                   package. Msg = Request["msg"];
                   package.State = state;
                   package. Rec = "0";
                   package. Rec_Hot = "0";
                   package. Rec_Index = "0";
                   package. Rec_Hot_Time = DateTime.Now;
                   package. Rec_Time = DateTime.Now;
                   package. Rec_Index_Time = DateTime.Now;
                   package. InTime = DateTime.Now;              
                     
            if (PackageBll.Add(package) != null)
            {
                //如果不为空、拿到其添加的id 据研究db.saveChange()后、EF会返回最新的自增id 
                //拆分字符串、拆分成一个一个逐个添加。
                string code = Request["code"];
                string[] splitCode = code.Trim().Split('\n');                          
                PackageCard card = null;           
                for (int i = 0; i < splitCode.Length; i++)
                {
                   card = new PackageCard();
                    card.PackageId=package.Id;
                    card.UserId=0;
                   card.InTime=DateTime.Now;
                    if (splitCode[i]==""||splitCode[i]==null)
	                {
		                continue;
	                }
                    card.Code=splitCode[i];                  
                    PackageCardBll.Add(card);
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        #endregion     
        #region 礼包修改
        public ActionResult CpyPackageEdit(int id)
        {
           // 除了卡号不能修改，其他的都可以改，提交之后，该重新审核的就重新审核，不用审核的直接过，按照原有的规则来。
            var currentCpy = GetCurrentUser();
            ViewData.Model = PackageBll.CpyPackageDetails(id, currentCpy)
                              .Select(p => new Package_Card()
                              {
                                  GiftType = p.GiftType,
                                  EndDate = p.EndDate,
                                  StartDate = p.StartDate,
                                  State = p.State,
                                  GameName = p.GameName,
                                  GiftName = p.GiftName,
                                  ServerName = p.ServerName,
                                  CompanyId = p.CompanyId,
                                  Id = p.Id,
                                  Url = p.Url,
                                  Memo = p.Memo,
                                  Msg = p.Msg,
                                  Card = p.Card
                              }).FirstOrDefault();
            List<Package_Card> list = PackageBll.CpyPackageDetails(id, currentCpy)
                            .Select(p => new Package_Card()
                            {
                                GiftType = p.GiftType,
                                EndDate = p.EndDate,
                                StartDate = p.StartDate,
                                State = p.State,
                                GameName = p.GameName,
                                GiftName = p.GiftName,
                                ServerName = p.ServerName,
                                CompanyId = p.CompanyId,
                                Id = p.Id,
                                Url = p.Url,
                                Memo = p.Memo,
                                Msg = p.Msg,
                                Card = p.Card
                            }).ToList();
            ViewData["currentAllCode"] = list;
            ViewBag.allCountCode = list.Count();         
            return View();
        }
        [HttpPost]
        public ActionResult CpyPackageEdit(int id,int companyId)
        {        
           string type= Request["type"];
           string state;
            Package package=  PackageBll.LoadEntities(p => p.Id == id&&p.CompanyId==companyId).FirstOrDefault();
            if (type == "3")
            {
                state = "2";
            }else{
            state="3";//无需审核
            }           
                package.GameName = Request["gamename"];
                package.ServerName = Request["servername"];
                package.Type = Request["type"];
                package.GiftName = Request["name"];
                package.StartDate = DateTime.Parse(Request["startdate"]);
                package.EndDate = DateTime.Parse(Request["enddate"]);
                package.Url = Request["url"];
                package.Memo = Request["memo"];
                package.Msg = Request["msg"];
                package.State = state;                                                                  
             if (PackageBll.Update(package))
             {
                 return Content("ok");
             }
             else {
                 return Content("no");
             }
           
        }
        #endregion
        #region 开服详情
        public ActionResult SeeCpyServersDetail(int id)
        {
            var currentCpy = GetCurrentUser();         
          
            ViewData.Model=OpenServiceBll.GetCpyServiceDetail(id,currentCpy)
                             .Select( s=>new OpenService_Package
                               {
                                   Id = s.Id,
                                   companyId = s.companyId,
                                   StartDate = s.StartDate,
                                   GameName = s.GameName,
                                   ServerName = s.ServerName,
                                   GiftName = s.GiftName!=null?s.GiftName:null,
                                   Rec_Hot = s.Rec_Hot,
                                   Url = s.Url
                               }).FirstOrDefault();                          
            return View();       
        }
        #endregion
        #region 购买账单详情参照表
        public ActionResult Cpy_Purchase()
        {
            return View();
        }
        #endregion
        #region 开服修改     
        /// <summary>
         /// 开服修改
         /// </summary>
         /// <param name="id">当前开服的id</param>
         /// <returns></returns>
        [HttpGet]
        public ActionResult ServiceEdit(int id)
        {
            var currentCpy = GetCurrentUser();
            var goGame = OpenServiceBll.MyHistoryGameName(currentCpy.Id);
            ViewBag.goGame = goGame;
            ViewData.Model = OpenServiceBll.GetCpyServiceDetail(id, currentCpy)
                             .Select(s => new OpenService_Package
                             {
                                 Id = s.Id,
                                 companyId=s.companyId,
                                 StartDate = s.StartDate,
                                 GameName = s.GameName,
                                 ServerName = s.ServerName,
                                 GiftName = s.GiftName,
                                 Rec_Hot = s.Rec_Hot,
                                 Url = s.Url
                             }).FirstOrDefault();     
            return View();
        }
         [HttpPost]
        public ActionResult ServiceEdit(int id,int companyId)
        {
            var op = OpenServiceBll.LoadEntities(s => s.Id == id && s.CompanyId == companyId).FirstOrDefault();                
               op. GameName = Request["gamename"];
               op. ServerName = Request["serverName"];
               op. Url = Request["url"];
               op. PackageId = int.Parse(Request["packageId"].ToString());
               op. Rec_Hot = Request["rec_hot"];             
               op. State = "2";
               op.StartTime = DateTime.Parse(Request["starttime"]);         
            if (OpenServiceBll.Update(op))
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
