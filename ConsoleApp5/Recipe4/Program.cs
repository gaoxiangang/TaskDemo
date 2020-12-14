using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Recipe4
{
    class Program
    {
        /// <summary>
        /// 对并行任务使用await字符
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            var task = AsyncchronousProcessing();

            task.Wait();
        }

        /// <summary>
        /// 同时执行两个Task线程
        /// </summary>
        /// <returns></returns>
        static async Task AsyncchronousProcessing()
        {
            Task<string> t1 = GetAsyncInfos("歪头傻逼", 2);
            Task<string> t2 = GetAsyncInfos("歪头傻逼", 3);

            string[] result = await Task.WhenAll(t1, t2);
            result.ToList().ForEach(o =>
            {
                Console.WriteLine(o);
            });

            //PS 连续异步任务和并行任务  区别在于 连续任务是在y同一个Task上多次执行
            //并行是一段时间内执行多个Task 而不是在同一个Task上执行
        }


        static async Task<string> GetAsyncInfos(string name, int fromSecond)
        {
            await Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(fromSecond)));
            return name;
        }
    }
}
