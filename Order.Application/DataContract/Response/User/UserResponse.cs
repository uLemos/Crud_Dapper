using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.DataContract.Response.User
{
    public sealed class UserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
    }
}
