using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.ViewModels.User
{
    public class TwoFactorAuthenticationVM
    {
        public string Code { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
