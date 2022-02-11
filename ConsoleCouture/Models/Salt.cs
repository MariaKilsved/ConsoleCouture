using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleCouture.Models
{
    public partial class Salt
    {
        public Salt()
        {
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string Salt1 { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
