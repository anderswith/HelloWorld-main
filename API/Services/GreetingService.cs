namespace API.Services;

public class GreetingService
{
    private static GreetingService? _instance;
    
    public static GreetingService Instance
    {
        get { return _instance ??= new GreetingService(); }
    }
    
    private GreetingService()
    { }
    
    public GreetingResponse Greet(Messages.GreetingRequest request)
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Getting GreetResponse");
        try
        {
            var language = request.LanguageCode;
            var greeting = language switch
            {
                "en" => "Hello",
                "es" => "Hola",
                "fr" => "Bonjour",
                "de" => "Hallo",
                "it" => "Ciao",
                "pt" => "Olá",
                "ru" => "Привет",
                "zh" => "你好",
                "ja" => "こんにちは",
                "ar" => "مرحبا",
                "hi" => "नमस्ते",
                "sw" => "Hujambo"
            };
            return new GreetingResponse { Greeting = greeting };
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GreetResponse from GreetingService");
            throw;
        }
    }
    
    public string[] GetLanguages()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Getting Languages");
        try
        {
            return new[] { "en", "es", "fr", "de", "it", "pt", "ru", "zh", "ya", "ar", "hi", "sw" };
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GetLanguages from GreetingService");
            throw;
        }
    }
}