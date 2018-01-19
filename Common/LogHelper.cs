using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common
{
   /// <summary>
    /// 1 咱创造性的制造了一个 错误消息的（内存）队列。当出现异常之后，
    /// 直接把错误消息放到内存的队列里去。
    /// 然后Web继续往下执行，给用户反馈信息。不会阻塞用户的响应。
    /// 2 用户了Log4net可以将日志写到不同地方去。不需要改代码。
    /// </summary>
    public class LogHelper
    {
        //异常信息的队列
        public static Queue<string> ExcMsg;

        static LogHelper()
        {
            ExcMsg = new Queue<string>();
            ThreadPool.QueueUserWorkItem(u =>
                {
                    while (true)
                    {
                        string str = string.Empty;

                        if (ExcMsg == null)
                        {
                            continue;
                        }

                        lock (ExcMsg)
                        {
                            if (ExcMsg.Count > 0)
                                str = ExcMsg.Dequeue();
                        }
                        //往日志文件里面写就可以了。
                        if (!string.IsNullOrEmpty(str))
                        {
                            ILog log = log4net.LogManager.GetLogger("Test");
                            log.Error(str);
                        }
                        if (ExcMsg.Count() <= 0)
                        {
                            Thread.Sleep(30);
                        }
                    }
                });
        }

        public static void WriteLog(string msg)
        {
            lock (ExcMsg)
            {
                ExcMsg.Enqueue(msg);
            }
            //数据结构。

            //磁盘。  内存块。

            //错误消息写到队列里面去。

            //lock ("全局锁")
            //{
            //    //写到文件里面去。
            //    using (StreamWriter writer = new StreamWriter())
            //    {
            //        writer.Write(msg);
            //    }

            //}
        }
    }
}
