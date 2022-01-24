using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherConsoleApp.Models;
using WeatherConsoleApp.Configurations;

namespace WeatherConsoleApp.Services
{
  public class WeatherService
  {
    private readonly ILogger<WeatherService> _logger;
    private readonly IHttpClientFactory _clientFactory;

    private readonly AppSetting _settings;

    public WeatherService(ILogger<WeatherService> logger, IHttpClientFactory clientFactory, IOptionsMonitor<AppSetting> settings)
    {
      _logger = logger;
      _clientFactory = clientFactory;
      _settings = settings.CurrentValue;
    }

    public async Task<string> GetWeatherData()
    {
      string url = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001";
      string parameter = $"?Authorization={_settings.Authorization}&format=JSON&locationName=臺北市";

      StringBuilder SB = new StringBuilder();

      HttpClient _httpClient = _clientFactory.CreateClient();
      _httpClient.BaseAddress = new Uri(url);

      try
      {
        var response = await _httpClient.GetAsync(url + parameter).ConfigureAwait(false);
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        JObject jsondata = JsonConvert.DeserializeObject<JObject>(responseString); //將資料轉為json物件
        var result = (JArray)jsondata["records"]["location"]; //回傳json陣列

        List<Weather> weathers = new List<Weather>();

        foreach (JObject data in result)
        {
          SB.AppendLine(Environment.NewLine + "*" + (string)data["locationName"] + "--未來天氣預報"); //地名

          for (int i = 1; i < 3; i++)
          {
            Weather weather = new Weather();
            //開始時間
            weather.starttime = (DateTime)data["weatherElement"][0]["time"][i]["startTime"];
            //結束時間
            weather.endtime = (DateTime)data["weatherElement"][0]["time"][i]["endTime"];
            //天氣狀況
            weather.Wx = JsonConvert.DeserializeObject<Parameter>(data["weatherElement"][0]["time"][i]["parameter"].ToString());
            //降雨機率
            weather.PoP = JsonConvert.DeserializeObject<Parameter1>(data["weatherElement"][1]["time"][i]["parameter"].ToString());
            //最低溫度
            weather.MinT = JsonConvert.DeserializeObject<Parameter1>(data["weatherElement"][2]["time"][i]["parameter"].ToString());
            //舒適度
            weather.CI = JsonConvert.DeserializeObject<Parameter>(data["weatherElement"][3]["time"][i]["parameter"].ToString());
            //最高溫度
            weather.MaxT = JsonConvert.DeserializeObject<Parameter1>(data["weatherElement"][4]["time"][i]["parameter"].ToString());

            weathers.Add(weather);
          }

          foreach (var item in weathers)
          {
            SB.AppendLine($"開始時間 : {item.starttime} \r\n結束時間 : {item.endtime}" + Environment.NewLine +
                $"天氣狀況 : {item.Wx.parameterName + item.Wx.parameterValue} , 舒適度 : { item.CI.parameterName + item.CI.parameterValue}" + Environment.NewLine +
                $"最高溫度 : {item.MaxT.parameterName + item.MaxT.parameterUnit} , 最低溫度 : {item.MinT.parameterName + item.MinT.parameterUnit}" + Environment.NewLine +
                $"降雨機率 : { item.PoP.parameterName + item.PoP.parameterUnit}" + Environment.NewLine);

          }
          SB.AppendLine($"天氣即時預報FB粉絲團:");
          SB.AppendLine($"https://www.facebook.com/pg/weather.taiwan/posts/?ref=page_internal");

        }
      }
      catch (System.Exception ex)
      {
        _logger.LogError($"Error! Message : {ex.Message}");
        SB.Clear();
        SB.Append($"Error! Message: { ex.Message}");
      }

      return SB.ToString();
    }
  }
}
