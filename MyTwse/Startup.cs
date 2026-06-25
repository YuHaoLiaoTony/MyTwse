using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyTwse.Infrastructure;
using MyTwse.IRepositories;
using MyTwse.Models;
using MyTwse.Repositories;
using MyTwse.ServiceInterface;
using MyTwse.Services;

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

            //資料庫提供者 (appsettings.json 中的 DatabaseProvider: "Sqlite" 或 "SqlServer")
            var provider = Configuration.GetValue<string>("DatabaseProvider");

            if (provider == "Sqlite")
            {
                services.AddDbContext<TwseStockContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("SQLite")));
                services.AddScoped<IStockInfoRepository, SqliteStockInfoRepository>();
            }
            else
            {
                services.AddDbContext<TwseStockContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DBConStr")));
                services.AddScoped<IStockInfoRepository, StockInfoRepository>();
            }

            //Repository
            services.AddScoped<IInsertDateLogRepository, InsertDateLogRepository>();

           

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

            //SQLite 自動建立資料庫與表格
            if (Configuration.GetValue<string>("DatabaseProvider") == "Sqlite")
            {
                using var scope = app.ApplicationServices.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TwseStockContext>();
                db.Database.EnsureCreated();
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
