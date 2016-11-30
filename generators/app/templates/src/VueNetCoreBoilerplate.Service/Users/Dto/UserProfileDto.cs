using System;
using <%= name %>.Global.Enums;

namespace <%= name %>.Service.Users.Dto {
    public class UserProfileDto {
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public GenderEnum Gender { get; set; }
        public string Email { get; set; }
    }
}