using FluentValidation;
using Order.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Validations
{
    public class OrderValidation : AbstractValidator<OrderModel>
    {
        public OrderValidation()
        {
            RuleFor(x => x.Client)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.User)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Items)
                .NotNull()
                .NotEmpty();
        }
    }
}
