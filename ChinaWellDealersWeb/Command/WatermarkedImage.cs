using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ShuangZan.Web.Command
{
    public class WatermarkedImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "image/jpg";
                string path = HttpContext.Current.Server.MapPath("~/Content/Img/mark.png");
                //System.Web.HttpContext.Current.Response.Write(System.Drawing.Image.FromFile(context.Request.PhysicalPath));
                using (Bitmap bmp = new Bitmap(Image.FromFile(context.Request.PhysicalPath)))
                {
                    int bmpw = bmp.Width, bmph = bmp.Height;
                    // 绑定画板 
                    Graphics grap = Graphics.FromImage(bmp);
                    // 加载水印图片 
                    using (Bitmap bt = new Bitmap(path))
                    {
                        int btw = bt.Width, bth = bt.Height;
                        if (bmpw > btw && bmph > bth) //正常，图片大于水印图
                        {
                            grap.DrawImage(bt, bmpw - btw, bmph - bth, bt.Width, bt.Height);
                            bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else
                        {
                            bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //if (bmpw < btw && bmph < bth) //整个图都比水印图小
                            //{
                            //}
                            //else
                            //{
                            //    if (bmpw < btw) //宽小于水印图宽
                            //    {
                            //    }
                            //    else //高小于水印图
                            //    {
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(@"D:\爽赞网\err.txt", e.ToString());
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}