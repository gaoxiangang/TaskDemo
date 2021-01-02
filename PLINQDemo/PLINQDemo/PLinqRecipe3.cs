using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Threading.Thread;

namespace PLINQDemo
{
    /********
     * 调整PLINQ查询的参数
     * 首先创建一个Plinq的查询 
     * 然后创建一个调整PLINQ选项的查询
     * 首先从取消选项开始。WithCancellation可以接收一个取消事件；在执行三秒后执行取消事件然后接收异常，并且取消剩余工作
     * 接下来我们为查询设置最大并行度WithDegreeOfParallelism，如果决定使用哪个较小的并行度以节省资源和提高性能，那么并行度就会小于最大值
     * WithExecutionMode  PLINQ基础设置如果认为并行化查询会增加工作量并且运行速度慢，那么将以顺序模式执行。当然我们可以强制设置它以并行化方式进行
     * WithMergeOptions  默认模式是PLINQ 基础设施在查询结果返回之前会缓存一部分结果，但是如果查询花费大量时间，更合理的方式是关闭缓存尽可能的返回结果
     * AsOrdered 当使用并行执行时，集中的项可能不是被顺序执行，我们可以设置其按照顺序方式来执行
     * *****/
    public class PLinqRecipe3
    {
        public static string EmulateProcessing(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine($"{typeName}被线程{CurrentThread.ManagedThreadId}处理完成！");
            return typeName;
        }
        public static IEnumerable<string> GetTypes()
        {

            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetExportedTypes()
                   where type.Name.StartsWith("Web")
                   orderby type.Name.Length
                   select type.Name;
        }

        public static void RunMain()
        {
            var parallelQuery = from t in GetTypes().AsParallel()
                                select EmulateProcessing(t);

            var cts = new CancellationTokenSource();

            cts.CancelAfter(TimeSpan.FromSeconds(3));
            try
            {
                parallelQuery
                    .WithDegreeOfParallelism(Environment.ProcessorCount)//设置最大并行度的数目
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)//设置PLINQ 强制执行查询以并行的方式执行
                    .WithMergeOptions(ParallelMergeOptions.Default)//对查询结果进行处理，在输出结果之前会缓存一定数量的结果到缓存区。
                    .WithCancellation(cts.Token)
                    .ForAll(Console.WriteLine);
            }
            catch (OperationCanceledException)
            {

                Console.WriteLine("--------");
                Console.WriteLine("操作已被取消");
            }
            Console.WriteLine("--------");
            Console.WriteLine("不是顺序处理的PLINQ查询方式");


            var unorderedQuery = from i in ParallelEnumerable.Range(1, 30)
                                 select i;

            foreach (var i in unorderedQuery)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("--------");
            Console.WriteLine("按照集合顺序处理的PLINQ查询方式");
            var orderedQuery = from i in ParallelEnumerable.Range(1, 30).AsOrdered()
                               select i;

            foreach (var i in orderedQuery)
            {
                Console.WriteLine(i);
            }
        }
    }
}
