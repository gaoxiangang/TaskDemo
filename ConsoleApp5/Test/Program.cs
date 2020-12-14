using Newtonsoft.Json;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            string result = GetProducts();
            test<Product>(result);

        }
        public static string GetProducts()
        {
            IList<Product> products = new List<Product>();

            products.Add(new Product()
            {
                ProductCode = "00544",
                ProductName = "花生瓜子矿泉水",
                ProductPrice = 1111,
                ProductLevel = 2,
                IsActive = true

            });
            products.Add(new Product()
            {
                ProductCode = "00544",
                ProductName = "啤酒饮料八宝粥",
                ProductPrice = 1111,
                ProductLevel = 2,
                IsActive = true

            });
            products.Add(new Product()
            {
                ProductCode = "00544",
                ProductName = "前边的腿收一收了",
                ProductPrice = 1111,
                ProductLevel = 2,
                IsActive = true

            });

            ResultData resultData = new ResultData();
            resultData.code = "200";
            resultData.msg = "调用成功";
            resultData.data = JsonConvert.SerializeObject(products);

            return JsonConvert.SerializeObject(resultData);
        }


        public static IList<T> test<T>(string result) where T : class
        {
            IList<T> ts = new List<T>();

            ResultData resultData = JsonConvert.DeserializeObject<ResultData>(result);

            ts = JsonConvert.DeserializeObject<List<T>>(resultData.data);

            return ts;
        }
    }
}
