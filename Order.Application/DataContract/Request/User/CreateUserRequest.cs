using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.DataContract.Request.User
{
    public sealed class CreateUserRequest
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
