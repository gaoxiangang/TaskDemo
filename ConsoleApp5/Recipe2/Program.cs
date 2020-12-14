using System;
using System.Threading.Tasks;
using static System.Threading.Thread;
namespace Recipe2
{
    class Program
    {
        /// <summary>
        /// 在lambda表达式中使用await字符
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Task task = AsynchronousProcessing();
            task.Wait();
            Console.ReadKey();
        }


        static async Task AsynchronousProcessing()
        {
            /**
             *  Func<string, Task<string>>此方法是一种委托的写法 传入string类型数据 返回值为Task<string>
             * async name 表明 该Lambda表达式方式是可以等待的 即可以使用await
             * ******/
            Func<string, Task<string>> asyncLambda = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                return $"线程{name}在线程:{CurrentThread.ManagedThreadId}上运行，是否是线程池:{CurrentThread.IsThreadPoolThread}";
            };
            string result = await asyncLambda("TaskFunc");
            Console.WriteLine(result);

        }
    }
}
