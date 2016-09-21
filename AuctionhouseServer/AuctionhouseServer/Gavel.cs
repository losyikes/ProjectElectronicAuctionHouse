using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AuctionhouseServer
{
    class Gavel
    {
        int seconds = 0;
        Product product;
        AuctionhouseService ahService;
        object gavelSecondsLock = new object();
        public int GavelStatus { get; set; } // 0=ended, 1= running

        public Gavel(Product product, AuctionhouseService ahService)
        {
            this.product = product;
            this.ahService = ahService;
            this.GavelStatus = 0;
        }

        internal void Start()
        {
            bool keepGoing = true;
            GavelStatus = 1;
            while (keepGoing)
            {
                Thread.Sleep(1000);
                seconds++;
                if (seconds == 10)
                {
                    ahService.BroadcastToAllClientsInLocation("First", product.Id);
                    ahService.server.screen.PrintLine("gavel 10");
                }
                else if (seconds == 15)
                {
                    ahService.BroadcastToAllClientsInLocation("Second", product.Id);
                    ahService.server.screen.PrintLine("gavel 15");
                }
                else if (seconds == 18)
                {
                    ahService.BroadcastToAllClientsInLocation("Third. Sold to Client id. " + product.CurrentBid.ClientID + "( " + product.CurrentBidIp + " ) for " + product.CurrentBid.Amount + " kr.", product.Id);
                    ahService.server.screen.PrintLine("gavel 18");
                    GavelStatus = 0;
                    product.AuctionStatus = 2;
                    keepGoing = false;
                }
            }
        }

        internal void ResetGavel()
        {
            lock (gavelSecondsLock)
            {
                seconds = 0;
            }
            
        }
    }
}
