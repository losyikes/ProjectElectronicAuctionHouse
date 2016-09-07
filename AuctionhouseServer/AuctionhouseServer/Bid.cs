using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class Bid
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int ClientID { get; set; }

        public Bid(decimal amount, DateTime date, int productID, int clientID)
        {
            Amount = amount;
            Date = date;
            ProductId = productID;
            ClientID = clientID;
        }
    }
}
