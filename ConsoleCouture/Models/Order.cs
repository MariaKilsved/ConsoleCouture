using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleCouture.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public string ReceiverName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
