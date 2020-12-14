using System;
using System.Threading;
using static System.Threading.Thread;

namespace ThreadPoollDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * 如何向线程池中放入异步操作
             * 1.调用AsyncOperation方法，未传递参数
             * 2.调用AsyncOperation 传递 state参数
             * 3.定义一个lambda方法并向内部传递state参数
             * 4.使用c#闭包方式，向异步方法内传递一个以上对象
             * 知识点:此处需要了解第四步c#闭包            
             * *****/
            const int x = 1;
            const int y = 2;
            const string lambdaState = "LambdaState";
            ThreadPool.QueueUserWorkItem(AsyncOperation);
            Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(AsyncOperation, "async state");
            Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"当前线程状态是:{lambdaState},{state}");
                Console.WriteLine($"当前线程ID是:{CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromSeconds(2));
            }, "1111");
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine($"当前线程状态是:{x+y},{lambdaState}");
                Console.WriteLine($"当前线程ID是:{CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromSeconds(2));

            }, lambdaState);
            Sleep(TimeSpan.FromSeconds(2));
            Console.ReadKey();
        }

        private static void AsyncOperation(object state)
        {
            Console.WriteLine($"当前线程状态是:{(state ?? "(null)")}");
            Console.WriteLine($"当前线程ID是{CurrentThread.ManagedThreadId}");          
        }
    }
}
