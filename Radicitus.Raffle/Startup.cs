using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radicitus.Data.Contexts.Raffles;
using Radicitus.Data.Contexts.Raffles.Implementations;
using Radicitus.Raffle.Hubs;
using Radicitus.Redis;

namespace Radicitus.Raffle
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            var redisConnection = "localhost";
            services.AddSingleton<IRedisRaffleRepository>(new RadRaffleRedisRepository(redisConnection));
            services.AddSignalR(options =>
                {
                    options.KeepAliveInterval = TimeSpan.FromSeconds(5);
                })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                }).AddRedis(redisConnection);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            var connectionString = Configuration.GetConnectionString("radicitus-5050");

            services.AddDbContext<RadicitusDbContext>(optionsAction =>
            {
                optionsAction.UseNpgsql(connectionString, builder =>
                {
                    builder.MigrationsAssembly("Radicitus.Raffle");
                });
            });
            services.AddScoped<RaffleRepository>();
            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = "wwwroot/radicitus-raffle/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<RaffleHub>("/rafflehub");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot/radicitus-raffle";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080/");
                }
            });
        }
    }
}
