using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AuctionhouseClient client = new AuctionhouseClient("127.0.0.1", 9001);
            client.Run();
        }
    }
}
