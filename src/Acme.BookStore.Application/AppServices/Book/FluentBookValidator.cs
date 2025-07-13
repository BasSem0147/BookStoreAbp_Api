using Acme.BookStore.IServices.Book;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.AppServices.Book
{
    internal class FluentBookValidator : AbstractValidator<Create_Update_Book>
    {
        public FluentBookValidator()
        {
            RuleFor(p => p.Name).MinimumLength(8)
                .WithErrorCode("Not Valid Name 100")
                .WithMessage("Not Valid");
            RuleFor(p => p.Type).IsInEnum()
                .WithErrorCode("Not Valid Book Type 101")
                .WithMessage("Not Valid Book Type");
        }
    }
}
