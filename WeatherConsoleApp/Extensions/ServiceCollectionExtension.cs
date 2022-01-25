using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherConsoleApp.Services;


namespace WeatherConsoleApp.Extensions
{
  public static class ServiceCollectionExtension
  {
    public static void AddJobService(this IServiceCollection services)
    {
      services.AddHttpClient<WeatherService>();
      services.AddSingleton<WeatherService>();
      services.AddHttpClient<LineNotifyService>();
      services.AddSingleton<LineNotifyService>();
    }

  }
}
