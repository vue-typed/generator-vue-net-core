using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using <%= name %>.Global.Options;

namespace <%= name %>.Repository {
    public class MigrationContextFactory : IDbContextFactory<Context> {
        public Context Create(DbContextFactoryOptions options) {
            
            var opt = new OptionsManager<DbOptions>(new[] {
                
                new ConfigureOptions<DbOptions>(dbOptions => {

                    var isDev = options.EnvironmentName == "Development";
                    var appsettings = File.ReadAllText($"../../../../<%= name %>/appsettings.{ (isDev ? "dev" : "prod") }.json");
                    dynamic set = JsonConvert.DeserializeObject(appsettings);

                    Console.WriteLine();
                    Console.WriteLine($"Running migration in {options.EnvironmentName} environment...");

                    dbOptions.ConnectionString = set.ConnectionStrings.<%= name %>;
                })
            });

            return new Context(opt);
        }

        
    }
}