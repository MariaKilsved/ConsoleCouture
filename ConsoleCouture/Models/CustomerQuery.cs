using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture.Models
{
    class CustomerQuery
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int? SaltId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
