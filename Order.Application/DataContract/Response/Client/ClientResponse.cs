using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.DataContract.Response.Client
{
    public sealed class ClientResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
    }
}
