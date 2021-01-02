using System;
using System.Collections.Generic;
using System.Linq;

namespace PLINQDemo
{
    public class PLINQRecipe4
    {

        /********
         * PLINQ 的异常处理
         * 第一个例子使用正常的linq语句执行foreach循环可以清楚地得到错误信息
         * 第二个例子，由于我们使用并行执行，由于程序并行执行，因此返回的错误集合中包含所有运行期间放生的异常
         * 因此返回的结果集合为AggregateException  我们可以使用Flatten 和Handle来处理集合中的异常信息
         * **/
        public static void RunMain()
        {

            IEnumerable<int> numbers = Enumerable.Range(-5, 10);

            var query = from number in numbers
                        select 100 / number;

            try
            {
                foreach (var num in query)
                {
                    Console.WriteLine(num);
                }
            }
            catch (DivideByZeroException)
            {

                Console.WriteLine("将整数除以0引发的异常");
            }

            Console.WriteLine("------");
            Console.WriteLine();


            var parallQuery = from number in numbers.AsParallel()
                              select 100 / number;
            try
            {
                parallQuery.ForAll(Console.WriteLine);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("除以0，异常 BY DivideByZeroException");

            }
            catch (AggregateException e)
            {
                e.Flatten().Handle(ex =>
                {
                    if (ex is DivideByZeroException)
                    {
                        Console.WriteLine("除以0，异常 BY AggregateException=>DivideByZeroException");
                        return true;
                    }
                    return false;
                });
            }

        }
    }
}
