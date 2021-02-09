using System;
using System.Collections.Generic;
using System.IO;
using ChrisfrewDotInApi.Infrastructure;
using ChrisfrewDotInApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ChrisfrewDotInApi
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8000", "http://localhost:3000", "http://localhost:5000"
                                        );
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://chrisfrew.in", "https://chrisfrewin.com"
                                        );
                    });
                });
            }
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChrisfrewDotInApi", Version = "v1" });
            });

            var env = GetEnv();

            services.AddScoped<ILinkPreviewService, LinkPreviewService>(x =>
            {
                return new LinkPreviewService("http://api.linkpreview.net/", env["LINK_PREVIEW_API_KEY"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChrisfrewDotInApi v1"));
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Dictionary<string, string> GetEnv()
        {
            var physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            using (var reader = new StreamReader(Path.Combine(physicalProvider.Root, ".env.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (Dictionary<string, string>)serializer.Deserialize(reader, typeof(Dictionary<string, string>));
            }
        }
    }
}
