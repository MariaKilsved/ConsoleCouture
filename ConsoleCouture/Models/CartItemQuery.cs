using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture.Models
{
    class CartItemQuery
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int StockId { get; set; }
        public string Size { get; set; }
        public int? InStock { get; set; }
    }
}
