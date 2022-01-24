using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherConsoleApp.Models
{
  public class Weather
  {
    public Weather() { }

    public DateTime starttime { get; set; }

    public DateTime endtime { get; set; }

    /// <summary>
    /// 天氣狀況
    /// </summary>
    public Parameter Wx { get; set; }

    /// <summary>
    /// 降雨機率
    /// </summary>
    public Parameter1 PoP { get; set; }

    /// <summary>
    /// 最低溫度
    /// </summary>
    public Parameter1 MinT { get; set; }

    /// <summary>
    /// 舒適度
    /// </summary>
    public Parameter CI { get; set; }

    /// <summary>
    /// 最高溫度
    /// </summary>
    public Parameter1 MaxT { get; set; }

  }

  public class Parameter
  {
    public string parameterName { get; set; }

    public string parameterValue { get; set; }
  }

  public class Parameter1
  {
    public string parameterName { get; set; }

    public string parameterUnit { get; set; }
  }
}
