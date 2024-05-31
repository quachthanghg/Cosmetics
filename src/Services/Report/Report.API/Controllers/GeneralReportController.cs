using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralReportController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        [HttpGet("export")]
        public IEnumerable<WeatherForecast> GetList()
        {
            return new List<WeatherForecast>()
            {
                new WeatherForecast()
                {
                    Date = DateTime.Now,
                    Summary ="ThangQV",
                    TemperatureC = 1
                }
            };
        }
    }
}
