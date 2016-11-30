using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VueNetCoreBoilerplate.Domain.Models;
using VueNetCoreBoilerplate.Global.Options;

namespace VueNetCoreBoilerplate.Repository {
    public class Context : DbContext {
        private readonly DbOptions _options;

        public Context(IOptions<DbOptions> options) : this(options.Value) {
            
        }

        public Context(DbOptions options) {
            this._options = options;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbOptions Options {
            get { return _options; }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var cnnStr = !_options.ConnectionString.EndsWith(";")
                ? _options.ConnectionString + ";"
                : _options.ConnectionString;

            optionsBuilder.UseSqlServer(cnnStr + "MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder) {        
            builder.Entity<UserRole>()
                .HasKey(x => new {x.UserId, x.RoleId});
            builder.Entity<UserRole>()
                .HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            builder.Entity<UserRole>()
                .HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);

            builder.Entity<UserProfile>()
                .HasKey(x => x.UserId);
            builder.Entity<UserProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.Profile);
        }
    }
}