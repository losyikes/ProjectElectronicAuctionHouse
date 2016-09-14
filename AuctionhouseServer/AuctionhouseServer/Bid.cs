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
        public Product Product { get; set; }
        public int ClientID { get; set; }

        public Bid(decimal amount, DateTime date, Product product, int clientID)
        {
            Amount = amount;
            Date = date;
            Product = product;
            ClientID = clientID;
            if(this.Amount > product.CurrentBid.Amount){
                Product.CurrentBid = this;
            }
        }
    }
}
