using Acme.BookStore.IServices.Author;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.AppServices.Author
{
    internal class FluentAuthorValidator: AbstractValidator<Create_Update_Author>
    {
        public FluentAuthorValidator()
        {
            RuleFor(p=>p.Name).MinimumLength(8)
                .WithErrorCode("Not Valid Name 100")
                .WithMessage("Not Valid");
        }
    }
}
