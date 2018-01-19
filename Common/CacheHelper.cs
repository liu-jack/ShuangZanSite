using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace Common
{
  public  class CacheHelper
    {
        public static ICacheWriter CacheWriter { get; set; }

        static CacheHelper()
        {
            //如果是静态的属性的话，如果想让它有注入的值，那么必须先创建一个实例后，才会注入。
            IApplicationContext ctx = ContextRegistry.GetContext();
            var userInfoDal = ctx.GetObject("CacheHelper") as CacheHelper;
        }
        //静态方法调用的时候，不需要spring容器创建实例。所以属性并没有注入进去。
        //那么在类的静态构造函数内部，逼迫spring容器给我们创建一个CacheHelper，那么顺便
        //就给当前 的静态属性CacheWriter做注入操作了。所以，下面的所有的静态方法在用的时
        //侯就可以有实例了。

        //mvc是根据请求地址创建控制器（用默认的控制器工厂。spring.net mvc把默认的控制器工厂给替换掉了。）
        public static void WriteCache(string key, object value, DateTime ex)
        {
            //ICacheWriter CacheWriter =new HttpRuntimeCacheWriter();
            CacheWriter.Set(key, value, ex);
        }
        public static object Get(string key)
        {
            //ICacheWriter CacheWriter = new MemmcachedWriter();
            return
            CacheWriter.Get(key);
        }
        public static object Remove(string  key,object value,DateTime exp)
        {
            return CacheWriter.Remove(key, value, exp);
        }
    }
}
