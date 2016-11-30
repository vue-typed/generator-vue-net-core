using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VueNetCoreBoilerplate.Service.Users;
using VueNetCoreBoilerplate.Service.Users.Dto;
using VueNetCoreBoilerplate.Service.Users.Stores;

namespace VueNetCoreBoilerplate.Service.Infrastructure {
    public static class Module {
        public static void Configure(IServiceCollection services) {
            Repository.Infrastructure.Module.Configure(services);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<RoleManager<ApplicationRole>>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddClaimsPrincipalFactory<UserPrincipalFactory>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserStore>();
        }

        public static void ConfigureMapper() {
            MapperConfig.Init();
        }
    }
}