using System;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace Recipe6
{
    class Program
    {

        /// <summary>
        /// 使用async Void 方法
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Task t = AsyncTask();
            t.Wait();

            t = AsyncTaskWithErrors();
            while (!t.IsFaulted)
            {
                Sleep(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine(t.Exception);
        }

        static async Task<string> GetAsyncInfos(string name, int fromSecond)
        {
            await Task.Delay(TimeSpan.FromSeconds(fromSecond));

            if (name.Contains("Exception"))
            {
                throw new Exception($"Boom from {name}");
            }
            return $"线程{name}在线程:{CurrentThread.ManagedThreadId}上运行，是否是线程池:{CurrentThread.IsThreadPoolThread}";
        }

        static async Task AsyncTaskWithErrors()
        {
            string result = await GetAsyncInfos("AsyncTaskWithErrors", 2);
            Console.WriteLine(result);
        }

        static  async void AsyncVoidWithErrors()
        {
            string result = await GetAsyncInfos("AsyncVoidWithException", 2);
            Console.WriteLine(result);
        }

        static async Task AsyncTask()
        {
            string result = await GetAsyncInfos("AsyncTask", 2);
            Console.WriteLine(result);
        }

        static async void AsyncVoid()
        {
            string result = await GetAsyncInfos("AsyncVoid", 2);
            Console.WriteLine(result);
        }
    }
}
