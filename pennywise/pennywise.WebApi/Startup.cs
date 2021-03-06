using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pennywise.Application;
using pennywise.Application.Interfaces;
using pennywise.Infrastructure.Identity;
using pennywise.Infrastructure.Persistence;
using pennywise.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using pennywise.Infrastructure.Identity.Contexts;
using pennywise.Infrastructure.Persistence.Contexts;
using pennywise.WebApi.Extensions;
using pennywise.WebApi.Services;

namespace pennywise.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext appcontext, IdentityContext identitycontext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHangfireExtension();
            RunMigration(appcontext).Wait();
            RunMigration(identitycontext).Wait();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
        
        private async Task RunMigration<T>(T db) where T : DbContext
        {
            List<string> pendingMigrations = db.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Any())
            {
                IMigrator migrator = db.Database.GetService<IMigrator>();
                foreach (string targetMigration in pendingMigrations)
                {
                    migrator.Migrate(targetMigration);
                }
            }
            await Task.CompletedTask;
        }
    }
}
