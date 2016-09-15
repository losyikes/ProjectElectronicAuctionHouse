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
        Screen screen = new Screen();

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
            // Do stuff
            Thread ContinuouslyReadThread = new Thread(ContinuouslyRead);
            ContinuouslyReadThread.IsBackground = true;
            ContinuouslyReadThread.Start();
            
            do
            {
                    input = Console.ReadLine(); //receives bid
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
                screen.PrintLine(serverText);
            }
        }
    }
}
