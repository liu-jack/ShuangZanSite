using IBLL;
using Models;
using Models.MapModel;
using Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
namespace BLL
{
    public class BeautifulGirlsBll : BaseBll<BeautifulGirls>, 
        IBeautifulGirlsBll
    {
        public override void SetCurrentDal()
        {
            CurrentDal = dbSession.BeautifulGirlsDal;
        }
        #region 福利美图管理
        /// <summary>
        /// 福利美图管理
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IQueryable<GirlsViewModel> LoadGirlSInfo(GirlsParam param)
        {
            var allGirls = dbSession.BeautifulGirlsDal.LoadEntities(b => true).Select(b => new { b.Views, b.Id, b.Imgs, b.InTime, b.Title, b.AddedBy,b.Rec,b.Rec_Time });
            var allMsg = dbSession.LeaveMsgDal.LoadEntities(m => m.Id > 0).Select(m => new { m.GirlId });
            var data = (from g in allGirls
                        join m in allMsg on g.Id equals m.GirlId into gmi
                        from gm in gmi.DefaultIfEmpty()
                        let Count = (from mm in allMsg where mm.GirlId == g.Id select mm.GirlId).Count()                     
                        select (new GirlsViewModel()
                        {
                            Id = g.Id,
                            Image = g.Imgs,
                            Views = g.Views,
                            InTime = g.InTime,
                            Title = g.Title,
                            AddedBy = g.AddedBy,
                            LeaveMsgCount =Count,
                            Rec=g.Rec,
                            Rec_Time=g.Rec_Time
                        })).Distinct();
            if (!string.IsNullOrEmpty(param.Title))
            {
                data = data.Where(d => d.Title.Contains(param.Title));
            }
            if (!string.IsNullOrEmpty(param.AddedBy))
            {
               data = data.Where(d => d.AddedBy.Contains(param.AddedBy));
            }
            GirlsViewModel viewModel = null;
            List<GirlsViewModel> list = new List<GirlsViewModel>();
            foreach (var item in data)
            {
                viewModel = new GirlsViewModel();
                viewModel.Image = item.Image.Split(',')[0];
                viewModel.Id = item.Id;
                viewModel.Views = item.Views;
                viewModel.InTime = item.InTime;
                viewModel.Title = item.Title;
                viewModel.AddedBy = item.AddedBy;
                viewModel.ImageCount = item.Image.Split(',').Count();
                viewModel.LeaveMsgCount = item.LeaveMsgCount;
                viewModel.Rec = item.Rec;
                viewModel.Rec_Time = item.Rec_Time;
                list.Add(viewModel);
            }          
            param.Total = data.Count();
            return list.OrderByDescending(d => d.InTime).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
        #region 校验标签
        public bool CheckIsContains(string tag)
        {
            List<BeautifulGirls> list = GetAllGirlTags();
            List<string> newList = new List<string>();
            foreach (var item in list)
            {
                newList.Add(item.Tags);
            }
            return newList.Contains(tag);
        } 
        #endregion
        #region 拿到所有的tag标签
        public List<BeautifulGirls> GetAllGirlTags()
        {

            Guid guid = Guid.NewGuid();
            object obj = Common.CacheHelper.Get(guid.ToString());
            List<BeautifulGirls> list = null;
            if (obj == null)
            {
                list = dbSession.BeautifulGirlsDal.LoadEntities(b => b.Id > 0).ToList().Select(b => new BeautifulGirls() { Tags = b.Tags }).ToList();

                if (list != null)
                {
                    try
                    {
                        Common.CacheHelper.WriteCache(guid.ToString(), list, DateTime.Now.AddMinutes(1));
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("福利美图的缓存标签抛异常啦！快找程序员！" + ex.Message);
                    }
                }
            }
            else
            {
                list = obj as List<BeautifulGirls>;
            }
            return list;
        } 
        #endregion
        #region 前台福利美图展示
        /// <summary>
        /// 前台福利美图展示
        /// </summary>
        /// <param name="type"></param> 
        /// <param name="game"></param>
        /// <returns></returns>
        public IQueryable<BeautifulGirls> GetAllGirlsInfo(string type, string game, int pageIndex, int pageSize)
        {         
            var temp = dbSession.BeautifulGirlsDal.LoadEntities(g => true);
            if (!string.IsNullOrEmpty(type))
            {
                // temp.Where(t => t.Tags.StartsWith(type) || t.Tags.EndsWith(type) || t.Tags.Contains(type));
              temp= temp.Where(t => t.Tags.Contains(type));
            }
            if (game == "hot")
            {
                return temp.AsNoTracking().OrderByDescending(t => t.Views).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            }
            else {
                return temp.AsNoTracking().OrderByDescending(t => t.InTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            }                     
        }
        #endregion
        #region 首页推荐6条最新数据
        /// <summary>
        /// 首页推荐6条最新数据
        /// </summary>
        /// <returns></returns>
        public List<BeautifulGirls> NewestIndexRecGirls()
        {
          
            var temp = dbSession.BeautifulGirlsDal.LoadEntities(g => g.Rec == 1).Select(g => new 
            {
               g.Id,
               g.Imgs,
               g.Tags,
                g.Title,
                g.Rec_Time
            }).AsNoTracking().OrderByDescending(d => d.Rec_Time).Take(6).ToList();
            List<BeautifulGirls> newlist = new List<BeautifulGirls>();
            BeautifulGirls viewModel = null;
            foreach (var item in temp)
            {
                viewModel = new BeautifulGirls();
                viewModel.Imgs = item.Imgs.Split(',')[0];
                viewModel.Title = item.Title;
                viewModel.Id = item.Id;
                newlist.Add(viewModel);
            }
            return newlist;
        }
        
        #endregion
        #region 福利美图详情
        /// <summary>
        /// 福利美图详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<GirlsViewModel> GetGirlDetails(int id)
        {
            var temp = dbSession.BeautifulGirlsDal.LoadEntities(g =>true);
          
            var msg= dbSession.LeaveMsgDal.LoadEntities(b => b.Id > 0);
            var data = (from t in temp
                       join m in msg on t.Id equals m.GirlId into tt
                       from tti in tt.DefaultIfEmpty()
                       let MsgNum = (from mm in msg where mm.GirlId == t.Id select mm.GirlId).Count()
                       where t.Id==id
                       select (new GirlsViewModel() {
                       Id=t.Id,
                       Title=t.Title,
                       Tags=t.Tags,
                       LeaveMsgCount=MsgNum!=0?MsgNum:0,
                       LeadTxt=t.LeadTxt,
                       Image=t.Imgs,
                       InTime=t.InTime,
                       Views=t.Views==null?0:t.Views,                       
                       })).AsNoTracking().AsQueryable();
            return data;                      
        } 
        #endregion
      
    } 
        
   
}
