using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace YuGet
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseMyBlazor();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/files", async context =>
                {
                    var options = context.RequestServices.GetRequiredService<IOptions<StaticFileOptions>>().Value;

                    string PrintPhysicalPath(IFileProvider provider)
					{
                        if (provider is CompositeFileProvider Composite) 
                        {
                            var sb = new StringBuilder();
                            foreach(var inner in Composite.FileProviders)
							{
                                sb.Append(PrintPhysicalPath(inner));
                            }
                            return sb.ToString();
                        }

						else if (provider.GetType().Name == "StaticWebAssetsFileProvider")
						{

                            try
							{
                                PathString p = (PathString)provider.GetType().GetProperty("BasePath").GetValue(provider);
                                PhysicalFileProvider physical = (PhysicalFileProvider)provider.GetType().GetProperty("InnerProvider").GetValue(provider);

                                return $"<tr><td>{p}</td><td>{physical.Root}</td></tr>";
                            }
                            catch
							{
                                return "rror";
							}
						}
                        else if (provider is PhysicalFileProvider physicalFile) 
                        {
                            return $"<tr><td></td><td>{physicalFile.Root}</td></tr>";
                        }
                        return $"<tr><td>{provider.GetType().FullName}</td></tr>";
                    }

                    await context.Response.WriteAsync("<table><tr><th>name</th><th>path</th></tr>" + PrintPhysicalPath(options.FileProvider) + "</table>");
                });
            });
        }
    }
}
