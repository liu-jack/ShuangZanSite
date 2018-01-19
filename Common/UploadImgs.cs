using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 上传图片
    /// </summary>
    public class UploadImgs
    {
        /// <summary>
        /// 公共的图片上传方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        ///  HttpPostedFileBase
        public static string CommonUploadImg(HttpPostedFileBase file,out string msg,string filePath) 
        {
           if (file==null)
           {
                return msg = "empty,您懂上传吗？"; 
           }
         string fileExt = Path.GetExtension(file.FileName);//得到文件的后缀名
         if (UploadImgs.IsCheckImgType(fileExt))
         {
             //拿到文件MD5值+文件扩展名
             string fileName = Common.WebHelper.GetStreamMd5(file.InputStream) +fileExt;
             //string dir = "/Upload/image/" + fileName;
             string dir = filePath + fileName;
             //转换为相对路径             
             string fullDir = System.Web.HttpContext.Current.Server.MapPath(dir);
             file.SaveAs(fullDir);
             return msg = fileName;//返回文件名
         }
         else {
             return msg = "typeError,请选择正确的图片类型,目前仅支持:jpg/png/jpeg";
         }
        }
        /// <summary>
        /// 校验图片类型
        /// </summary>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public static bool IsCheckImgType(string imgType)
        {
            List<string> imgList = new List<string>();
            imgList.Add(".jpg");
            imgList.Add(".png");
            imgList.Add(".jpeg");
            imgList.Add(".bmp");
            return imgList.Contains(imgType); 
        }    
    }
  }
