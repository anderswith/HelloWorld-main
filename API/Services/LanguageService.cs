using Messages;

namespace API.Services;

public class LanguageService
{
    private static LanguageService? _instance;
    
    public static LanguageService Instance
    {
        
        get { return _instance ??= new LanguageService(); }
    }
    
    private LanguageService()
    { }
    
    public LanguageResponse GetLanguages()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Getting Languages in LanguageService");
        try
        {
            return new LanguageResponse
            {
                Languages = GreetingService.Instance.GetLanguages()
            };
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GetLanguages from LanguageService");
            throw;
        }
    }
}