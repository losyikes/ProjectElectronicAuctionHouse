using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseClient
{
    class AuctionhouseClient
    {
        private string serverName;
        private int port;

        public AuctionhouseClient(string serverName, int port)
        {
            this.serverName = serverName;
            this.port = port;
        }

        internal void Run()
        {
            // Initialize
            TcpClient server = new TcpClient(serverName, port);
            NetworkStream stream = server.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            StreamReader reader = new StreamReader(stream);
            string serverText;
            string input;
            Console.WriteLine("Welcome to EAL Auctionhouse!");

            // Handle userinput
            do
            {

            } while (input.ToLower() != "exit");

            // End
            reader.Close();
            writer.Close();
            stream.Close();
            server.Close();
        }
    }
}
