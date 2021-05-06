using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Web_Service.Models;

namespace Web_Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ParkingmachinesdbMainContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/html;charset=utf-8";

                    System.DateTime current_date = System.DateTime.Now;

                    if (current_date.DayOfWeek == System.DayOfWeek.Monday && current_date.Hour == 10)
                    {
                        context.Response.Redirect("http://localhost:7285/api/Data/Update");
                    }
                    else
                    {
                        await context.Response.WriteAsync(@"<script>
                                                            onload = function () {setTimeout ('location.reload (true)', 60*1000*60)}
                                                            </script>");

                        await context.Response.WriteAsync(
                        "<b>Показать данные:</b><br> <a href='http://localhost:7285/api/Data/Show'>Click<a>" + "<br>" +
                        "<b>Перезаписать данные(<u>Осторожно: Ресурснозатратная операция!</u>):</b><br> <a href='http://localhost:7285/api/Data/Create'>Click</a>" + "<br>" +
                        "<b>Обновить данные:</b><br> <a href='http://localhost:7285/api/Data/Update'>Click</a>" + "<br>" +
                        "<b>Экспортировать в Json:</b><br> <a href='http://localhost:7285/api/Data/Export'>Click</a>" + "<br>");
                    }
                });
            });
        }
    }
}