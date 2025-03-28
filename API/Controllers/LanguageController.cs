using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("GetLanguages in MonitorService called from controller");
        try
        {
            MonitorService.Log.Information("GetLanguages in LanguageService called");
            var language = LanguageService.Instance.GetLanguages();
            return Ok(new GetLanguageModel.Response { DefaultLanguage = language.DefaultLanguage, Languages = language.Languages });
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GetLanguages from LanguageService");
            return StatusCode(500, "An error occurred");
        }

    }
}