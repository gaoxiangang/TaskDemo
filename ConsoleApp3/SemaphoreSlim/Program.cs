using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace SemaphoreSlimDemo
{
    class Program
    {
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(4);

        static void AccessDatabase(string name, int seconds)
        {
            WriteLine($"{name} waits to  access a database ");
            _semaphoreSlim.Wait();
            WriteLine($"{name} 正在授权连接数据库");
            Sleep(TimeSpan.FromSeconds(seconds));

            WriteLine($"{name}授权连接成功!");

            _semaphoreSlim.Release();
        }

        /// <summary>
        /// 限制同时访问资源的线程数量 在限制中的线程可以直接访问，但是超过限制就会被阻塞直到可以进入
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            for (int i = 0; i < 6; i++)
            {
                string str = "Thread" + i;
                int secondToWait = 2 + 2 * i;
                var t = new Thread(() => AccessDatabase(str, secondToWait));
                t.Start();
            }
        }
    }
}
