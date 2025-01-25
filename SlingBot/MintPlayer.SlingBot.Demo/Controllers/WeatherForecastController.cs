using Microsoft.AspNetCore.Mvc;

namespace MintPlayer.SlingBot.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration configuration;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Env")]
        public string[] Env()
        {
            return [
                Environment.GetEnvironmentVariable("WebhookProxy:Username") ?? string.Empty,
                Environment.GetEnvironmentVariable("WebhookProxy:Password") ?? string.Empty,
                configuration.GetValue<string>("WebhookProxy:Username") ?? string.Empty,
                configuration.GetValue<string>("WebhookProxy:Password") ?? string.Empty,
            ];
        }
    }
}
