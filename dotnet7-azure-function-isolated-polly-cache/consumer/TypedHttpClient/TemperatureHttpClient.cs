using Polly;

namespace consumer.TypedHttpClient
{
    public class TemperatureHttpClient
    {
        private readonly HttpClient _httpClient;

        public TemperatureHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetProducer(string endpoint, double initialTemperature)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri($"{endpoint}?temperature={initialTemperature}"));

            // Operation key is used to identify the operation in the cache
            httpRequestMessage.SetPolicyExecutionContext(new Context("GetProducer"));

            var response = await _httpClient.SendAsync(httpRequestMessage);
            return await response.Content.ReadAsStringAsync();
        }
    }
}