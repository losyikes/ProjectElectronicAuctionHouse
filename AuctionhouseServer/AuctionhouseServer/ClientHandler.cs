using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AuctionhouseServer
{
    class ClientHandler
    {
        public int Location { get; set; }
        private Socket clientSocket;
        public int clientNumber;
        NetworkStream stream;
        public StreamWriter writer;
        StreamReader reader;
        AuctionhouseService ahService;
        Screen screen;
        int chosenProduct;
        string clientText  = "";
        public ClientHandler(Socket clientSocket, int clientNumber , AuctionhouseService ahService , Screen screen )
        {
            this.clientSocket = clientSocket;
            this.clientNumber = clientNumber;
            this.ahService = ahService;
            this.screen = screen;
            this.Location = 0;
        }
        internal void Start()
        {
            // Initialize
            this.stream = new NetworkStream(clientSocket);
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            
            
            sendToClient("Welcome to EAL Auctionhouse!");
            // Handle client
            do
            {
                if (isValidInput(clientText))
                {
                    int input;
                    decimal decInput = 0;
                    decimal bid = 0;
                    if (int.TryParse(clientText, out input))
                    {
                        if (decimal.TryParse(clientText, out decInput) && Location == 2)
                        {
                            bid = decInput;
                        }
                    }
                    
                    if (Location == 1 && input != 0)
                    {
                        showProductMenu(input);
                        chosenProduct = input;
                        Location = 2;
                    }
                    else if (Location == 2 && bid != 0)
                    {
                        showBidMenu(chosenProduct, decInput);
                    }
                    else
                        Location = 0;
                }
                else if(Location == 0)
                {
                    showMainMenu();
                }
                else
                {
                    sendToClient("Invalid input try again");
                }
                clientText = reader.ReadLine();
                screen.PrintLine("Client " + clientNumber + " says: " + clientText);
            } while (clientText.ToLower() !="exit" );
            
            // End
            screen.PrintLine("Client " + clientNumber + " disconnected");
            
            reader.Close();
            writer.Close();
            stream.Close();
            clientSocket.Close();
        } // Start() END
        void showProductMenu(int chosenProduct)
        {
            int productIndex = chosenProduct - 1;
            Product product = ahService.GetProductByIndex(productIndex);

            sendToClient(product.GetProduct());
            sendToClient("Please place your bid");
            screen.PrintLine("Info for Product Id. " + product.Id + " sent to Client " + clientNumber);
        }
        void showBidMenu(int chosenProduct, decimal bid)
        {
            int productIndex = chosenProduct - 1;
            //clientText = reader.ReadLine(); // waiting for client to place a bid
            Product product = ahService.GetProductByIndex(productIndex);

            if (product.IsValidBid(bid))
            {
                product.PlaceBid(bid, clientNumber);
                screen.PrintLine("A bid of " + bid + " kr. has been placed on product Id." + product.Id);
                ahService.BroadcastToAllClientsInLocation("Current Bid on product Id. " + product.Id + " is " + product.GetCurrentBid() + " kr", Location);
                sendToClient("Your bid of " + bid + " kr. has been placed on product Id." + product.Id);
                showBidStatusMenu(chosenProduct);
            }
            else
            {
                sendToClient("Bid is too low. The product's current bid is " + product.GetCurrentBid() + " kr.");
            }
        }
        
        void showBidStatusMenu(int chosenProduct)
        {
            int productIndex = chosenProduct - 1;
            Product product = ahService.GetProductByIndex(productIndex);
            sendToClient(product.GetProduct());
            sendToClient("Your current bid is winning, we will tell you when its not");
        }
        void showMainMenu()
        {
            string products = ahService.GetProductsMenu();
            sendToClient(products);
            //Thread.Sleep(300);
            sendToClient("Which product would you like to bid on ? ");
            Location = 1;
        }
        bool isValidInput(string clientText)
        {
            bool isValid = false;
            int number;
            decimal decNumber = 0;

            if (int.TryParse(clientText, out number) || decimal.TryParse(clientText, out decNumber))
                return true;
            
            return isValid;
        }
        
        
        void sendToClient(string input)
        {
            writer.WriteLine(input);
            writer.Flush();
        }
    }
}