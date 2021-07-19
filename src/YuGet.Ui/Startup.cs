using MatBlazor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace YuGet
{
	public static class StartupUIExtension
    {
        public static void AddMyBlazor(this IServiceCollection services)
        {
            services.AddMatBlazor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
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
