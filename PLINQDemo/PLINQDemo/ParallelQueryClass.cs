using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Threading.Thread;

namespace PLINQDemo
{
    public class ParallelQueryClass
    {

        public static void PrintInfo(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(150));
            Console.WriteLine($"{typeName}被线程{CurrentThread.ManagedThreadId}打印完成！");
        }

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
                   select type.Name;
        }


        public static void RunMain()
        {
            var sw = new Stopwatch();
            sw.Start();


            /*********
             * 第一个片段 按照正常的linq方式运行 和输出
             * 第二个片段使用并行的方式去处理结果集(即并行异步)，打印结果操作依然是合并到同一个线程内完成
             * 第三个片段同样使用并行的方式去处理结果集，不同的是用forAll的方式去轮询打印结果集。处理和打印的线程是同一个但是跳过了结果合并步骤
             * 最后展示使用AsSequential 的方法将PLINQ查询以顺序化方式运行 类似强行将并行操作转换成普通的循环
             * **/

            var query = from t in GetTypes()
                        select EmulateProcessing(t);

            foreach (string typeName in query)
            {
                PrintInfo(typeName);
            }

            sw.Stop();
            Console.WriteLine("----");

            Console.WriteLine($"Time elased :{sw.Elapsed}");
            Console.WriteLine("按任意键继续.....");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Restart();


           

            var parallelQuery = from t in GetTypes().AsParallel()
                                select EmulateProcessing(t);

            foreach (string typeName in parallelQuery)
            {
                PrintInfo(typeName);
            }

            sw.Stop();

            Console.WriteLine("----");
            Console.WriteLine("Parallel linq Query 结果集将在单个线程上被合并！ ");

            Console.WriteLine($"Time elased :{sw.Elapsed}");
            Console.WriteLine("按任意键继续.....");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();

          

            parallelQuery = from t in GetTypes().AsParallel()
                            select EmulateProcessing(t);

            parallelQuery.ForAll(PrintInfo);

            sw.Stop();

            Console.WriteLine("----");
            Console.WriteLine("Parallel linq Query 结果集在Parallel被处理！ ");

            Console.WriteLine($"Time elased :{sw.Elapsed}");
            Console.WriteLine("按任意键继续.....");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();

           

            query = from t in GetTypes().AsParallel().AsSequential()
                    select EmulateProcessing(t);

            foreach (string typeName in query)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            Console.WriteLine("----");
            Console.WriteLine("Parallel linq Query 转换到Sequential！ ");

            Console.WriteLine($"Time elased :{sw.Elapsed}");
            Console.WriteLine("按任意键继续.....");
            Console.ReadLine();
            Console.Clear();

        }
    }
}
