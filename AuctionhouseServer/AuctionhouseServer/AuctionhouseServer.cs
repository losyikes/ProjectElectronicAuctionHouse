using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class AuctionhouseServer
    {
        private IPAddress IP = IPAddress.Parse("127.0.0.1");
        private int port;
        private volatile bool stop;
        //Screen screen = new Screen();

        public AuctionhouseServer(int port)
        {
            this.port = port;
        }

        internal void Run()
        {
            // Initialize
            TcpListener listener = new TcpListener(IP, port);
            listener.Start();
            int clientNumber = 0;
            stop = false;

            // Handle new clients
            while (!stop)
            {
                //screen.PrintLine("Server is ready for a new client to connect.");
                Console.WriteLine("Server is ready for a new client to connect.");

                Socket clientSocket = listener.AcceptSocket();
                clientNumber++;
                //screen.PrintLine("Client" + clientNumber + ", connected.");
                Console.WriteLine("Client {0}, connected.", clientNumber);

                Thread ClientHandlerThread = new Thread(new ClientHandler(clientSocket, clientNumber).Start);
                ClientHandlerThread.Start();
            }

            // End
            //screen.PrintLine("Shutting down connection...");
            Console.WriteLine("Shutting down connection...");
            listener.Stop();
            Thread.Sleep(3000);
        }
    }
}
