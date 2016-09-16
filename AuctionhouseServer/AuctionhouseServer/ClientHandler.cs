using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AuctionhouseServer
{
    class ClientHandler
    {
        public int MenuLocation { get; set; }
        private Socket clientSocket;
        public int clientNumber;
        NetworkStream stream;
        public StreamWriter writer;
        StreamReader reader;
        AuctionhouseService ahService;
        Screen screen;
        public int ChosenProductId;
        string clientText  = "";

        public ClientHandler(Socket clientSocket, int clientNumber , AuctionhouseService ahService , Screen screen )
        {
            this.clientSocket = clientSocket;
            this.clientNumber = clientNumber;
            this.ahService = ahService;
            this.screen = screen;
            this.MenuLocation = 0;
        }

        public string getIp()
        {
            string remoteIP = ((IPEndPoint)this.clientSocket.RemoteEndPoint).Address.ToString();
            return remoteIP;
        }

        internal void Start()
        {
            // Initialize
            this.stream = new NetworkStream(clientSocket);
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            
            SendToClient("Welcome to EAL Auctionhouse!");
            // Handle client
            do
            {
                if (IsValidInput(clientText))
                {
                    int input = 0;
                    decimal decInput = 0;
                    decimal bid = 0;
                    int arrayLength = clientText.Split(' ').Length;
                    if (int.TryParse(clientText, out input)) { }
                      
                    bool parseDec = decimal.TryParse(clientText, out decInput);
                    if ( MenuLocation == 2 && parseDec )
                    {
                        bid = decInput;
                    }
                    
                    if (MenuLocation == 1 && input != 0)
                    {
                        ShowProductMenu(input);
                        int productIndex = input - 1;
                        ChosenProductId = ahService.GetProductByIndex(productIndex).Id;
                        MenuLocation = 2;
                    }
                    else if (MenuLocation == 2 && bid != 0)
                    {
                        ShowBidMenu(ChosenProductId, bid);
                    }
                    else
                        MenuLocation = 0;
                }
                else if(MenuLocation == 0)
                {
                    ShowMainMenu();
                }
                else
                {
                    SendToClient("Invalid input try again");
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

        private void ShowProductMenu(int chosenProduct)
        {
            int productIndex = chosenProduct - 1;
            Product product = ahService.GetProductByIndex(productIndex);

            SendToClient(product.GetProduct());
            SendToClient("Please place your bid:");
            screen.PrintLine("Info for Product Id. " + product.Id + " sent to Client " + clientNumber);
        }

        private void ShowBidMenu(int chosenProduct, decimal bid)
        {
            Product product = ahService.GetProductById(ChosenProductId);
            
            if (product.IsValidBid(bid))
            {
                product.PlaceBid(bid, clientNumber, getIp() );
                Gavel gavel = new Gavel(product, ahService);
                ahService.StartGavel(gavel);
                
                screen.PrintLine("A bid of " + bid + " kr. has been placed on product Id." + product.Id);
                ahService.BroadcastToAllClientsInLocation("Current Bid on product Id. " + product.Id + " is " + product.GetCurrentBid() + " kr", product.Id);
                SendToClient("Your bid of " + bid + " kr. has been placed on product Id." + product.Id);
                ShowBidStatusMenu();
            }
            else
            {
                SendToClient("Bid is too low. The product's current bid is " + product.GetCurrentBid() + " kr.");
            }
        }
        
        private void ShowBidStatusMenu()
        {
            Product product = ahService.GetProductById(ChosenProductId);
            SendToClient(product.GetProduct());
            SendToClient("Your current bid is winning, we will tell you when its not");
        }

        private void ShowMainMenu()
        {
            string products = ahService.GetProductsMenu();
            SendToClient(products);
            SendToClient("Which product would you like to bid on? ");
            MenuLocation = 1;
        }

        private bool IsValidInput(string clientText)
        {
            bool isValid = false;
            int number;
            decimal decNumber = 0;

            if (int.TryParse(clientText, out number) || decimal.TryParse(clientText, out decNumber))
                return true;
            
            return isValid;
        }
        
        private void SendToClient(string input)
        {
            writer.WriteLine(input);
            writer.Flush();
        }
    }
}