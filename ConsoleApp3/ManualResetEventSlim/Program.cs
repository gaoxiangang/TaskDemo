using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace ManualResetEventSlimDemo
{
    class Program
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);

        static void TravelThroughGates(string threadName, int seconds)
        {
            WriteLine($"{threadName} 开始睡眠");
            Sleep(TimeSpan.FromSeconds(seconds));
            WriteLine($"{threadName}等待执行中");
            _mainEvent.Wait();
            WriteLine($"{threadName}正在执行中");

        }

        /// <summary>
        /// 更灵活的在线程之间传递信号
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var t1 = new Thread(() => TravelThroughGates("Thread 1", 5));
            var t2 = new Thread(() => TravelThroughGates("Thread 2", 6));
            var t3 = new Thread(() => TravelThroughGates("Thread 3", 12));
            t1.Start();
            t2.Start();
            t3.Start();
            Sleep(TimeSpan.FromSeconds(6));
            WriteLine("线程可以执行了");
            _mainEvent.Set();
            Sleep(TimeSpan.FromSeconds(2));
            _mainEvent.Reset();
            WriteLine("线程 将要关闭");
            Sleep(TimeSpan.FromSeconds(10));
            WriteLine("线程将再次重启");
            _mainEvent.Set();
            Sleep(TimeSpan.FromSeconds(2));
            WriteLine("线程又要关闭了");
            _mainEvent.Reset();
            ReadLine();

        }
    }
}
