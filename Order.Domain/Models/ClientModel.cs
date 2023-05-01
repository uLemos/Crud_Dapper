using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Models
{
    public class ClientModel : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
    }
}
