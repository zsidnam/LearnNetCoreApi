using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LearnNetCoreApi.Entities;
using LearnNetCoreApi.Services;
using LearnNetCoreApi.Models;

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
            services.AddMvc();

            // Register DbContext; get connection string from appSettings file
            var connectionString = Configuration["ConnectionStrings:Organization"];
            services.AddDbContext<OrganizationContext>(x => x.UseSqlServer(connectionString));

            // Register repository
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure AutoMapper
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<OrgList, OrgListDto>();
                cfg.CreateMap<OrgListItem, OrgListItemDto>();
            });

            app.UseMvc();
        }
    }
}
