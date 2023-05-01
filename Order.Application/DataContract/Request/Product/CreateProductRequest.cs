using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.DataContract.Request.Product
{
    public sealed class CreateProductRequest
    {
        public string Description { get; set; }
        public decimal SellValue { get; set; }
        public string Stock { get; set; }

    }
}
