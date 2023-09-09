using consumer.TypedHttpClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace consumer.Functions
{
    public class TimerTriggered
    {
        private readonly IConfiguration _configuration;
        private readonly TemperatureHttpClient _httpClient;

        public TimerTriggered(IConfiguration configuration, TemperatureHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [Function("TimerTriggered")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] FunctionContext context)
        {
            var logger = context.GetLogger<TimerTriggered>();

            try
            {
                logger.LogInformation($"### New request");

                var endpoint = _configuration.GetValue<string>(Constants.ProducerEndpoint);

                var temperatureVariation = await _httpClient.GetProducer(endpoint, 18);

                logger.LogInformation($"### Temperature = {temperatureVariation}");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
