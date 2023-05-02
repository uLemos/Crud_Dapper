using FluentValidation;
using Order.Domain.Models;

namespace Order.Domain.Validations
{
    public class ProductValidation : AbstractValidator<ProductModel>
    {
        public ProductValidation() 
        {
            RuleFor(x => x.Description)
                    .NotNull()
                    .NotEmpty();

            RuleFor(x => x.SellValue)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Stock)
                .NotNull()
                .NotEmpty();
        }
    }
}
