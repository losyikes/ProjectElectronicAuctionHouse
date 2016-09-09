using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class ClientHandler
    {
        private Socket clientSocket;
        private int clientNumber;

        public ClientHandler(Socket clientSocket, int clientNumber)
        {
            this.clientSocket = clientSocket;
            this.clientNumber = clientNumber;
        }
        internal void Start()
        {
            // Initialize
            NetworkStream stream = new NetworkStream(clientSocket);
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            AuctionhouseService ahService = new AuctionhouseService();
            ahService.HardcodeProducts();
            string clientText;

            // Handle client
            do
            {
                clientText = reader.ReadLine();
                Console.WriteLine("Client {0} says: {1}", clientNumber, clientText);

                if (clientText == "query product list")
                {
                    string products = ahService.GetProducts();
                    writer.WriteLine(products);
                    writer.Flush();
                }
                else if (clientText.Split(' ')[0] == "product")
                {
                    int chosenProduct = int.Parse(clientText.ToLower().Split(' ')[1]);
                    int productIndex = chosenProduct - 1;
                    clientText = reader.ReadLine(); // waiting for client to place a bid
                    decimal bid = decimal.Parse(clientText);
                    /* We need to make AboveCurrentBid(), UpdateProduct() and GetCurrentBid()
                    bool aboveCurrentBid = ahService.AboveCurrentBid(productIndex);
                    if (aboveCurrentBid == true)
                    {
                        ahService.UpdateProduct(productIndex, bid); // -1, so it sends the correct index number
                        writer.WriteLine("A bid of {0} kr. has been placed on product nr. {1}", bid, chosenProduct);
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("Bid is too low. The product's current bid is {0} kr.", ahService.GetCurrentBid(productIndex));
                        writer.Flush();
                    }
                    */
                }
            } while (clientText.ToLower() !="exit" );

            // End
            Console.WriteLine("Client {0} disconnected.", clientNumber);
            reader.Close();
            writer.Close();
            stream.Close();
            clientSocket.Close();
        }
    }
}