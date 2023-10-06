using FluentValidation;
using SS_EDUP.Core.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Validation.User
{
    public class UpdateProfileValidation : AbstractValidator<UpdateProfileVM>
    {
        public UpdateProfileValidation()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Surname).NotEmpty();
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.PhoneNumber).NotEmpty();
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
            RuleFor(r => r.OldPassword).NotEmpty().MinimumLength(6);
            RuleFor(r => r.ConfirmPassword).MinimumLength(6).Equal(r => r.Password);
        }
    }
}
