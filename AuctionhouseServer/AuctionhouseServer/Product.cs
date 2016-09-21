using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

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
        public string CurrentBidIp { get; set; }
        public Gavel Gavel { get; set; }
        object bidLock = new object();
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

        public void PlaceBid(decimal bid, int clientId, string clientIP, AuctionhouseService ahService)
        {
            if (AuctionStatus == 0)
            {
                AuctionStatus = 1;
                Gavel gavel = new Gavel(this, ahService);
                this.Gavel = gavel;
                Thread gavelThread = new Thread(gavel.Start);
                gavelThread.Start();
            }
            else
            {
                this.Gavel.ResetGavel();
            }
                
            Bid b = new Bid(bid, DateTime.Now, clientId);
            lock (bidLock)
            {
                CurrentBidIp = clientIP;
                LastBid = CurrentBid;
                CurrentBid = b;
            }
            
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

        internal void HardcodeCustomers()
        {
            Customer customer1 = new Customer(1, "Anders Larsen", "Møllergade 3", 5700, "Svendborg", 123456789);
            Customer customer2 = new Customer(1, "Jens Mogensen", "Møllergade 3", 5000, "Odense", 987654321);
        }
    }
}
