using Ordering.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Core.Entities
{
    public class Order : Entity
    {
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int PaymentMethod { get; set; }
    }
}
