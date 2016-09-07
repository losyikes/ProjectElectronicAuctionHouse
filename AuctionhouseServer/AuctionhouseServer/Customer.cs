using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionhouseServer
{
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public int AccountNumber { get; set; }

        public Customer(int id, string name, string address, int postalCode, string city, int accountNumber)
        {
            Id = id;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            AccountNumber = accountNumber;
        }
    }
}
