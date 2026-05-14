using System.Text.Json;

namespace LLMWorkshop;

public class WeatherEntry
{
    public DateOnly Date { get; set; }
    public string City { get; set; }
    public int TemperatureCelsius { get; set; }
    public string Forecast { get; set; }
}

public static class Functions
{
    
    public static string GetWeather(string city)
    {
        // return 8 entries, from today to the same day next week, with random weather forecasts and temperatures
        var entries = new WeatherEntry[]
        {
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now), City = city, Forecast = "Sunny", TemperatureCelsius = 28 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), City = city, Forecast = "Cloudy", TemperatureCelsius = 20 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), City = city, Forecast = "Rainy", TemperatureCelsius = 14 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), City = city, Forecast = "Rainy", TemperatureCelsius = 12 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), City = city, Forecast = "Rainy", TemperatureCelsius = 11 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), City = city, Forecast = "Rainy", TemperatureCelsius = 7 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(6)), City = city, Forecast = "Sunny", TemperatureCelsius = 17 },
            new WeatherEntry() { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(7)), City = city, Forecast = "Sunny", TemperatureCelsius = 19 },
        };
        return JsonSerializer.Serialize(entries);
    }
}
