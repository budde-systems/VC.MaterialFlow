using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlueApps.MaterialFlow.Common.Connection.Client.Http;

public class WMS_Client //TODO: Use httpclient_factory
{
    public Dictionary<string, string>? Headers { get; set; }

    private readonly ILogger<WMS_Client> _logger;



    public WMS_Client(ILogger<WMS_Client> logger)
    {
        _logger = logger;
    }

    public void AddHeader(string key, string value)
    {
        if (Headers is null)
            Headers = new Dictionary<string, string>();

        Headers.Add(key, value);
    }

    public void RemoveHeader(string key)
    {
        if (Headers is null)
            return;

        Headers.Remove(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <exception cref="HttpRequestException"></exception>
    public async Task PostAsync<T>(string? url, T? data)
    {
        if (data is null || url is null)
        {
            _logger.LogWarning("The data or the url is null");
            return;
        }

        var jsonData = GetJsonData(data);

        if (!string.IsNullOrEmpty(jsonData))
        {
            try
            {
                await Send(url, jsonData, HttpMethod.Post);
                _logger.LogInformation($"Data that will be patched:\n{jsonData}");
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError($"An error occurs in the POST-method. The statuscode is {exception.StatusCode}");
                throw;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <exception cref="HttpRequestException"></exception>
    public async Task PatchAsync<T>(string url, T data)
    {
        var jsonData = GetJsonData(data);

        if (!string.IsNullOrEmpty(jsonData))
        {
            try
            {
                await Send(url, jsonData, HttpMethod.Patch);
                _logger.LogInformation($"Data that will be patched:\n{jsonData}");
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError($"An error occurs in the PATCH-method. The statuscode is {exception.StatusCode}, " +
                                 $"message of exception is {exception.Message}");
                throw;
            }
        }
    }

    private async Task Send(string url, string data, HttpMethod httpMethod)
    {
        using var client = new HttpClient();

        CreateHeader(client);

        var request = new HttpRequestMessage(httpMethod, url);

        request.Content = new StringContent(data, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private void CreateHeader(HttpClient client)
    {
        if (client is null) return;

        if (Headers != null && Headers.Count > 0)
        {
            foreach (var kp in Headers)
            {
                client.DefaultRequestHeaders.Add(kp.Key, kp.Value);
            }
        }
    }

    private string GetJsonData<T>(T data)
    {
        if (data is null)
            return string.Empty;

        try
        {
            var jsonData = JsonSerializer.Serialize(data, typeof(T));

            return jsonData;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
            return string.Empty;
        }
    }

    public async Task<T> Get<T>(string url)
    {
        using var client = new HttpClient();
            
        CreateHeader(client);
            
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.ToString());
            return default!;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns>null if something went wrong</returns>
    public async Task<Stream?> GetStream(string url)
    {
        using var client = new HttpClient();

        CreateHeader(client);

        try
        {
            var result = await client.GetAsync(url);
            var stream = await result.Content.ReadAsStreamAsync();

            return stream;
        }
        catch ( Exception exception)
        {
            _logger.LogError(exception.ToString());
            return null;
        }
    }
}