using System;
using System.Diagnostics;
using System.Threading;
using static System.Threading.Thread;


namespace ThreadVSThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {    

            /**
             * 分别执行五百次的异步操作，时间上来说一直开启线程比将方法放入线程池来执行快的多，但是线程池在资源的消耗上要低的多1
             * ******/
            const int numOfOperations = 500;
            var sw = new Stopwatch();
            sw.Start();
            UseThreads(numOfOperations);
            sw.Stop();
            Console.WriteLine($"线程工作完成使用时间为:{sw.ElapsedMilliseconds}");
            sw.Restart();

            UseThreadPool(numOfOperations);
            sw.Stop();

            Console.WriteLine($"线程池工作完成使用时间为:{sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }


        static void UseThreads(int numOfOperation)
        {
            using (var countdown = new CountdownEvent(numOfOperation))
            {
                Console.WriteLine("程序开始创建线程");
                for (int i = 0; i < numOfOperation; i++)
                {
                    var thread = new Thread(() =>
                    {

                        Console.WriteLine($"当前执行的线程ID:{CurrentThread.ManagedThreadId}");
                        Sleep(TimeSpan.FromSeconds(2));
                        countdown.Signal();
                    });
                    thread.Start();
                }
                countdown.Wait();
                Console.WriteLine("线程工作结束.....");

            }
        }

        static void UseThreadPool(int numOfOpertaions)
        {

            using (var countdown = new CountdownEvent(numOfOpertaions))
            {
                Console.WriteLine("开始创建工作线程池");
                for (int i = 0; i < numOfOpertaions; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {

                        Console.WriteLine($"当前执行的线程ID:{CurrentThread.ManagedThreadId}");
                        Sleep(TimeSpan.FromSeconds(2));
                        countdown.Signal();
                    });
                }
                countdown.Wait();
            }
        }
    }
}
