using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive_ExtensionsDemo
{
    class Program
    {
        /// <summary>i
        /// https://www.cnblogs.com/shanyou/p/3233894.html
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            foreach (int i in Power(2, 8))
            {
                Console.WriteLine("{0} ", i);
            }

            Console.ReadKey();
        }

        public static System.Collections.Generic.IEnumerable<int> Power(int number, int exponent)
        {
            int result = 1;

            for (int i = 0; i < exponent; i++)
            {
                result = result * number;
                yield return result;
            }
        }
    }
}
