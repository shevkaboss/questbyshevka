using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestByShevka.WebApi.ActionFilters;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using QuestByShevka.Services.Models;
using QuestByShevka.Services;
using QuestByShevka.Shared.Utils;

namespace QuestByShevka.WebApi
{
    public class Startup
    {
        private static readonly string GAME_INFO_FILE_NAME = "GameInfo.json";
        private static readonly string CONFIG_FOLDER = "Configuration";
        public Startup(IConfiguration configuration)
        { 
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

            var config = new ConfigurationBuilder()
                                    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, CONFIG_FOLDER, GAME_INFO_FILE_NAME)).Build();
            services.Configure<Quest>(config);

            services.AddSingleton<QuestHandler>();
            services.AddSingleton<QuestRunnerService>();

            services.AddHttpClient<SelfTrigger>();

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quest By Shevka API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Quest By Shevka API");
            });

            var selfTrigger = app.ApplicationServices.GetRequiredService<SelfTrigger>();
            selfTrigger.StartHerokuSelfTrigger();

            app.UseRouting();
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
