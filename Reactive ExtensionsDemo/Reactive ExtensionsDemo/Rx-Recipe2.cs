using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace Reactive_ExtensionsDemo
{
    public class Rx_Recipe2
    {
        /// <summary>
        /// 声明一个可枚举集合
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> EnumerableEventSequence()
        {
            for (int i = 0; i < 10; i++)
            {
                Sleep(TimeSpan.FromSeconds(0.5));
                yield return i;
            }

        }

        public static void RunMain()
        {
            var st = new Stopwatch();
            st.Start();
            //从集合中循环打印数据
            foreach (int i in EnumerableEventSequence())
            {
                WriteLine(i);
            }

            WriteLine();
            WriteLine("IEnumerable");
            WriteLine($"花费{st.Elapsed}");

            st.Restart();
            //将集合转换成可观察的集合
            IObservable<int> observable = EnumerableEventSequence().ToObservable();

            //Subscribe 订阅该集合的更新
            using (IDisposable subscription = observable.Subscribe(Write))
            {
                //在每次更新该集合时打印数据 类似做切换数据库服务器操作
                WriteLine();
                WriteLine("IObservable");
                WriteLine($"花费{st.Elapsed}");
            }

            st.Restart();

            //SubscribeOn将集合转换成可观察的异步集合，并放入TPL任务池中，并卸载主线程的任务
            observable = EnumerableEventSequence().ToObservable()
                .SubscribeOn(TaskPoolScheduler.Default);
            using (IDisposable subscription =observable.Subscribe(Write))
            {
                WriteLine();
                WriteLine("IObservablr Async");

                ReadLine();
            }
            WriteLine($"花费{st.Elapsed}");
        }

    }
}
