﻿using Blazorfirebase.Client.Models;
using Refit;

namespace Blazorfirebase.Client.Network;

public interface IWeatherApiService
{
    [Get("/WeatherForecast")]
    Task<List<WeatherForecast>> GetWeather();
}
