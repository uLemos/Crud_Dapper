using FluentValidation;
using Order.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Validations
{
    public class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation() 
        {
            RuleFor(x => x.Name)
                        .NotNull()
                        .NotEmpty()
                        .Length(3, 20);

            RuleFor(x => x.Login)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.PasswordHash)
                .NotNull()
                .NotEmpty();
        }
    }
}
