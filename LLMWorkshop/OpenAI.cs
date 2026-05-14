using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LLMWorkshop;

public sealed class OpenAi
{
    private readonly HttpClient client;

    public OpenAi()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("OPENAI_API_KEY missing");

        var apiUrl = Environment.GetEnvironmentVariable("OPENAI_URL");
        if (string.IsNullOrWhiteSpace(apiUrl))
            throw new Exception("OPENAI_URL missing");

        this.client = new HttpClient() 
        {
            BaseAddress = new Uri(apiUrl),
            Timeout = TimeSpan.FromSeconds(90)
        };
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string> PostAsync(object payload)
    {
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { WriteIndented = true });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await this.client.PostAsync("v1/chat/completions", content);
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
}