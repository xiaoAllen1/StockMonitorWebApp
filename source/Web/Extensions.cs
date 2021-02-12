using Architecture.Application;
using Architecture.Database;
using DotNetCore.AspNetCore;
using DotNetCore.EntityFrameworkCore;
using DotNetCore.IoC;
using DotNetCore.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Architecture.Web
{
    public static class Extensions
    {
        public static bool IsAPIOnly
        {
            get
            {
                if (Environment.GetEnvironmentVariable("IsAPIOnly") != null)
                {
                    return bool.Parse(Environment.GetEnvironmentVariable("IsAPIOnly"));
                }
                return false;
            }
        }

        public static void AddContext(this IServiceCollection services)
        {
            var connectionString = "Data Source=tcp:stockmonitorapp-server.database.windows.net,1433;Initial Catalog=stockmonitorapp-database;User Id=stockmonitorapp-server-admin@stockmonitorapp-server.database.windows.net;Password=Q057S1MV70IX415R$;";
            services.AddContext<Context>(options => options.UseSqlServer(connectionString));
            services.AddUnitOfWork<Context>();
        }

        public static void AddSecurity(this IServiceCollection services)
        {
            services.AddHash();
            services.AddJsonWebToken(Guid.NewGuid().ToString(), TimeSpan.FromHours(12));
            services.AddAuthenticationJwtBearer();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddFileExtensionContentTypeProvider();
            services.AddClassesInterfaces(typeof(IUserService).Assembly);
            services.AddClassesInterfaces(typeof(IUserRepository).Assembly);
        }

        public static void AddSpa(this IServiceCollection services)
        {
            if (IsAPIOnly) return;
            services.AddSpaStaticFiles("Frontend/dist");
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            if (IsAPIOnly)
            {
                services.AddSwaggerGen(ConfigureOpenApi);
            }
        }

        public static void UseSpa(this IApplicationBuilder application)
        {
            if (IsAPIOnly) return;
            application.UseSpaAngular("Frontend", "start", "http://localhost:4200");
        }

        public static void AddSwagger(this IApplicationBuilder application)
        {
            if (IsAPIOnly)
            {
                application.UseSwagger();
                application.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
        }

        private static void ConfigureOpenApi(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Stock Monitor Service",
                Description = "Stock Monitor Service API"
            });
        }
    }
}
