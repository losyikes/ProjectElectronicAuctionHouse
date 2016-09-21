using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace AuctionhouseServer
{
    class AuctionhouseService
    {
        public List<StreamWriter> clientWriters { get; set; }
        public AuctionhouseServer server;
        List<Product> productList;
        
        public AuctionhouseService(AuctionhouseServer server)
        {
            productList = new List<Product>();
            this.server = server;
            HardcodeProducts();
        }
        internal string GetProductsMenu()
        {
            string products ="Products:\n";
            int productNumber = 0;
            foreach (Product product in productList)
            {
                if(product.AuctionStatus != 2)
                {
                    productNumber++;
                    products += productNumber + ": " + product.GetProduct();
                }
            }
            return products;
        }
        
        public void BroadcastToAllClientsInLocation(string input, int productId)
        {
            foreach(ClientHandler ch in server.ClientHandlers)
            {
                if(productId == ch.ChosenProductId && productId != 0)
                {
                    Product product = this.GetProductById(productId);
                    ch.writer.WriteLine(input);
                    ch.writer.Flush();
                    if (product.CurrentBid.ClientID != ch.clientNumber )
                    {
                        if(product.LastBid != null && product.LastBid.ClientID == ch.clientNumber)
                        {
                            ch.writer.WriteLine("You have been outbid by another please bid again");
                            ch.writer.Flush();
                            server.screen.PrintLine("outbid msg sent to client " + ch.clientNumber);
                        }
                    }
                }
            }
        }

        public Product GetProductByIndex(int productIndex)
        {
            Product product = productList[productIndex];
            return product;
        }

        public Product GetProductById(int productId)
        {
            Product product = productList.Where(x => x.Id == productId).FirstOrDefault();
            return product;
        }

        internal void HardcodeProducts()
        {
            Product product1 = new Product(1337, "Rembrandt, The Jewish Pride", DateTime.Now, 2000000, "Painting by Rembrandt", 0, 500000);
            Product product2 = new Product(123, "Lenovo Z50", DateTime.Now, 2000, "Daniel's laptop", 0, 1000);

            productList.Add(product1);
            productList.Add(product2);
        }
    }
}
