namespace WeatherConsoleApp.Configurations
{
  public class AppSetting
  {
    public LineNotify LineNotify { get; set; }

    public string Authorization { get; set; }

  }

  public class LineNotify
  {
    public string ClientID { get; set; }

    public string ClientSecret { get; set; }

    public string CallbackURL { get; set; }

    public string Token { get; set; }
  }
}
