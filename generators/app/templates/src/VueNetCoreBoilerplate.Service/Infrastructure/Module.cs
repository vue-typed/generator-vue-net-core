using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using <%= name %>.Service.Users;
using <%= name %>.Service.Users.Dto;
using <%= name %>.Service.Users.Stores;

namespace <%= name %>.Service.Infrastructure {
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