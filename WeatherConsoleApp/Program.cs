using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherConsoleApp.Configurations;
using WeatherConsoleApp.Services;
using WeatherConsoleApp.Extensions;
using NLog.Web;
using NLog.Extensions.Logging;

namespace WeatherConsoleApp
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

      //建立依賴注入的容器
      IServiceCollection services = new ServiceCollection();

      try
      {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //註冊設定檔
        IConfiguration _configuration = new ConfigurationBuilder()
                                        .AddJsonFile($"appsettings.json")
                                        .AddJsonFile($"appsettings.{environmentName}.json", true).Build();
        services.Configure<AppSetting>(_configuration.GetSection("ServiceSetting"));
        //使用NLog 依賴注入
        services.AddLogging(logging =>
           {
             logging.AddConsole();
             logging.AddNLog();
           }
        );

        //註冊服務執行者
        services.AddTransient<Runner>();
        //註冊各類工作服務
        services.AddJobService();

        //建立依賴服務提供者
        var serviceProvider = services.BuildServiceProvider();
        //由服務執行者啟動各類工作
        await serviceProvider.GetRequiredService<Runner>().Run();

        //Console.Read();
      }
      catch (Exception ex)
      {
        logger.Error(ex, "Stopped program because of exception");
        throw;
      }
      finally
      {
        // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        NLog.LogManager.Shutdown();
      }

    }
  }
}
