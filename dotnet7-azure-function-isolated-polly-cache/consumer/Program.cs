using consumer.TypedHttpClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Caching.Memory;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var currentDirectory = hostingContext.HostingEnvironment.ContentRootPath;

        config.SetBasePath(currentDirectory)
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        config.Build();
    })
    .ConfigureServices((services) =>
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var memoryCacheProvider = new MemoryCacheProvider(memoryCache);

        var cachePolicy = Policy
            .CacheAsync<HttpResponseMessage>(memoryCacheProvider, TimeSpan.FromSeconds(30));

        services.AddHttpClient<TemperatureHttpClient>()
            .AddPolicyHandler(cachePolicy);
    })
    .Build();

host.Run();
