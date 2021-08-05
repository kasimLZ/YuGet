using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using YuGet;

public class Program
{
    public static void Main(string[] args)
    {
        
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => {
                services.AddSQLServerYuGetDbContext();
                services.AddSQLiteYuGetDbContext();
                services.AddPostgreSQLYuGetDbContext();
                services.AddMySQLYuGetDbContext();

                services.AddYuGetDbContext();
            });
}