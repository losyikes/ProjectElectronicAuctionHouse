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
        public int AuctionStatus { get; set; } // 0: pending, 1: started, 2: ended
        public decimal MinimumBid { get; set; }
        public Bid CurrentBid { get; set; }
        public Bid LastBid { get; set; }
        internal string GetProduct()
        {
            decimal bid = 0;
            if (CurrentBid != null)
            {
                if (CurrentBid.Amount < MinimumBid)
                    bid = MinimumBid;
                else
                    bid = CurrentBid.Amount;
            }
            else
                bid = MinimumBid;
            
            return "Productname: " + Name + ". " +
                "Valuation: " + Valuation + " kr. " +
                "Current Bid: " + bid + "kr.\n";
        }
        public bool IsValidBid(decimal bid)
        {
            bool isValid = false;
            if(CurrentBid != null)
            {
                if (bid > CurrentBid.Amount)
                    isValid = true;
            }
            else
            {
                if (bid > MinimumBid)
                    isValid = true;
            }
            return isValid;
                
        }
        public void PlaceBid(decimal bid, int clientId)
        {
            if(AuctionStatus == 0)
            {
                AuctionStatus = 1;
            }
            Bid b = new Bid(bid, DateTime.Now, clientId);
            LastBid = CurrentBid;
            CurrentBid = b;

        }
        public decimal GetCurrentBid()
        {
            decimal currentBid = MinimumBid;
            if (CurrentBid != null)
                currentBid = CurrentBid.Amount;

            return currentBid;
        }
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
