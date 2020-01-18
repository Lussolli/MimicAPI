using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.V1.Repositories;
using MimicAPI.V1.Repositories.Contracts;
using AutoMapper;
using MimicAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace MimicAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region AutoMapper-Config
            var configuracao = new MapperConfiguration(config => {
                config.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = configuracao.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddDbContext<Contexto>(options => 
                options.UseSqlite("Data Source=Database/MimicAPI.db"));

            services.AddControllers();
            services.AddScoped<IPalavraRepository, PalavraRepository>();
            services.AddApiVersioning(config => {
                config.ReportApiVersions = true;
                // config.ApiVersionReader = new HeaderApiVersionReader("api-version");
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
