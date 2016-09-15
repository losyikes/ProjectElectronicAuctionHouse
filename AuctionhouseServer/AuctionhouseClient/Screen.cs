using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseClient
{
    class Screen : IScreen
    {
        public void Print(string input)
        {
            Console.WriteLine(input);
        }
        public void PrintLine(string input)
        {
            Console.WriteLine(input);
        }
    }
}
