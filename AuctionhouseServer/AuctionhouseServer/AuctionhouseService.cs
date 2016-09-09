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
            Product product = new Product(1337, "Rembrandt, The Jewish Pride", DateTime.Now, 2000000, "Painting by Rembrandt", 2, 500000);
            productList.Add(product);
        }
    }
}
