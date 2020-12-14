using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace Recipe6
{
    class Program
    {

        static Timer _timer;
        /// <summary>
        /// 在线程池中创建周期性调用的异步操作(定时线程)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            WriteLine("按回车键以停止计时器");
            DateTime start = DateTime.Now;
            _timer = new Timer(_ => TimeOperation(start), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            try
            {
                Sleep(TimeSpan.FromSeconds(6));
                _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(4));
                ReadLine();
            }
            finally {

                _timer.Dispose();
            }
   
        }



        static void TimeOperation(DateTime start)
        {
            TimeSpan elapsed = DateTime.Now - start;
            WriteLine($"距离上一次执行时间为{elapsed.Seconds}执行线程ID是:{CurrentThread.ManagedThreadId}");        
        }
    }
}
