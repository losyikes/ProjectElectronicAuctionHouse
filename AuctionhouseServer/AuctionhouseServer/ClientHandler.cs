﻿using System;
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
            string clientText;

            // Handle client
            do
            {
                clientText = reader.ReadLine();
                Console.WriteLine("Client {0} says: {1}", clientNumber, clientText);

                // Do stuff with clientText here

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