using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
  public  class Md5Helper
    {
      /// <summary>
        /// MD5　32位加密
      /// </summary>
      /// <param name="txt"></param>
      /// <returns></returns>
      public static string GetMd5(string txt)
      {
          //创建md5对象
          MD5 md5 = MD5.Create();//创建MD5对象（MD5类为抽象类不能被实例化）
          byte[] by = Encoding.UTF8.GetBytes(txt);//将字符串编码转换为一个字节序列
          byte[] by2 = md5.ComputeHash(by);//计算data字节数组的哈希值（加密）
          
          StringBuilder sb = new StringBuilder();
          for (int  i= 0;  i< by2.Length; i++)
          {
              sb.Append(by2[i].ToString("x2").ToLower()); 
          }
          return sb.ToString();
      }
    }
}
