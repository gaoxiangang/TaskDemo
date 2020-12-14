using System;
using System.Threading;
using static System.Threading.Thread;

namespace CancelThread
{
    class Program
    {
        /// <summary>
        /// 实现在线程池中实现取消异步操作效果
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            /**
             * AsyncOperations1 通过轮询的方式去查询取消状态
             * AsyncOperations2 通过查询取消状态来抛出一个异常，来执行线程池取消操作
             * AsyncOperations3 通过注册一个回调函数，当操作被取消时，执行该回调函数内部方法，允许传递一个取消逻辑到一个异步操作中
             * *******/
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperations1(token));
                Sleep(TimeSpan.FromSeconds(2));

                cts.Cancel();
            }
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperations2(token));
                Sleep(TimeSpan.FromSeconds(2));

                cts.Cancel();
            }
            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperations3(token));
                Sleep(TimeSpan.FromSeconds(2));

                cts.Cancel();
            }
        }

        static void AsyncOperations1(CancellationToken token)
        {
            Console.WriteLine("开始第一个Task");
            for (int i = 0; i < 5; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("第一个异步任务任务取消");
                    return;
                }
                Sleep(TimeSpan.FromSeconds(2));

            }
            Console.WriteLine("第一个异步任务执行成功!");
        }

        static void AsyncOperations2(CancellationToken token)
        {
            Console.WriteLine("开始第二个Task");
            try
            {


                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Sleep(TimeSpan.FromSeconds(2));

                }
                Console.WriteLine("第二个异步任务执行成功!");
            }
            catch (Exception)
            {
                Console.WriteLine("第二个异步任务任务取消");

            }
        }

        static void AsyncOperations3(CancellationToken token)
        {
            Console.WriteLine("开始第三个Task");

            bool cancellatFlag = false;
            token.Register(() => cancellatFlag = true);

            for (int i = 0; i < 5; i++)
            {
                if (cancellatFlag)
                {
                    Console.WriteLine("第三个异步任务取消成功!");
                    return;
                }
                Sleep(TimeSpan.FromSeconds(2));
            }
            Console.WriteLine("第三个异步任务执行成功!");
        }
    }
}
