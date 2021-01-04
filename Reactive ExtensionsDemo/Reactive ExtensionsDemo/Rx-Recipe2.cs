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
            foreach (int i in EnumerableEventSequence())
            {
                WriteLine(i);
            }

            WriteLine();
            WriteLine("IEnumerable");
            WriteLine($"花费{st.Elapsed}");

            st.Restart();
            IObservable<int> observable = EnumerableEventSequence().ToObservable();

            using (IDisposable subscription = observable.Subscribe(Write))
            {
                WriteLine();
                WriteLine("IObservable");
                WriteLine($"花费{st.Elapsed}");
            }

            st.Restart();
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
