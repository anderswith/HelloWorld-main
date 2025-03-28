namespace API.Services;

public class PlanetService
{
    private static PlanetService? _instance;
    
    public static PlanetService Instance
    {
        get { return _instance ??= new PlanetService(); }
    }

    public PlanetResponse GetPlanet()
    {
        using var activity = MonitorService.ActivitySource.StartActivity("Getting Planet in PlanetService");
        try
        {
            var planets = new[]
            {
                "Mercury",
                "Venus",
                "Earth",
                "Mars",
                "Jupiter",
                "Saturn",
                "Uranus",
                "Neptune"
            };

            var index = new Random(DateTime.Now.Millisecond).Next(1, planets.Length + 1);
            return new PlanetResponse
            {
                Planet = planets[index]
            };
        }
        catch (Exception ex)
        {
            MonitorService.Log.Error(ex, "Error in GetPlanet from PlanetService");
            throw;
        }
    }
}