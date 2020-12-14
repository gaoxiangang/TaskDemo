using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Thread;

namespace AutoResetEcentDemo
{
    class Program
    {
        private static AutoResetEvent _workerEvent = new AutoResetEvent(false);
        private static AutoResetEvent _mainEvent = new AutoResetEvent(false);


        static void Process(int  seconds) {
            WriteLine("Starting a long  running  work");

            Sleep(TimeSpan.FromSeconds(seconds));

            WriteLine("工作完成了");

            _workerEvent.Set();
            WriteLine("等待主线程完成它的工作");
            _mainEvent.WaitOne();
            WriteLine("开始第二个操作");
            Sleep(TimeSpan.FromSeconds(seconds));
            WriteLine("工作完成了");
            _workerEvent.Set();
        }

        /// <summary>
        /// 从线程A向线程B推送通知
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var t = new Thread(() => Process(10));
            t.Start();
            WriteLine("等待其他线程完成工作");
            _workerEvent.WaitOne();
            WriteLine("第一个线程工作完成了");
            WriteLine("开始在主线程上完成工作");
            _mainEvent.Set();
            WriteLine("现在主线程开始执行第二部操作");
            _workerEvent.WaitOne();
            WriteLine("主线程第二步操作完成");
            ReadLine();
        }
    }
}
