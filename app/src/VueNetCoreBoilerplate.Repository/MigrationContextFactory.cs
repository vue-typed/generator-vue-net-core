using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VueNetCoreBoilerplate.Global.Options;

namespace VueNetCoreBoilerplate.Repository {
    public class MigrationContextFactory : IDbContextFactory<Context> {
        public Context Create(DbContextFactoryOptions options) {
            
            var opt = new OptionsManager<DbOptions>(new[] {
                
                new ConfigureOptions<DbOptions>(dbOptions => {

                    var isDev = options.EnvironmentName == "Development";
                    var appsettings = File.ReadAllText($"../../../../VueNetCoreBoilerplate/appsettings.{ (isDev ? "dev" : "prod") }.json");
                    dynamic set = JsonConvert.DeserializeObject(appsettings);

                    Console.WriteLine();
                    Console.WriteLine($"Running migration in {options.EnvironmentName} environment...");

                    dbOptions.ConnectionString = set.ConnectionStrings.VueNetCoreBoilerplate;
                })
            });

            return new Context(opt);
        }

        
    }
}