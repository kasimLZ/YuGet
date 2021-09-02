using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace YuGet
{
	public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyBlazor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseMyBlazor();

            app.UseEndpoints(a =>
            {
                a.MapControllers();
                a.MapControllerRoute("default", "/{controller}/{action}", new
                {

                });
            });

        }
    }
}
