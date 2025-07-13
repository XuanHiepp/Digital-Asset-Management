using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MusicServer.Models.Database;
using Swashbuckle.AspNetCore.Swagger;
using MusicServer.Services.Interfaces;
using MusicServer.Services.Enforcements;
using MusicServer.Repositories.Interfaces;
using MusicServer.Repositories.Enforcements;
using System.IO;
using Hangfire;

namespace MusicServer
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var ethereumSettings = Configuration.GetSection("EthereumSettings");
            services.Configure<EthereumSettings>(ethereumSettings);

            services.AddTransient<IEthereumService, EthereumService>();
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IMusicRepository, MusicRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMusicAssetTransferRepository, MusicAssetTransferRepository>(); 

            services.AddTransient<IUploadMusicService, UploadMusicService>();
            services.AddTransient<IMusicAssetRepository, MusicAssetRepository>();
            services.AddTransient<IMusicAssetTransferService, MusicAssetTransferService>();
            services.AddTransient<IPKIEncordeService, PKIEncordeService>();
            services.AddTransient<IUploadEncodeAndStreamFiles, UploadEncodeAndStreamFiles>();

            services.AddDbContext<MusicDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // register the swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Music API",
                    Version = "v1"
                });

                c.OperationFilter<SwaggerFileOperationFilter>();

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = "apiKey"
                    }
                );
                c.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {
                            "Bearer", Enumerable.Empty<string>()
                        },
                    }
                );
            });

            services.AddCors();

            // Hangfire automatically creates necessary tables in the database. No need to run extra migrations for the service.
            services.AddHangfire(
                x => x.UseSqlServerStorage(Configuration.GetConnectionString("WebApiDatabase"))
            );
            services.AddHangfireServer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable Swagger middleware 
            app.UseSwagger();

            // specify the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
