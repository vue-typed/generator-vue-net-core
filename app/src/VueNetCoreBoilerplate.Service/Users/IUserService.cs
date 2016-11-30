﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VueNetCoreBoilerplate.Service.Users.Dto;

namespace VueNetCoreBoilerplate.Service.Users {
    public interface IUserService {        
        Task AddRole(AddRoleDto data);
        Task<ClaimsPrincipal> SignUp(SignUpDto data, string[] roles = null);
        Task<ClaimsPrincipal> SignIn(SignInDto data);
        Task<UserProfileDto> GetUserProfile(Guid userId);
        Task UpdateUserProfile(Guid userId, UserProfileDto profile);
        Task<ClaimsPrincipal> RefreshUser(string userName);
    }
}