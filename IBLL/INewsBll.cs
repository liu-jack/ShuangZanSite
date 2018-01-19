using Models;
using Models.MapModel;
using Models.SqlMapModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL
{
    public interface INewsBll : IBaseBll<News>
    {
        int MoreCheckNewsInfo(IList<int> ids, string checkIsPass, string  currentAdmin);
        bool NewsDelete(int id);
        bool NewsUpdate(News news);
        List<NewsViewModel> GetNewAllMsgImg(string type);
        /// <summary>
        /// pc新闻板块推荐最新3条->内容中的图片
        /// </summary>
        /// <param name="recForum"></param>
        /// <returns></returns>
        List<NewsViewModel> GetAllNewsPcMsg(string recForum);
        /// <summary>
        /// 查询对应的新闻类型数据
        /// </summary>
        /// <param name="rec">推荐参数</param>
        /// <param name="type">类型参数</param>
        /// <param name="take">要查询的总数</param>
        /// <returns></returns>
        IQueryable<FrontNews> GetAllTypeNews(string rec, string type, int take);
        /// <summary>
        /// 娱乐八卦板块推荐读取--内容中的图片提取
        /// </summary>
        /// <returns></returns>
        List<NewsViewModel> GetAllHappyNews();
        /// <summary>
        /// 资讯排行榜，同网站首页，不分类型，只要是新闻版块链的，按照浏览量的多少从高到底排。拿的数据是，编辑标题+链接
        /// </summary>
        /// <returns></returns>
        List<FrontNews> GetAllPageViewTopNews();

        /// <summary>
        /// 新闻详情 结合留言表联合查询出要得到数据
        /// </summary>
        /// <param name="id">前台请求的id</param>
        /// <returns></returns>
        IQueryable<NewsViewModel> GetNewsDetails(int id);
        /// <summary>
        /// 得到新闻列表的内容
        /// </summary>
        /// <param name="pageSize">一夜多少条</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="total">新闻总条数</param>
        /// <param name="type">要查询的新闻类型</param>
        /// <returns>返回list集合</returns>
        IQueryable<NewsViewModel> GetMoreNews(int pageSize, int pageIndex, out int total,string type);
        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        IQueryable<NewsViewModel> GetSearchNews(int pageSize, int pageIndex, out int total, string title);
        /// <summary>
        /// 网站首页显示的15条数据
        /// </summary>
        /// <returns></returns>       
        List<FrontNews> GetAllNewsIndex();
        /// <summary>
        /// 根据类型查询数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<FrontNews> AccondingTypeGetNews(string type);
        /// <summary>
        /// 根据浏览量的高低查询最近一个月的新闻数据
        /// </summary>
        /// <returns></returns>
        List<NewsViewModel> InLikeNumNews();
        /// <summary>
        /// 娱乐八卦新闻详情推荐
        /// </summary>
        /// <param name="top"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Sql_NewsModel> GetRelatedFunNews(int id,int top, string type);
        IList<NewsViewModel> TheSameKeyWords(int id);
    }
}
