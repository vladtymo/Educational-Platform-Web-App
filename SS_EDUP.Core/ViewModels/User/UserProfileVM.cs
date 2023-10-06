using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.ViewModels.User
{
    public class UserProfileVM
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;    
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
