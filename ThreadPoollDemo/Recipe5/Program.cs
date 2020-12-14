using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe5
{
    class Program
    {
        /// <summary>
        /// 在线程池中使用等待事件处理器及超时
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            RunOpoerations(TimeSpan.FromSeconds(5));
            RunOpoerations(TimeSpan.FromSeconds(7));
            ReadKey();
        }

        static void RunOpoerations(TimeSpan workerOperationTimeOut)
        {
            using (var evt = new ManualResetEvent(false))
            {
                using (var cts = new CancellationTokenSource())
                {
                    WriteLine("注册超时行为...");
                    var worker = ThreadPool.RegisterWaitForSingleObject(evt, (state, isTimeOut) => WorkerOperationWait(cts, isTimeOut), null, workerOperationTimeOut, true);
                    WriteLine("开始执行工作....");
                    ThreadPool.QueueUserWorkItem(_ => WorkerOperation(cts.Token, evt));
                    Sleep(workerOperationTimeOut.Add(TimeSpan.FromSeconds(2)));
                }
            }

        }

        static void WorkerOperation(CancellationToken token, ManualResetEvent evt)
        {
            for (int i = 0; i < 6; i++)
            {
                if (token.IsCancellationRequested)
                {

                    return;
                }
                Sleep(TimeSpan.FromSeconds(1));
            }
            WriteLine("未超时！！！！");
            evt.Set();
        }


        static void WorkerOperationWait(CancellationTokenSource cts, bool isTimeOut)
        {
            if (isTimeOut)
            {
                cts.Cancel();
                WriteLine("工作已经超时，并已取消");
            }
            else
            {
                WriteLine("工作已完成");
            }
        }
        /***
         * 代码含义
         * 使用ThreadPool中RegisterWaitForSingleObject  
         * 注册一个等待的委托(ManualResetEvent)，并制定一个超时时间(workerOperationTimeOut)，设置超时或者工作完成后的回调方法(WorkerOperationWait)
         * 将方法放入线程池执行异步操作 (方法内部for循环每次循环完成就线程睡眠一秒)
         * 如果睡眠时间小于超时时间，则表示工作在超时时间内完成 isTimeOut=false，否则则超时 isTimeOut=true
         * 超时则执行线程取消操作，取消异步执行       
         * -----应用场景
         * 当程序中有大量线程处于阻塞状态去等待一些多线程事件发信号时，以上方式很有用
         * 借助此操作我们不需阻塞所有线程，可以释放这些线程直到事件被设置（evt.Set();）
         * 具有高伸缩以及高性能
         * *****/
    }
}
