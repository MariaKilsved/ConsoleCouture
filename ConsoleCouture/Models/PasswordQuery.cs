using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture.Models
{
    class PasswordQuery
    {
        public int SaltId { get; set; }
        public string Salt { get; set; }
        public int CustomerId { get; set; }
        public string Password { get; set; }

    }
}
