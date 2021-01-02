using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;
namespace PLINQDemo
{
    public static class ParallelClass
    {

        public static string EmulateProcessing(string taskName,int fromSecond)
        {
            Sleep(TimeSpan.FromSeconds(fromSecond));
            Console.WriteLine($"{taskName}在线程{CurrentThread.ManagedThreadId}上被处理!{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return taskName;
        }

        public static void RunMain()
        {
            
            //简化的并行任务，会等待所有任务完成，在有任务未完成时，会一直阻塞线程
            Parallel.Invoke(
                () => EmulateProcessing("Task1",2),
                () => EmulateProcessing("Task2",4),
                () => EmulateProcessing("Task3",6)
            );

            Console.WriteLine($"Invoke Done{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            var cts = new CancellationTokenSource();
          
           var result = Parallel.ForEach(
                Enumerable.Range(1, 30),//个人理解为迭代次数
                new ParallelOptions//为每次迭代设置配置参数
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default
                }, (i, state) =>//每次迭代设置Action 委托实例
                {

                    Console.WriteLine(i);
                    if (i == 20)
                    {
                        state.Break();//stop 和break的区别。stop直接停止所有任务，包括正在运行中，而break则是停止后续任务，继续运行当前已执行任务
                        Console.WriteLine($"循环停止{state.IsStopped}");
                    }
                });
            Console.WriteLine("----------");
            Console.WriteLine($"是否完成:{result.IsCompleted}");
            Console.WriteLine($"最小断点迭代:{result.LowestBreakIteration}");
        }
    }
}
