using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionhouseClient
{
    class AuctionhouseClient
    {
        private string serverName;
        private int port;
        TcpClient server;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;

        public AuctionhouseClient(string serverName, int port)
        {
            this.serverName = serverName;
            this.port = port;
        }

        internal void Run()
        {
            // Initialize
            server = new TcpClient(serverName, port);
            stream = server.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            string input;
            Console.WriteLine("Welcome to EAL Auctionhouse!");

            // Do stuff
            Thread ContinuouslyReadThread = new Thread(ContinuouslyRead);
            ContinuouslyReadThread.IsBackground = true;
            ContinuouslyReadThread.Start();

            do
            {
                writer.WriteLine("query product list");
                writer.Flush();
                Thread.Sleep(300);

                Console.Write("Which product would you like to bid on? ");
                input = Console.ReadLine();
                writer.WriteLine(input);
                writer.Flush();


            } while (input.ToLower() != "exit");

            // End
            reader.Close();
            writer.Close();
            stream.Close();
            server.Close();
        }

        private void ContinuouslyRead()
        {
            string serverText;
            while (true)
            {
                serverText = reader.ReadLine();
                Console.WriteLine(serverText);
            }
        }
    }
}
