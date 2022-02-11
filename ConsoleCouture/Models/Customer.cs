using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleCouture.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int? SaltId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }

        public virtual Salt Salt { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
