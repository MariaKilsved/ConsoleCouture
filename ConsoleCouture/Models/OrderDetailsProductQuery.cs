using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture.Models
{
    class OrderDetailsProductQuery
    {
        public string ProductName { get; set; }
        public long? SumQuantity { get; set; }
        public long? SumPrice { get; set; }

    }
}
