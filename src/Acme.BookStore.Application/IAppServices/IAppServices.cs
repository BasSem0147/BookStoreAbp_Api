using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Validation;

namespace Acme.BookStore.IAppServices
{
    public abstract class IAppServices : ApplicationService
    {
        #region helper methods
        internal AbpValidationException GetValidationException
                    (FluentValidation.Results.ValidationResult validationResult)
        {

            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            var errors =
                validationResult
                .Errors
                .Select(x => new System.ComponentModel.DataAnnotations.ValidationResult
                                (x.ErrorMessage, [x.PropertyName]))
                .ToList();

            return new AbpValidationException(message, errors);
        }
        #endregion helper methods
    }
}
