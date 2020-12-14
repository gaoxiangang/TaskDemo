using System;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace Recipe3
{
    class Program
    {
        /// <summary>
        /// 对连续的异步任务使用await 操作符
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //！！！！使用wait 注意如果不使用，异步线程在未结束时主线程结束。那所有的后台线程将会退出。导致执行失败
            Task t = AsyncchronWithTPL();
            t.Wait();
            t = AsyncchronyWithAwait();
            t.Wait();

            Console.ReadKey();

        }

        /// <summary>
        /// 这种方式成为线程的TPL模式
        /// </summary>
        /// <returns></returns>
        static Task AsyncchronWithTPL()
        {
            //启动主任务，在主任务内部加入一些后续操作
            var containerTask = new Task(() =>
            {
                Task<string> t = GetInfoAsync("TPL 1");//步骤1
                //步骤1完成后添加后续任务
                t.ContinueWith(task =>
                {
                    //后续任务完成后打印结果
                    Console.WriteLine(t.Result);
                    //启用步骤2
                    Task<string> t2 = GetInfoAsync("TPL 2");
                    //步骤2完成后继续添加后续任务
                    t2.ContinueWith(innerTask => Console.WriteLine(t2.Result),
                        TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);
                    //在步骤2的后续任务2中。抛出异常来测试异常的处理
                    t2.ContinueWith(innerTask => Console.WriteLine(innerTask.Exception.InnerException), 
                        TaskContinuationOptions.OnlyOnFaulted|TaskContinuationOptions.AttachedToParent);

                },TaskContinuationOptions.NotOnFaulted|TaskContinuationOptions.AttachedToParent);
                //步骤1的后续任务2中一九抛出异常来测试异常处理
                t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException),
                    TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
            });
            //启动线程
            containerTask.Start();
            return containerTask;
        }

        static async Task AsyncchronyWithAwait()
        {
            try
            {
                //执行到此处时。代码看到await会立即返回后续的代码逻辑将由后续的操作任务完成。这时该步骤将是异步操作
                string result = await GetInfoAsync("Async 1");

                Console.WriteLine(result);
                result = await GetInfoAsync("Async 2");
                Console.WriteLine(result);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            Console.WriteLine($"线程 {name} 开始! ");
            await Task.Delay(TimeSpan.FromSeconds(2));

            if (name == "TPL 2")
                throw new Exception("boom!");
            return $"线程{name}正在线程{CurrentThread.ManagedThreadId}上运行 ,是否是线程池线程{CurrentThread.IsThreadPoolThread }";
        }

    }
}
