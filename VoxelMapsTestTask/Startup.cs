using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Threading;
using VoxelMapsTestTask.Hub;
using VoxelMapsTestTask.Service;

namespace VoxelMapsTestTask
{
    public class Startup
    {
        private const string CorsPolicyName = "CORS_POLICY";



        public Startup() { }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();

            services.AddSignalR();
            services.AddHttpContextAccessor();
            services.AddScoped<IFileService, FileService>();

            ConfigureSwaggerUi(services);

            services.AddCors(opt =>
            {
                opt.AddPolicy(CorsPolicyName,
                    builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ProgressHub>("/hub/progress");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoxelMapsTestTaskApi v1");
                c.RoutePrefix = "";
            });
        }
        private void ConfigureSwaggerUi(IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VoxelMapsTestTaskApi",
                    Version = "v1"
                });
            });
        }
    }
}
