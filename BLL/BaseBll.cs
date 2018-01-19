using DalFactory;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace BLL
{
  public abstract  class BaseBll<T>where T:class,new()
    {
           protected IDbSession dbSession;//当前类或者是子类
           public BaseBll()
        {
            dbSession = DbSessionFactory.GetCurrentDbSession();
            SetCurrentDal();//构造函数里面调用了 抽象方法：SetCurrentDal
        }    
     public IBaseDal<T> CurrentDal;
      //需要给CurrentDal赋值。基类 不知道当前Dal是谁。 子类知道。
      //逼迫子类给父类的一个属性赋值。
     public abstract void SetCurrentDal();
        public T Add(T entity)
        {
            CurrentDal.Add(entity);
            dbSession.SaveChanges();
            return entity;
            //return dbSession.UserInfoDal.Add(entity);
        }
        public bool Update(T enetity)
        {
           CurrentDal.Update(enetity);
           return dbSession.SaveChanges() > 0;
        }
        public bool Delete(T entity)
        {
             CurrentDal.Delete(entity);
            return dbSession.SaveChanges() > 0;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex,
                                                 out int totalCount,
                                                 Expression<Func<T, bool>> whereLambda, bool isAsc,
                                                 Expression<Func<T, S>> orderBy)
        {
            return CurrentDal.LoadPageEntities(pageSize, pageIndex, out totalCount, whereLambda, isAsc, orderBy);
        }
        
    }
}
