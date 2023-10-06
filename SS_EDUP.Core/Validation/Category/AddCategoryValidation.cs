using FluentValidation;
using SS_EDUP.Core.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Validation.Category
{
    public class AddCategoryValidation : AbstractValidator<CategoryDto>
    {
        public AddCategoryValidation()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}
