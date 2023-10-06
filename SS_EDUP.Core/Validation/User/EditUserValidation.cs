using FluentValidation;
using SS_EDUP.Core.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Validation.User
{
    public class EditUserValidation : AbstractValidator<EditUserVM>
    {
        public EditUserValidation()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Surname).NotEmpty();
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.PhoneNumber).NotEmpty();
        }
    }
}
