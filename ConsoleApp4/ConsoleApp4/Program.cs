using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Product product = new Product();

            IList<Product> products = product.GetProducts();

            //返回序列中的第一个元素，但是如果序列为空 则会报错 "序列中不包含任何元素"
            products.Where(o => o.Price > 200).First();
            //返回序列中的第一个元素，但是如果序列为空  则会返回默认值
            products.Where(o => o.Price > 200).FirstOrDefault();
            //筛选出集合中价格大于200的产品集合
            products.Where(o => o.Price > 200).ToList();
            //按照价格升序排列
            products.OrderBy(o => o.Price);
            products.OrderBy(o => o.Price).ThenBy(o => o.Name);
            //按照价格倒叙排列
            products.OrderByDescending(o => o.Price);
            products.OrderByDescending(o => o.Price).ThenByDescending(o => o.Name);
            //按照产品名称进行分组 --单个元素分组
            products.GroupBy(o => o.Name);
            //按照名称/价格分组
            products.GroupBy(o => new { o.Name, o.Price });
            //按照名称和价格分组，再将其分组完的数据进行处理，得出分组后的数量
            var sum = products.GroupBy(o => new { o.Price, o.Name }).Select(k => new { Product = k.Key, Count = k.Count() }).ToList();

            int pageSize = 1, pageNum = 1;
            while (pageSize * pageNum < products.Count())
            {
                var  list = products.Skip(pageSize * pageNum).Take(pageSize).ToList();
                foreach (var item in list)
                {
                    Console.WriteLine($"Name:{item.Name},Price:{item.Price}");
                }
                pageNum++;
            }




            //Console.Write($"产品:{product.Name}，价格是:{product.Price}");
            Console.ReadKey();


        }
    }
}
