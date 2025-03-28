using System.Diagnostics;
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
        using var activity = MonitorService.ActivitySource.StartActivity("TextController - Get")!; 
        activity?.SetTag("languageCode", languageCode);

        try
        {
            MonitorService.Log.Information("Greet in GreetingService called from TextController");

            using var activityGreeting = MonitorService.ActivitySource.StartActivity("GreetingService - Greet");
            activityGreeting?.SetTag("languageCode", languageCode);

            var greeting = GreetingService.Instance.Greet(new GreetingRequest { LanguageCode = languageCode });

            try
            {
                MonitorService.Log.Information("GetPlanet in PlanetService called from TextController");

                using var activityPlanet = MonitorService.ActivitySource.StartActivity("PlanetService - GetPlanet");
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
                
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                MonitorService.Log.Error(ex, "Error in GetPlanet from PlanetService");
                return StatusCode(500, "An error occurred while fetching the planet");
            }
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            MonitorService.Log.Error(ex, "Error in GetLanguages from LanguageService");
            return StatusCode(500, "An error occurred while fetching the greeting");
        }
    }
}