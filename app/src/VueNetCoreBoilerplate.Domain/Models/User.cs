using System;
using System.Collections.Generic;

namespace VueNetCoreBoilerplate.Domain.Models {
    public class User {
        public User() {
            Profile = new UserProfile();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> UserRoles{ get; set; }
        public UserProfile Profile { get; set; }        
    }
}