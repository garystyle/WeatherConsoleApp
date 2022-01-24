using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherConsoleApp.Services;


namespace WeatherConsoleApp
{

  public class Runner
  {
    private readonly ILogger<Runner> _logger;
    private readonly LineNotifyService _lineNotifyService;
    private readonly WeatherService _weatherService;

    public Runner(ILogger<Runner> logger, LineNotifyService lineNotifyService, WeatherService weatherService)
    {
      _logger = logger;
      _lineNotifyService = lineNotifyService;
      _weatherService = weatherService;
    }

    public async Task Run()
    {
      _logger.LogInformation("Start Run-----------");

      string message = await _weatherService.GetWeatherData();

      await _lineNotifyService.SendTextAsync(message);

      _logger.LogInformation("Finish Run-----------");
      _logger.LogInformation("--------------------------------------------------");
    }
  }
}