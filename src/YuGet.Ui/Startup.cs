using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Ui.Data;

namespace YuGet
{
	public static class StartupUIExtension
    {
        public static void AddMyBlazor(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        public static void UseMyBlazor(this IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
