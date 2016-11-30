using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using <%= name %>.Global.Options;
using <%= name %>.JWT;
using <%= name %>.Service.Users.Dto;
using <%= name %>.Service.Users.Stores;

namespace <%= name %> {
    public class Startup {
        public IConfigurationRoot Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment()) {
                builder.AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true);
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            else {
                builder.AddJsonFile("appsettings.prod.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddOptions();

            services.Configure<JwtOptions>(opt => {
                opt.JWT_SECRET = Configuration["JWT_SECRET"];
                opt.JWT_ISSUER = Configuration["JWT_ISSUER"];
                opt.JWT_AUDIENCE = Configuration["JWT_AUDIENCE"];
                opt.JWT_EXPIRATION = TimeSpan.FromMinutes(int.Parse(Configuration["JWT_EXPIRATION"]));
            });

            services.Configure<DbOptions>(
                opt => { opt.ConnectionString = Configuration.GetConnectionString("<%= name %>"); });

            services.Configure<AppOptions>(opt => { opt.Title = Configuration["TITLE"]; });

            services.AddTransient<TokenProvider>();

            Service.Infrastructure.Module.Configure(services);

            Service.Infrastructure.Module.ConfigureMapper();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                });
            }

            app.UseStaticFiles();
            app.UseIdentity();

            var tokenProvider = app.ApplicationServices.GetService<TokenProvider>();
            app.UseJwtBearerAuthentication(tokenProvider.BuildBearerOptions());

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new {controller = "Home", action = "Index"});
            });
        }
    }
}