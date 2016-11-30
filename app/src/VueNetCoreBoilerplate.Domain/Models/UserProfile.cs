using System;
using VueNetCoreBoilerplate.Global.Enums;

namespace VueNetCoreBoilerplate.Domain.Models {
    public class UserProfile {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public GenderEnum Gender { get; set; }
        public string Email { get; set; }
    }

    
}