using API.Models;
using API.Services;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string languageCode)
    {
        try
        {
            using var activity =
                MonitorService.ActivitySource.StartActivity("Greet in GreetingService called from TextController");
            var greeting = GreetingService.Instance.Greet(new GreetingRequest { LanguageCode = languageCode });

            try
            {
                using var activity2 =
                    MonitorService.ActivitySource.StartActivity(
                        "GetPlanet in PlanetService called from TextController");
                var planet = PlanetService.Instance.GetPlanet();

                var response = new GetGreetingModel.Response
                {
                    Greeting = greeting.Greeting,
                    Planet = planet.Planet
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                MonitorService.Log.Error(ex, "Error in GetPlanet from PlanetService");
                return StatusCode(500, "An error occurred while fetching the planet");
            }
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GetLanguages from LanguageService");
            return StatusCode(500, "An error occurred while fetching the greeting");
        }
    }
}