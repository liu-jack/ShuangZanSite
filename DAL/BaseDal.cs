using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using Models;
using System.Linq.Expressions;
namespace DAL
{
     //针对所有的实体的公共的crud方法的封装。
    public class BaseDal<T> where T:class ,new()
    {      
        public DbContext db;        
        public BaseDal()
        {
            db = DbContextFactory.GetCurrentDbContext();
        }
        #region CUD

        public T Add(T entity)
        {
            
            db.Set<T>().Add(entity);
          //  db.SaveChanges();//自动增长的主键，会自动赋值到  属性上去。
            return entity;
        }

        public bool Update(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
       
            return true;

        }
        public bool Delete(T entity)
        {                 
           db.Entry(entity).State = EntityState.Deleted;
           // db.SaveChanges();       
            return true;
        }
        #endregion

        #region 查询
       
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            //db.Set<UserInfo>().Where(u => u.ID > 10).Select(u => u);
            
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }
        // 实现对数据的分页查询
        /// </summary>

        /// <typeparam name="S">按照某个类进行排序</typeparam>

        /// <param name="pageIndex">当前第几页</param>

        /// <param name="pageSize">一页显示多少条数据</param>

        /// <param name="totalCount">总条数</param>

        /// <param name="whereLambda">取得排序的条件</param>

        /// <param name="isAsc">如何排序，根据倒叙还是升序</param>

        /// <param name="orderBy">根据那个字段进行排序</param>

        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex,
                                                  out int totalCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc,Expression<Func<T,S>> orderBy )
        {
            IQueryable<T> temp = db.Set<T>().Where(whereLambda).AsQueryable();
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy(orderBy)
                           .Skip(pageSize*(pageIndex - 1))
                           .Take(pageSize).AsQueryable();
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                          .Skip(pageSize * (pageIndex - 1))
                          .Take(pageSize).AsQueryable();
            }
            return temp;
        }

        #endregion
    }
}
