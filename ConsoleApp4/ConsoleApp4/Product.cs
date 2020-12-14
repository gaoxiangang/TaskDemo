using System.Collections.Generic;

namespace ConsoleApp4
{
    public class Product
    {
        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public Product(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }

        public Product() { }

        public IList<Product> GetProducts()
        {

            var products = new List<Product>();

            products.Add(new Product("苹果", 111));
            products.Add(new Product("橘子", 222));
            products.Add(new Product("香蕉", 333));
            products.Add(new Product("桃子", 444));
            products.Add(new Product("苹果", 111));

            return products;
        }

    }
}
