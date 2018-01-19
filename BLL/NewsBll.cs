using Common;
using IBLL;
using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class NewsBll : BaseBll<News>, INewsBll
    {

        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.NewsDal;
        }
        #region 批量审核
        public int MoreCheckNewsInfo(IList<int> ids, string checkIsPass, string currentAdmin)
        {
            foreach (var id in ids)
            {
                //先查询出来每个ID对应的数据
                var news = dbSession.NewsDal.LoadEntities(u => u.Id == id).FirstOrDefault();
                if (checkIsPass == "1")//1   代表审核通过
                {   //然后将对应的值更新到数据库
                    news.State = "1";
                    news.CheckName = currentAdmin;//当前审核人
                    dbSession.NewsDal.Update(news);
                }
                else if (checkIsPass == "0")//0  代表审核未通过
                {
                    news.State = "0";
                    news.CheckName = currentAdmin;
                    dbSession.NewsDal.Update(news);
                }
            }
            return dbSession.SaveChanges();
        }
        #endregion
        #region 删除
        public bool NewsDelete(int id)
        {
            News news = new News() { Id = id };
            dbSession.NewsDal.Delete(news);
            return dbSession.SaveChanges() > 0;
        }
        #endregion
        #region 更新数据
        public bool NewsUpdate(News news)
        {
            dbSession.NewsDal.Update(news);
            return dbSession.SaveChanges() > 0;
        }	 
	    #endregion
        #region (新闻热游、新游、产业)分别读取新闻类别中的数据、提取文本内容中的第一张照片
        /// <summary>
        /// 分别读取新闻类别中的数据、提取文本内容中的第一张照片
        /// </summary>
        /// <param name="type">新闻类别</param>
        /// <returns></returns>
        public List<NewsViewModel> GetNewAllMsgImg(string type)
        {
            var  allNewMsgImg = dbSession.NewsDal.LoadEntities(n => n.Type == type&&n.State=="1")
                                                 .Select(n => new NewsViewModel()
                                                 {
                                                   Id= n.Id,
                                                   Msg= n.Msg,
                                                   Title= n.Title,
                                                   EditTitle= n.EditTitle,
                                                  InTime=n.InTime
                                                 }).OrderByDescending(n =>n.InTime).Take(1).ToList();

            return allNewMsgImg;
        }
        #endregion
        #region pc新闻板块推荐最新3条
        /// <summary>
        /// pc新闻板块推荐最新3条并且审核通过
        /// </summary>
        /// <param name="recForum">板块推荐的标识：1</param>
        /// <returns></returns>
        public List<NewsViewModel> GetAllNewsPcMsg(string recForum)
        {
         var allNewMsgImg = dbSession.NewsDal.LoadEntities(n => n.Rec_Forum_Index == recForum && n.Type == "4" && n.State == "1") .Select(n => new 
                                               {
                                                   Id = n.Id,
                                                   Msg = n.Msg,
                                                   Title = n.Title,
                                                   EditTitle = n.EditTitle,
                                                   Memo = n.Memo,
                                                   InTime = n.InTime,
                                                   n.Rec_Forum_Time,
                                                   n.Views
                                               }).AsNoTracking().OrderByDescending(n => n.Rec_Forum_Time).Take(3);
         var allLeaveMsg = dbSession.LeaveMsgDal.LoadEntities(l => true).Select(l => new { l.NewsId }).AsNoTracking();
            var data = (from m in allNewMsgImg
                        let MsgNum=(from n in allLeaveMsg where m.Id==n.NewsId select n).Count()
                        join l in allLeaveMsg on m.Id equals l.NewsId into mm
                        from mmi in mm.DefaultIfEmpty()
                        select new NewsViewModel()
                        {
                            Id = m.Id,
                            EditTitle = m.EditTitle,
                            Msg = m.Msg,
                            InTime = m.InTime,
                            ViewNum = m.Views,
                            MsgNum =MsgNum,
                            Memo = m.Memo
                        }).Distinct().ToList();
         
            return data;
        }
        #endregion
        #region 查询对应的新闻类型数据
        /// <summary>
        /// 查询对应的新闻类型数据
        /// </summary>
        /// <param name="rec">推荐参数</param>
        /// <param name="type">类型参数</param>
        /// <param name="take">要查询的总数</param>
        /// <returns></returns>
        public IQueryable<FrontNews> GetAllTypeNews(string rec, string type, int take)
        {
            return dbSession.NewsDal.LoadEntities(n => n.Type == type && n.Rec_Forum_Index == rec && n.State == "1").OrderByDescending(n => n.Rec_Forum_Time)
                                .Select(n => new FrontNews
                                 {
                                     EditTitle = n.EditTitle,
                                     Id = n.Id,
                                     Title = n.Title,
                                     InTime = n.InTime
                                 }).Take(take).AsNoTracking().AsQueryable();
        }
        #endregion
        #region 娱乐八卦板块推荐读取--内容中的图片提取
        /// <summary>
        /// 娱乐八卦板块推荐读取--内容中的图片提取
        /// </summary>
        /// <returns></returns>
        public List<NewsViewModel> GetAllHappyNews()
        {
            var allNewMsgImg = dbSession.NewsDal.LoadEntities(n => n.Type == "0" && n.Rec_Forum_Index == "1"&&n.State=="1").OrderByDescending(n => n.Rec_Forum_Time)
                                                 .Select(n => new 
                                                 {
                                                     n.Id,
                                                      n.Msg,
                                                    n.EditTitle
                                                 }).Take(7).AsNoTracking();
            List<NewsViewModel> newList = new List<NewsViewModel>();       
            NewsViewModel viewmodel = null;
            foreach (var item in allNewMsgImg)
            {
                viewmodel = new NewsViewModel();              
                viewmodel.Image = WebHelper.PickupImgUrl(item.Msg).ToString();
                viewmodel.Id = item.Id;
                viewmodel.EditTitle = item.EditTitle;
                newList.Add(viewmodel);
            }
            return newList;                       
        }
        #endregion
        #region 新闻版块链的，按照浏览量的多少从高到底排
        /// <summary>
        /// 排行榜，同网站首页，不分类型，只要是新闻版块链的，按照浏览量的多少从高到底排。拿的数据是，编辑标题+链接
       /// </summary>
       /// <returns></returns>
        public List<FrontNews> GetAllPageViewTopNews()
        {
            var data = dbSession.NewsDal.LoadEntities(n => n.State=="1")
                .OrderByDescending(n=>n.Views).ThenByDescending(n=>n.InTime)              
                .Select(n => new FrontNews()
                {          
                 Id=n.Id,
                 Title=n.Title,
                EditTitle=n.EditTitle,
                Views=n.Views,
                }).Take(10).AsNoTracking().ToList();
            
             return data;
        }
        #endregion
        #region 新闻详情 结合留言表联合查询出要得到数据
        /// <summary>
        /// 新闻详情 结合留言表联合查询出要得到数据
        /// </summary>
        /// <param name="id">前台请求的id</param>
        /// <returns></returns>
        public IQueryable<NewsViewModel> GetNewsDetails(int id)
        {
            var allNews = dbSession.NewsDal.LoadEntities(n => true);
                     
            var allLeaveMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0);             
            //链接查询
            var data = from n in allNews
                       join m in allLeaveMsg on n.Id equals m.NewsId into mc
                       from mci in mc.DefaultIfEmpty()
                       let MsgNum=(from a in allLeaveMsg  where a.NewsId ==n.Id select a.NewsId).Count() 
                       where n.Id==id
                       select new NewsViewModel {
                              Id = n.Id,
                               Type= n.Type,
                               InTime=n.InTime,
                               ViewNum =n.Views!=null?n.Views:0,                             
                              Source = n.Source??"爽赞网",
                               Title= n.Title,
                               Editor=n.Editor??"爽赞小编",
                               Msg= n.Msg,
                               MsgNum=MsgNum!=0?MsgNum:0,
                               EditTitle=n.EditTitle,
                               Memo=n.Memo,
                               Kewords=n.Kewords
                        };
            return data.AsNoTracking().AsQueryable();
        }
        #endregion
        #region 得到新闻列表的内容            
        /// <summary>
        /// 得到新闻列表的内容
        /// </summary>
        /// <param name="pageSize">一夜多少条</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="total">新闻总条数</param>
        /// <param name="type">要查询的新闻类型</param>
        /// <returns>返回list集合</returns>
        public IQueryable<NewsViewModel> GetMoreNews(int pageSize, int pageIndex, out int total, string type)
        {
           
            //先分别查询两表的数据
            var allNews = dbSession.NewsDal.LoadEntities(n=>n.Type==type&&n.State=="1")
                                                .Select(n => new
                                                        {
                                                            n.Id,
                                                            n.Type,
                                                            n.InTime,
                                                            n.Views,
                                                            n.EditTitle,
                                                            n.Title,
                                                            n.Msg,
                                                            n.Memo,
                                                        });
            var allLeaveMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0).Select(m => new
                                                        {
                                                            m.NewsId,                                                          
                                                        });          
            //联合查询转换成集合
            var data = from n in allNews
                       join m in allLeaveMsg on n.Id equals m.NewsId into mc
                       from mci in mc.DefaultIfEmpty()
                       let MsgNum = (from c in allNews where c.Id == mci.NewsId select c).Count()
                       select new NewsViewModel
                       {
                           Id = n.Id,
                           Type = n.Type,
                           InTime = n.InTime,
                           ViewNum = n.Views,
                           Title = n.Title,
                           EditTitle = n.EditTitle,
                           Msg = n.Msg,
                           MsgNum = MsgNum,
                           Memo = n.Memo
                       };          
            total = data.Count();
            return data.Distinct().AsNoTracking().OrderByDescending(n => n.InTime)
                       .Skip(pageSize * (pageIndex - 1))
                         .Take(pageSize).AsQueryable();         
        }
        #endregion
        #region 搜索的结果
        /// <summary>
        /// 搜索的结果
        /// </summary>
        /// <param name="pageSize">一夜多少条</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="total">新闻总条数</param>
        /// <param name="type">搜索的标题</param>
        /// <returns>返回list集合</returns>
        public IQueryable<NewsViewModel> GetSearchNews(int pageSize, int pageIndex, out int total, string title)
        {
            //先分别查询两表的数据
            var allNews = dbSession.NewsDal.LoadEntities(n => n.Title.Contains(title) || n.EditTitle.Contains(title) || n.Kewords.Contains(title))
                                                .Select(n => new
                                                {
                                                    n.Id,
                                                    n.Type,
                                                    n.InTime,
                                                    n.Views,
                                                    n.EditTitle,
                                                    n.Title,
                                                    n.Msg,
                                                    n.Memo,
                                                });
            var allLeaveMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0).Select(m => new
            {
                m.NewsId,
                m.MsgNum
            });
            //联合查询转换成集合
            var data = from n in allNews
                       join m in allLeaveMsg on n.Id equals m.NewsId into nm
                       from nmi in nm.DefaultIfEmpty()
                       let MsgNum = (from mm in allLeaveMsg where mm.NewsId == n.Id  select mm).Count()
                       select new NewsViewModel
                       {
                           Id = n.Id,
                           Type = n.Type,
                           InTime = n.InTime,
                           ViewNum = n.Views,//浏览量
                           EditTitle = n.EditTitle,
                           Title = n.Title,
                           Msg = n.Msg,
                           MsgNum = MsgNum,//留言数
                           Memo = n.Memo
                       };
                    
            total = data.Count();
            return data.AsNoTracking().OrderByDescending(n => n.InTime)
                         .Skip(pageSize * (pageIndex - 1))
                         .Take(pageSize).AsQueryable(); 
        } 
        #endregion
        #region 网站首页显示的15条数据
        /// <summary>
        /// 网站首页显示的15条数据
        /// </summary>
        /// <returns></returns>
        public List<FrontNews> GetAllNewsIndex()
        {
            var data = dbSession.NewsDal.LoadEntities(n => n.State == "1" && n.Rec_Hot_Index == "1").Select(n => new FrontNews() { Id = n.Id, Type = n.Type, InTime = n.InTime, EditTitle = n.EditTitle, Title = n.Title }).OrderByDescending(d => d.InTime).Take(15).AsNoTracking().ToList();
            return data;
        } 
        #endregion
        #region 根据类型分别三条最新首页推荐数据
        /// <summary>
        /// 根据类型分别三条最新首页推荐数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<FrontNews> AccondingTypeGetNews(string type)
        {
            var data = dbSession.NewsDal.LoadEntities(n => n.Type == type && n.State == "1" && n.Rec_Hot_Index == "1").Select(n => new FrontNews() { Id = n.Id, EditTitle = n.EditTitle, Title = n.Title, InTime = n.InTime }).OrderByDescending(d => d.InTime).Take(3).AsNoTracking().ToList();
            return data;
        } 
        #endregion
        #region 根据浏览量的高低查询最近一个月的新闻数据
        /// <summary>
        /// 根据浏览量的高低查询最近一个月的新闻数据
        /// </summary>
        /// <returns></returns>
        public List<NewsViewModel> InLikeNumNews()
        {
            var data = dbSession.NewsDal.LoadEntities(n => DbFunctions.DiffMonths(n.InTime, DateTime.Now) <= 30 && n.State == "1").Select(n => new NewsViewModel()
               {
                 Id= n.Id,
                 EditTitle=n.EditTitle,
                 Views=n.Views,
              Msg= n.Msg,
              InTime= n.InTime,
             Title= n.Title,
             Type= n.Type
               }).AsNoTracking().OrderByDescending(n => n.Views).Take(10).ToList();         
          
            return data;
        } 
        #endregion

        public List<Sql_NewsModel> GetRelatedFunNews(int id,int top, string type)
        {
            return dbSession.NewsDal.GetRelatedFunNews(id, top, type);
        }
        #region 同关键词名，最新6条，数量不够，同类型（是新游的，就拿新游的最新数据替补）
        public IList<NewsViewModel> TheSameKeyWords(int id)
        {
            var news = dbSession.NewsDal.LoadEntities(n => n.Id == id).FirstOrDefault();
            var data = dbSession.NewsDal.LoadEntities(n => n.GameTerms.Contains(news.GameTerms) && n.Id != id).Select(n => new NewsViewModel
               {
                   Id = n.Id,
                   Title = n.Title,
                   InTime = n.InTime
               }).AsNoTracking().OrderByDescending(n => n.InTime).Take(6).ToList();
            List<NewsViewModel> list = null;
            if (data.Count < 6)
            {
                list = dbSession.NewsDal.LoadEntities(n => n.Type.Contains(news.Type) && !n.GameTerms.Contains(news.GameTerms)).Select(n => new NewsViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    InTime = n.InTime
                }).AsNoTracking().OrderByDescending(n => n.InTime).Take(6 - data.Count).ToList();
            }
            if (list != null)
            {
                data.AddRange(list);
            }
            return data;
        } 
        #endregion
    }
}











