using System;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            /***
             * 相对于TPL运行模式来说。await 的代码相对简洁很多，可读性也比较高
             * ****/
            Task t = AsynchronyWithTPL();
            t.Wait();
            t = AsynchronyWithAsync();
            t.Wait();
            Console.ReadKey();
        }

        static Task AsynchronyWithTPL()
        {
            Task<string> t = GetInfoAsync("Task with TPL");
            //新线程去打印第一个线程的返回结果 如果第一个线程的状态是失败，则会引发一个异常
            Task task2 = t.ContinueWith(task => Console.WriteLine(t.Result), TaskContinuationOptions.NotOnFaulted);
            //保证第一个线程未引发任何异常的情况下再执行
            Task t3 = t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

            return Task.WhenAny(task2, t3);

        }

        static async Task AsynchronyWithAsync()
        {
            try
            {
                //其实在异步方法内部。程序在执行时遇到await字段时，会自动的创建一个新的任务去执行await后所有的剩余代码GetInfoAsync("TaskWithAsync");
                //这个新的任务会处理后续事务，包括错误的处理然后返回到当前主方法中等待其完成
                string result = await GetInfoAsync("TaskWithAsync");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            throw new Exception("Boom");
            return $"线程{name}在线程:{CurrentThread.ManagedThreadId}上运行，是否是线程池:{CurrentThread.IsThreadPoolThread}";

        }
    }
}
