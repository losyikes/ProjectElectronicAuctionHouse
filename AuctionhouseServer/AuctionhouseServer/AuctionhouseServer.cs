﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
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
        public Screen screen = new Screen();
        public List<ClientHandler> ClientHandlers = new List<ClientHandler>();
        AuctionhouseService ahService;

        public AuctionhouseServer(int port)
        {
            this.port = port;
            ahService = new AuctionhouseService(this);
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
                screen.PrintLine("Server is ready for a new client to connect.");

                Socket clientSocket = listener.AcceptSocket();
                clientNumber++;
                screen.PrintLine("Client" + clientNumber + ", connected.");
                
                ClientHandler clientHandler = new ClientHandler(clientSocket, clientNumber, ahService, screen);
                Thread ClientHandlerThread = new Thread(clientHandler.Start);
                ClientHandlerThread.Start();
                ClientHandlers.Add(clientHandler);
            }

            // End
            screen.PrintLine("Shutting down connection...");
            
            listener.Stop();
            Thread.Sleep(3000);
        }
    }
}
