using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace PLINQDemo
{
    /// <summary>
    /// 为PLINQ查询自定义聚合器
    /// </summary>
    public class PLINQRecipe6
    {

        /********
         * 实现了可以工作与PLINQ查询的自定义聚合机制，由于一个查询会被多个任务同时执行，那么久需要一种机制来并行的聚合每个任务的结果
         * 通过定义ParallelEnumerable类中的Aggregate扩展方法来聚合PLINQ查询结果。分为四个部分
         * 1是一个工厂类，构造了该聚合器空的初始值，即要要对其进行聚合的序列
         * 2将每个集合项聚合到分区聚合对象中。这里的聚合对象在每个分区中是不一样的
         * 3是一个高阶聚合函数，用于将分区内的聚合结果合并到全局聚合对象中
         * 4是一个选择器函数，指定全局对象中我们需要的确切数据
         * 最后按照使用字符的频率从高到底打印出结果
         * **/
        public static ConcurrentDictionary<char, int> AccumulateLettersInfomation(ConcurrentDictionary<char, int> taskTotal, string item)
        {

            foreach (var c in item)
            {
                if (taskTotal.ContainsKey(c))
                {
                    taskTotal[c] = taskTotal[c] + 1;

                }
                else
                {
                    taskTotal[c] = 1;
                }
            }

            WriteLine($"{item}已经在线程{CurrentThread.ManagedThreadId}上被添加");

            return taskTotal;
        }

        public static ConcurrentDictionary<char, int> MergeAccumulators(ConcurrentDictionary<char, int> total, ConcurrentDictionary<char, int> taskTotal)
        {

            foreach (var key in taskTotal.Keys)
            {
                if (total.ContainsKey(key))
                {

                    total[key] = total[key] + taskTotal[key];
                }
                else
                {
                    total[key] = taskTotal[key];
                }
            }

            WriteLine($"合计聚合值在线程{CurrentThread.ManagedThreadId}上被计算完成");

            return total;

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

            var parallelQuery = from t in GetTypes().AsParallel()
                                select t;
            var parallelAggregator = parallelQuery.Aggregate(
                () => new ConcurrentDictionary<char, int>(),
                (taskTotal, item) => AccumulateLettersInfomation(taskTotal, item),
                (total, taskTotal) => MergeAccumulators(total, taskTotal),
                total => total
             );

            WriteLine();

            WriteLine("以下字母时是Type名称");
            var orderedKeys = from k in parallelAggregator.Keys
                              orderby parallelAggregator[k] descending
                              select k;

            foreach (var c in orderedKeys)
            {
                WriteLine($"使用 '{c}'-----{parallelAggregator[c]} 次");
            }
        }
    }
}
