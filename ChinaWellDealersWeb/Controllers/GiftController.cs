using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IBLL;
using Models;
namespace ShuangZan.Web.Controllers
{
    //礼品控制器
      [LoginCheckFilterAttribute(IsCheck = false)]
    public class GiftController : Controller
    {     
        // GET: /Gift/            
        public IGiftBll GiftBll { get; set; }
        IUserMessageBll UserMessageBll { get; set; }
        IUserAdressBll UserAdressBll { get; set; }
        #region 所有的礼品
        [OutputCache(CacheProfile="ItemConfigCache")]
        public ActionResult GiftList()
        {
            int pageIndex = int.Parse(Request["pageIndex"] ?? "1");
            int pageSize = int.Parse(Request["pageSize"] ?? "8");
            int totalCount = 0;
            ViewBag.AllGift = GiftBll.LoadPageEntities(pageSize, pageIndex, out totalCount, g => true, false, g => g.Intime).ToList();
                                     
            ViewData["pageSize"] = pageSize;
            ViewData["pageIndex"] = pageIndex;
            ViewData["totalCount"] = totalCount;
            return View();
        }
        #endregion
          /// <summary>
          /// 礼品详情
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
        public ActionResult GiftDetails(int id)
        {          
            var   gift= GiftBll.LoadEntities(g => g.Id==id).FirstOrDefault();
            ViewData.Model = gift as Gift;                     
            return View();
        }
        public ActionResult ConfirmGiftOrder()
        {                 
            string guid = Request["userId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int giftId = int.Parse(Request["giftId"]);
            string currentColor = Request["color"];
            int num = int.Parse(Request["num"]);
           var  gift= GiftBll.LoadEntities(g => g.Id == giftId).FirstOrDefault();
           ViewData.Model = gift as Gift; 
            //总价格
           ViewBag.TotalPrice =(float)gift.Price * num;
           //索要的数量
           ViewBag.Num = num;
           ViewBag.Color = currentColor;
            return View();
        }
          /// <summary>
          /// 玩家剩余的爽币
          /// </summary>
          /// <returns></returns>
        public ActionResult GetCoolCoinUsed()
        {
            string guid = Request["userId"];
           
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            int coinUsed=UserMessageBll.CoolCoinsUsed(user);
            return Json(coinUsed, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadUserAdress()
        {
            string guid = Request["userId"];
         
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
            var  data=  UserAdressBll.LoadEntities(d => d.UserId == user.Id ).Take(3).Select(d => new { 
            d.Id,d.InTime,d.Name,d.Phone,d.Address,d.IsDefault                    
            }).ToList();
           return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
           /// <summary>
           /// 添加收货地址
           /// </summary>
           /// <param name="name"></param>
           /// <param name="adress"></param>
           /// <param name="tel"></param>
           /// <returns></returns>
        public ActionResult AddUserAdress(string name,string address,string tel,int isDefault)
          {
              string guid = Request["userId"];
           
              var user = Common.CacheHelper.Get(guid) as PersonalUser;
             
              //UserAdress ua = new UserAdress();
              if (UserAdressBll.AddUserAddress(user.Id,isDefault,name,address,tel))
              {
                  return Content("ok");
              }
              else
              {
                  return Content("no");
              }  
                          
          }
          /// <summary>
          /// 删除我的收货地址
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
        public ActionResult AdressDel(int id)
        {
            UserAdress ua = new UserAdress() {
            Id=id
            };
            if (UserAdressBll.Delete(ua))
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }
        }
          /// <summary>
          /// 编辑收货地址
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
        public ActionResult AdressEdit(int id)
        {
            string name=Request["name"];
            string tel=Request["tel"];
            string address=Request["address"];
            var uadress = UserAdressBll.LoadEntities(ua => ua.Id == id).FirstOrDefault();
             uadress.Name=name;
            uadress.Address=address;
            if (UserAdressBll.Update(uadress))
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }     
        }
          /// <summary>
          /// 地址是否设置为默认
          /// </summary>
          /// <param name="id"></param>
          /// <param name="userId"></param>
          /// <returns></returns>
        public ActionResult AdressIsDefault(int id)
        {
            string guid = Request["userId"];
            //从分布式缓存拿出来的对象不能进行延迟加载。
            var user = Common.CacheHelper.Get(guid) as PersonalUser;
         
            if (UserAdressBll.UpdateUserAddress(id, user.Id))
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }          
        }
        #region 礼品退兑换
        public ActionResult ExChangeGift(int userId, int giftId, int addressId, int num, string color)
        {
            var data = GiftBll.ExChangeGift(userId, giftId, addressId, num, color);
            return Json(data, JsonRequestBehavior.AllowGet);
        } 
        #endregion

      }
}
