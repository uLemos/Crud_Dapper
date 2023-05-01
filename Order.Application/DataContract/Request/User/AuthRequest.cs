using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.DataContract.Request.User
{
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
