using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LearnNetCoreApi.Entities;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace LearnNetCoreApi
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
            services.AddMvc(setupAction =>
            {
                // Additional setup for content types
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            // Register DbContext
            var connectionString = Configuration["ConnectionStrings:Organization"];
            services.AddDbContext<OrganizationContext>(x => x.UseSqlServer(connectionString));

            // Register repository
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            // Register URL Helper (used for pagination metadata)
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error occurred. Try again later.");
                    });
                });
            }

            // Configure AutoMapper
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<OrgList, OrgListDto>();
                cfg.CreateMap<OrgListForCreationDto, OrgList>();
                cfg.CreateMap<OrgListForUpdateDto, OrgList>();
                cfg.CreateMap<OrgList, OrgListForUpdateDto>();

                cfg.CreateMap<OrgListItem, OrgListItemDto>();
                cfg.CreateMap<OrgListItemForCreationDto, OrgListItem>();
                cfg.CreateMap<OrgListItemForUpdateDto, OrgListItem>();
                cfg.CreateMap<OrgListItem, OrgListItemForUpdateDto>();
            });

            app.UseMvc();
        }
    }
}
