using System;
using System.Collections.Generic;

namespace <%= name %>.Domain.Models {
    public class Role {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}