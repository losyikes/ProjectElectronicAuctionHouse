using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SubmitDate { get; set; }
        public decimal Valuation { get; set; }
        public string ProductInfo { get; set; }
        public int AuctionStatus { get; set; } // 1: can't bid, 2: can bid, 3: sold
        public decimal MinimumBid { get; set; }

        public Product(int id, string name, DateTime submitDate, decimal valuation, string productInfo, int auctionStatus, decimal mininumBid)
        {
            Id = id;
            Name = name;
            SubmitDate = submitDate;
            Valuation = valuation;
            ProductInfo = productInfo;
            AuctionStatus = auctionStatus;
            MinimumBid = mininumBid;
        }
    }
}
