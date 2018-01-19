using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class LaomaPager
    {
        #region 当前页
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">一页多少条</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalCount">总条数</param>
        /// <returns></returns>
        public static string ShowPageNavigate(int pageSize, int currentPage, int totalCount)
        {
            string redirectTo = "";
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink first' href='{0}?pageIndex=1&pageSize={1}'>首　页</a> ", redirectTo, pageSize);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink prev' href='{0}?pageIndex={1}&pageSize={2}'>上一页</a> ", redirectTo, currentPage - 1, pageSize);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 1;
                for (int i = 0; i < 6; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage, pageSize, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink next' href='{0}?pageIndex={1}&pageSize={2}'>下一页</a> ", redirectTo, currentPage + 1, pageSize);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink last' href='{0}?pageIndex={1}&pageSize={2}'>末　页</a> ", redirectTo, totalPages, pageSize);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行
            return output.ToString();
        } 
        #endregion
        #region 加当前id
        public static string ShowPageNavigate(int pageSize, int currentPage, int totalCount, int id)
        {
            string redirectTo = "";
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink first' href='{0}?pageIndex=1&pageSize={1}&id={2}'>首页</a> ", redirectTo, pageSize, id);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink prev' href='{0}?pageIndex={1}&pageSize={2}&id={3}'>上一页</a> ", redirectTo, currentPage - 1, pageSize, id);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}&id={3}'>{4}</a> ", redirectTo, currentPage, pageSize, id, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&id={3}'>{4}</a> ", redirectTo, currentPage + i - currint, pageSize, id, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink next' href='{0}?pageIndex={1}&pageSize={2}&id={3}'>下一页</a> ", redirectTo, currentPage + 1, pageSize, id);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink last' href='{0}?pageIndex={1}&pageSize={2}&id={3}'>末页</a> ", redirectTo, totalPages, pageSize, id);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行
            return output.ToString();
        } 
        #endregion
        #region 搜索分页
        public static string ShowPageNavigate(int pageSize, int currentPage, int totalCount, string key)
        {
            string redirectTo = "";
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink first' href='{0}?pageIndex=1&pageSize={1}&key={2}'>首页</a> ", redirectTo, pageSize, key);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink prev' href='{0}?pageIndex={1}&pageSize={2}&key={3}'>上一页</a> ", redirectTo, currentPage - 1, pageSize, key);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}&key={3}'>{4}</a> ", redirectTo, currentPage, pageSize, key, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&key={3}'>{4}</a> ", redirectTo, currentPage + i - currint, pageSize, key, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink next' href='{0}?pageIndex={1}&pageSize={2}&key={3}'>下一页</a> ", redirectTo, currentPage + 1, pageSize, key);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink last' href='{0}?pageIndex={1}&pageSize={2}&key={3}'>末页</a> ", redirectTo, totalPages, pageSize, key);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行
            return output.ToString();
        }  
        #endregion
        #region 伪静态的分页链接
        public static string ShowPageNavigate(string redirectTo, int pageSize, int currentPage, int totalCount)
        {
            // string redirectTo = "";
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink first' href='{0}/pageIndex=1/pageSize={1}'>首页</a> ", redirectTo, pageSize);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink prev' href='{0}/pageIndex={1}/pageSize={2}'>上一页</a> ", redirectTo, currentPage - 1, pageSize);
                }              
                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}/pageIndex={1}/pageSize={2}'>{3}</a> ", redirectTo, currentPage, pageSize, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}/pageIndex={1}/pageSize={2}'>{3}</a> ", redirectTo, currentPage + i - currint, pageSize,  currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink next' href='{0}/pageIndex={1}/pageSize={2}'>下一页</a> ", redirectTo, currentPage + 1, pageSize);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink last' href='{0}/pageIndex={1}/pageSize={2}'>末页</a> ", redirectTo, totalPages, pageSize);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行
            return output.ToString();
        } 
        #endregion
    }

}
