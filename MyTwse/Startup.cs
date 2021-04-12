using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyTwse.Filters;
using MyTwse.IRepositories;
using MyTwse.Models;
using MyTwse.Repositories;
using MyTwse.ServiceInterface;
using MyTwse.Services;
using System.Net.Mime;

namespace MyTwse
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
            services.AddMvc(config =>
            {
                config.Filters.Add(new ExceptionFilter());
                config.Filters.Add(new ActionFilter());
            });

            //Service
            services.AddScoped<IStockInfoService, StockInfoService>();
            //服務注入DbContext
            services.AddDbContext<TwseStockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConStr")));

            //Repository
            services.AddScoped<IInsertDateLogRepository, InsertDateLogRepository>();
            services.AddScoped<IStockInfoRepository, StockInfoRepository>();

           

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);

                    result.ContentTypes.Add(MediaTypeNames.Application.Json);

                    return result;
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
