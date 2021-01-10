using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static System.Console;
using static System.Threading.Thread;

namespace Reactive_ExtensionsDemo
{
    /// <summary>
    /// 使用Subject
    /// </summary>
    public class Rx_Recipe4
    {


        /******************
         * Subject 代表IObservable 和IObserver这两个接口的实现
         * OutputToConsole  是我们的一个订阅事件，分别是OnNext、onError、onComplete
         * 第一个字符串A不会打印，因为订阅事件发生在之后，当调用OnComplete和OnError时，将停止事件序列传播，因此最后一个字符也没打印出来
         * ReplaySubject 允许我们实现三个附加场景，首先从广播开始可以缓存所有事件，如果开始订阅，将会获取之前缓存的所有事件，这里输出四个字符是因为第一个被缓存并传递给了订阅者
         * 接着顶一个ReplaySubject的缓存大小和时间，接下来只允许设置两个缓存事件，如果广播更多的事件，那只有最后两个被传递给订阅者；
         * 使用时间的也类似，我们可以指定只缓存指定时间内的事件，如果未达到时间以内事件则会被丢弃，而且也不会被订阅，只会订阅在规定时间内的事件
         * AsyncSubject 类似于异步任务中的Task，代表单个异步操作，如果有多个事件等待发布，他会等待事件序列完成，然后只会把最后一个事件传递给订阅者
         * BehaviorSubject 与Replay类似 但是只缓存一个值，并允许万一没发送任何通知时，指定一个默认值。它打印出了四个值，如果将B移动到Default下，将不会见到Default
         * 
         * **/

        public static IDisposable OutputToConsole<T>(IObservable<T> observable) {
            return observable.Subscribe(
                obj => WriteLine($"{obj}"),
                ex => WriteLine($"Error{ex.Message}"),
                () => WriteLine("Complete")
                );
        }

        public static void RuMain()
        {
            WriteLine("Subject");
            var subject = new Subject<string>();

            subject.OnNext("A");
            using (var subscriptiion = OutputToConsole(subject))
            {
                subject.OnNext("B");
                subject.OnNext("C");
                subject.OnNext("D");
                subject.OnCompleted();
                subject.OnNext("Will not be printed out");
            }

            WriteLine("ReplaySubject");
            var replaySubject = new ReplaySubject<string>();
            replaySubject.OnNext("A");
            using (var subscriptiion = OutputToConsole(replaySubject))
            {
                replaySubject.OnNext("B");
                replaySubject.OnNext("C");
                replaySubject.OnNext("D");
                replaySubject.OnCompleted();

            }

            WriteLine("Buffer ReplaySubject");

            var bufferedSubject = new ReplaySubject<string>(2);

            bufferedSubject.OnNext("A");
            bufferedSubject.OnNext("B");
            bufferedSubject.OnNext("C");

            using (var subscription=OutputToConsole(bufferedSubject))
            {
                bufferedSubject.OnNext("D");
                bufferedSubject.OnCompleted();
            }


            WriteLine("Time Window Subject");
            var timeSubject = new ReplaySubject<string>(TimeSpan.FromMilliseconds(200));

            timeSubject.OnNext("A");
            Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("B");
            Sleep(TimeSpan.FromMilliseconds(100));
            timeSubject.OnNext("C");
            Sleep(TimeSpan.FromMilliseconds(100));

            using (var subscriptiion = OutputToConsole(timeSubject))
            {
                Sleep(TimeSpan.FromMilliseconds(300));

                timeSubject.OnNext("D");
                timeSubject.OnCompleted();
            }

            WriteLine("AsyncSubject");

            var asyncSubject = new AsyncSubject<string>();

            asyncSubject.OnNext("A");
            using (var subscription= OutputToConsole(asyncSubject))
            {
                asyncSubject.OnNext("B");
                asyncSubject.OnNext("C");
                asyncSubject.OnNext("D");
                asyncSubject.OnCompleted();
            }

            WriteLine("BehaviorSubject");
            var behaviorSubject = new BehaviorSubject<string>("Default");
            using ( var subscription= OutputToConsole(behaviorSubject))
            {
                behaviorSubject.OnNext("B");
                behaviorSubject.OnNext("C");
                behaviorSubject.OnNext("D");
                behaviorSubject.OnCompleted();
            }
        }
    }
}
