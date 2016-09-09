using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class AuctionhouseService
    {
        List<Product> productList;

        public AuctionhouseService()
        {
            productList = new List<Product>();
        }

        internal string GetProducts()
        {
            string products ="Products:\n";
            int productNumber = 0;
            foreach (Product product in productList)
            {
                productNumber++;
                products += productNumber + ": " + product.GetProduct();
            }
            return products;
        }

        internal int GetProductAmount()
        {
            return productList.Count;
        }

        internal void HardcodeProducts()
        {
            Product product1 = new Product(1337, "Rembrandt, The Jewish Pride", DateTime.Now, 2000000, "Painting by Rembrandt", 2, 500000);
            Product product2 = new Product(123, "Lenovo Z50", DateTime.Now, 2000, "Daniel's laptop", 2, 1000);

            productList.Add(product1);
            productList.Add(product2);
        }
    }
}
