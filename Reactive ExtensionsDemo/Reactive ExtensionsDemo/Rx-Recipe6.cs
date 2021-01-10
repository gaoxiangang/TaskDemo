using System;
using System.Reactive.Linq;
using static System.Console;
namespace Reactive_ExtensionsDemo
{
    /// <summary>
    /// 对可观察的集合使用linq查询
    /// </summary>
    public class Rx_Recipe6
    {
        /******************
         * 对可观察事件序列使用LINQ的能力是Reactive Extensions框架的主要优势，有很多场景，这里只展示linq查询应用到异步的可观察的集合的精华
         * 使用Observable事件创建一个数字队列 每个数字用时50毫秒 我们从0开始产生21个事件。然后将linq查询结合到该序列
         * 首先只选择奇数 然后再选择偶数，最后再链接这两个序列
         * 最后查询使用最有用的Do 其允许引入副作用。例如从结果序列记录每个值
         * **/

        public static void RunMain()
        {
            IObservable<long> sequence = Observable.Interval(TimeSpan.FromMilliseconds(50)).Take(21);

            var evenNumbers = from n in sequence
                              where n % 2 == 0
                              select n;

            var oddNumbers = from n in sequence
                             where n % 2 != 0
                             select n;

            var combine = from n in evenNumbers.Concat(oddNumbers)
                          select n;

            var nums = (from n in combine
                        where n % 5 == 0
                        select n).Do(n => WriteLine($"---------Number {n} is Proessed in Do method"));

            using (var sub = OutputToConsole(sequence, 0)) ;
            using (var sub2 = OutputToConsole(combine, 1)) ;
            using (var sub3 = OutputToConsole(nums, 2))
            {
                WriteLine("Press enter to finish the demom");
                ReadLine();
            }         
        }

        public static IDisposable OutputToConsole<T>(IObservable<T> sequence, int innerLevel)
        {
            string delimiter = innerLevel == 0 ? string.Empty : new string('-', innerLevel * 3);

            return sequence.Subscribe(
                obj => WriteLine($"{delimiter}{obj}"),
                ex => WriteLine($"Error: {ex.Message}"),
                () => WriteLine($"{delimiter} Complete")
            );
        }
    }
}
