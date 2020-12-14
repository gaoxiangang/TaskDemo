using System;
using System.Threading.Tasks;

namespace Recipe5
{
    class Program
    {
        /// <summary>
        /// 处理异步操作中的异常
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var task = AsyncchronousProcessing();
            task.Wait();
            Console.ReadKey();
        }


        /*****
         * 总结  
         * 第一种异常任务是最为常见的单个 任务线程的异常 普通的try catch 就可以捕获
         * 第二种的多个任务的异常 在whenAll 等待所有任务结束时。此时的异常时有两个异常 ，而try/cahtch只捕获到一个异常 
         * 第三种 在whenAll等待所有任务完成后，在catch块使用Exception.Flatten(); 将异常层级放入一个新的列表内。从而可以取出所有异常信息
         * 第四种异常，展示在C#6.0中支持在 catch 和finally区域使用await字符
         * **/
        static async Task AsyncchronousProcessing()
        {
            Console.WriteLine("单个异常");

            try
            {
                string result1 = await GetAsyncInfos("单个异常任务", 2);
                Console.WriteLine(result1);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            Console.WriteLine("多个异常");
            Task<string> task1 = GetAsyncInfos("多个异常任务1", 3);
            Task<string> task2 = GetAsyncInfos("多个异常任务2", 2);
            try
            {
                string[] result = await Task.WhenAll(task1, task1);
                Console.WriteLine(result.Length);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }


            Console.WriteLine("AggregateException 类型的多异常");
            task1 = GetAsyncInfos("AggregateException 类型的多异常1", 3);
            task2 = GetAsyncInfos("AggregateException 类型的多异常2", 2);
            Task<string[]> t3 = Task.WhenAll(task1, task1);

            try
            {
                string[] result = await t3;
                Console.WriteLine(result.Length);
            }
            catch
            {

                var ae = t3.Exception.Flatten();
                var exceptions = ae.InnerExceptions;

                Console.WriteLine($"异常捕获{exceptions.Count}");
                foreach (var item in exceptions)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
            }

            Console.WriteLine("在 catch 和finally 块使用异常");

            try
            {
                string result = await GetAsyncInfos("在 catch 和finally 块使用异常_1", 2);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {

                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"catch块区域异常:{ex}");
            }
            finally
            {

                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"finally 区域");
            }
        }


        static async Task<string> GetAsyncInfos(string name, int fromSecond)
        {
            await Task.Delay(TimeSpan.FromSeconds(fromSecond));
            throw new Exception($"Boom！{name}");
        }
    }
}
