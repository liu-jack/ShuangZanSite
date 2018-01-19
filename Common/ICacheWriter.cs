using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
namespace Common
{
   public interface ICacheWriter
    {
       void Set(string  key, object value, DateTime exp);
       void Set(string  key, object value);
       object Get(string key);
       object Remove(string  key,object value,DateTime exp);
    }
}
