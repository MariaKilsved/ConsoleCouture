using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleCouture.Models
{
    public partial class Stock
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Size { get; set; }
        public int? InStock { get; set; }

        public virtual Product Product { get; set; }
    }
}
