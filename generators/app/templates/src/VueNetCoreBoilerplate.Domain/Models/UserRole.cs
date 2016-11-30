using System;

namespace <%= name %>.Domain.Models {
    public class UserRole {
        public User User { get; set; }
        public Guid UserId { get; set; }

        public Role Role { get; set; }
        public Guid RoleId { get; set; }
    }
}