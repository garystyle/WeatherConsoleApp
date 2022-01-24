using System;
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
  public class LineNotifyService
  {
    private const string LineNotifyAPI = "https://notify-api.line.me/api/notify";
    private const string LineTokenAPI = "https://notify-bot.line.me/oauth/token";
    private const string LineStatusAPI = "https://notify-api.line.me/api/status";
    private const string LineRevokeAPI = "https://notify-api.line.me/api/revoke";
    private readonly LineNotify _settings;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<LineNotifyService> _logger;

    public LineNotifyService(IOptionsMonitor<AppSetting> settings, IHttpClientFactory clientFactory, ILogger<LineNotifyService> logger)
    {
      _settings = settings.CurrentValue.LineNotify;
      _clientFactory = clientFactory;
      _logger = logger;
    }

    public async Task<bool> SendTextAsync(string token, string message)
    {
      return false;
    }

    /// <summary>
    /// 傳送純文字訊息給使用者
    /// </summary>
    /// <param name="token">access token</param>
    /// <param name="message">欲傳送的文字內容</param>
    public async Task<bool> SendTextAsync(string message)
    {
      _logger.LogWarning($"Start SendTextAsync,Token : {_settings.Token} , Message : {message} ");

      HttpClient client = _clientFactory.CreateClient();

      client.BaseAddress = new Uri(LineNotifyAPI);

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Token);

      //StringContent content = new StringContent($"message={message}", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

      var content = new FormUrlEncodedContent(new[]
                        {
                                new KeyValuePair<string, string>("message", message),
                                new KeyValuePair<string, string>("stickerPackageId", "6362"),
                                new KeyValuePair<string, string>("stickerId", "11087943"),
                                new KeyValuePair<string, string>("notificationDisabled", "false")
                              });

      var response = await client.PostAsync("", content);

      if (!response.IsSuccessStatusCode)
      {
        string responseContent = await response.Content.ReadAsStringAsync();
        _logger.LogError($"SendTextAsync Error : {responseContent} , Token: {_settings.Token} 。");

        return false;
      }

      return true;
    }

    /// <summary>
    /// 集體送發訊息
    /// </summary>
    /// <param name="tokens">所有使用者的 access token</param>
    /// <param name="message">欲傳送的訊息內容</param>
    public async Task SendTextAsync(IEnumerable<string> tokens, string message)
    {
      List<Task> tasks = new List<Task>();
      foreach (var t in tokens)
      {
        tasks.Add(SendTextAsync(t, message));
      }
      await Task.WhenAll(tasks);
    }

    /// <summary>
    /// 解除綁定 (access token 將失效)
    /// </summary>
    /// <param name="token">user access token</param>
    public async Task<bool> RevokeAsync(string token)
    {
      if (string.IsNullOrEmpty(token)) return false;

      _logger.LogWarning($"Start RevokeAsync,Token : {token} ");

      HttpClient client = _clientFactory.CreateClient();

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      StringContent content = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

      var response = await client.PostAsync(LineRevokeAPI, content);

      if (!response.IsSuccessStatusCode)
      {
        string responseContent = await response.Content.ReadAsStringAsync();
        _logger.LogError($"RevokeAsync Error : {responseContent} , Token: {token} 。");

        return false;
      }

      return true;
    }

    /// <summary>
    /// 解除綁定 (access token 將失效)
    /// </summary>
    /// <param name="tokens">所有使用者的 access token</param>
    public async Task RevokeAsync(IEnumerable<string> tokens)
    {
      List<Task> tasks = new List<Task>();
      foreach (var t in tokens)
      {
        tasks.Add(RevokeAsync(t));
      }
      await Task.WhenAll(tasks);
    }
  }


}
