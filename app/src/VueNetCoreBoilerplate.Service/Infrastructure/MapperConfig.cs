using AutoMapper;
using VueNetCoreBoilerplate.Domain.Models;
using VueNetCoreBoilerplate.Service.Users.Dto;

namespace VueNetCoreBoilerplate.Service.Infrastructure {
    internal static class MapperConfig {
        public static void Init() {
            Mapper.Initialize(Configure);
        }

        private static void Configure(IMapperConfigurationExpression cfg) {

            cfg.CreateMap<User, ApplicationUser>();
            cfg.CreateMap<ApplicationUser, User>();

            cfg.CreateMap<Role, ApplicationRole>();
            cfg.CreateMap<ApplicationRole, Role>();

            cfg.CreateMap<UserProfile, UserProfileDto>();
            cfg.CreateMap<UserProfileDto, UserProfile>();

        }
    }
}