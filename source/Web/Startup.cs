using Architecture.Web;
using DotNetCore.AspNetCore;
using DotNetCore.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

Host.CreateDefaultBuilder().UseSerilog().Run<Startup>();

namespace Architecture.Web
{
    public sealed class Startup
    {
        public void Configure(IApplicationBuilder application)
        {
            application.UseException();
            application.UseHttps();
            application.UseRouting();
            application.UseEndpoints();
            application.UseResponseCompression();
            application.UseAuthentication();
            application.UseAuthorization();
            application.UseSpa();
            application.AddSwagger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSecurity();
            services.AddResponseCompression();
            services.AddControllersMvcJsonOptions();
            services.AddSpa();
            services.AddContext();
            services.AddServices();
            services.AddSwagger();
        }
        
    }
}
