using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleCouture.Models
{
    public partial class DeliveryOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
}
