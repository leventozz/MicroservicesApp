using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Responses
{
    public class OrderResponse
    {
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int PaymentMethod { get; set; }
    }
}
