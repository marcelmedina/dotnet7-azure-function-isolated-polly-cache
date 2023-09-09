using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace producer.Functions
{
    public class Temperature
    {
        private readonly ILogger _logger;

        public Temperature(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Temperature>();
        }

        [Function("Temperature")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Get temperature called.");

            var initialTemperature = req.Query["temperature"];

            var temperature = GetTemperature(double.Parse(initialTemperature));

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(temperature);
            return response;
        }

        private double GetTemperature(double initialTemperature)
        {
            // Generate a random temperature variation between -2 and +2 degrees Celsius
            var random = new Random();
            var temperatureVariation = random.NextDouble() * 4.0 - 2.0;

            return initialTemperature + temperatureVariation;
        }
    }
}
