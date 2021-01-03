using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace PLINQDemo
{
    /// <summary>
    /// 管理PLINQ查询中的数据分区
    /// </summary>
    public class PLINQRecipe5
    {
        /*******************
         * 为了演示PLINQ查询的自定义分区策略，创建一个简单的分区器。用于并行的处理奇数长度与偶数长度的字符串
         * 首先从Partition<string>派生出StringPartitioner,并以string 为参数
         *我们只支持静态分区  通过重载SupportsDynamicPartition设置为false 来进行预定义分区策略
         *静态分区在某一方的工作线程完成工作后不会去帮助另一个线程去分担工作，而动态分区则是实时进行分区，使得工作者线程平衡工作负载
         * GetPartitions定义两个迭代第一个从源集合中返回奇数长度字符串，另一个返回偶数长度字符串
         * 最后创建一个分区实例并使用PLINQ查询  我们会看到不同的线程处理奇数长度和偶数长度的字符串
         * 
         * **/
        public static void PrintInfo(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(150));
            WriteLine($"{typeName}被线程{CurrentThread.ManagedThreadId}打印完成！");
        }

        public static string EmulateProcessing(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(150));
            WriteLine($"{typeName}被线程{CurrentThread.ManagedThreadId}处理完成！长度为{(typeName.Length%2==0?"Even":"Odd")}");
            return typeName;
        }

        public static IEnumerable<string> GetTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetExportedTypes());

            return from type in types
                   where type.Name.StartsWith("Web")
                   select type.Name;
        }

        public static void RunMain()
        {
            var timer = Stopwatch.StartNew();

            var partitioner = new StringPartitioner(GetTypes());

            var partitionQuery = from t in partitioner.AsParallel()
                                  //.WithDegreeOfParallelism(1)//设置为1 按照顺序执行 增加参数则开始并行处理
                                 select EmulateProcessing(t);

            partitionQuery.ForAll(PrintInfo);

            int count = partitionQuery.Count();

            timer.Stop();

            WriteLine("------");
            WriteLine($"结果总数为{count}");
            WriteLine($"用时{timer.Elapsed}");

        }
    }

    public class StringPartitioner : Partitioner<string>
    {
        private readonly IEnumerable<string> _data;

        public StringPartitioner(IEnumerable<string> data)
        {
            _data = data;
        }

        public override bool SupportsDynamicPartitions => false;

        public override IList<IEnumerator<string>> GetPartitions(int partitionCount)
        {
            var result = new List<IEnumerator<string>>(partitionCount);

            for (int i = 1; i <= partitionCount; i++)
            {
                result.Add(CreateEnumerator(i, partitionCount));
            }
            return result;
        }

        IEnumerator<string> CreateEnumerator(int partitionNumber, int partitionCount)
        {
            int evenPartitions = partitionCount / 2;

            bool isEven = partitionCount % 2 == 0;

            int step = isEven ? evenPartitions : partitionCount - evenPartitions;

            int startIndex = partitionNumber / 2 + partitionNumber % 2;

            var query = _data.Where(o => !(o.Length % 2 == 0 ^ isEven) || partitionCount == 1)
             
                .Skip(startIndex - 1);

            return query.Where((x, i) => i % step == 0).GetEnumerator();

        }
    }
}
