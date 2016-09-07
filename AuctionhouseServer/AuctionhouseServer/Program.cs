using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AuctionhouseServer server = new AuctionhouseServer(9001);
            server.Run();
        }
    }
}
