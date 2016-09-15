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
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        List<StreamWriter> clientWriters;
        AuctionhouseService ahService;
        Screen screen;
        public ClientHandler(Socket clientSocket, int clientNumber , AuctionhouseService ahService , Screen screen )
        {
            this.clientSocket = clientSocket;
            this.clientNumber = clientNumber;
            this.ahService = ahService;
            this.screen = screen;
        }
        internal void Start()
        {
            // Initialize
            this.stream = new NetworkStream(clientSocket);
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            string clientText;

            // Handle client
            do
            {
                clientText = reader.ReadLine();
                screen.PrintLine("Client " + clientNumber + " says: " + clientText);
                Console.WriteLine("Client {0} says: {1}", clientNumber, clientText);

                if (clientText == "query product list")
                {
                    string products = ahService.GetProductsMenu();
                    sendToClient(products);
                }
                else if (clientText.Split(' ')[0] == "product")
                {
                    int chosenProduct = int.Parse(clientText.ToLower().Split(' ')[1]);
                    int productIndex = chosenProduct - 1;
                    clientText = reader.ReadLine(); // waiting for client to place a bid
                    decimal bid = decimal.Parse(clientText);

                    Product product = ahService.GetProductByIndex(productIndex);
                    if (product.IsValidBid(bid))
                    {
                        product.PlaceBid(bid, clientNumber);
                        screen.PrintLine("A bid of " + bid + " kr. has been placed on product Id." + product.Id);
                        sendToClient("A bid of " + bid + " kr. has been placed on product Id." + product.Id);
                    }
                    else
                    {
                        sendToClient("Bid is too low. The product's current bid is "+ product.GetCurrentBid() + " kr.");
                    }
                        
                    
                }
            } while (clientText.ToLower() !="exit" );

            // End
            screen.PrintLine("Client " + clientNumber + " disconnected");
            //Console.WriteLine("Client {0} disconnected.", clientNumber);
            reader.Close();
            writer.Close();
            stream.Close();
            clientSocket.Close();
        } // Start() END
        void sendToClient(string input)
        {
            writer.WriteLine(input);
            writer.Flush();
        }
    }
}