using API.Services;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .WriteTo.Console()
    .WriteTo.Seq("http://seq:5341")
);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyAPI"))
        .AddAspNetCoreInstrumentation()  
        .AddHttpClientInstrumentation() 
        .AddZipkinExporter(opt => opt.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
    );
// Add services to the container.
builder.Services.AddScoped<GreetingService>();
builder.Services.AddScoped<PlanetService>();
builder.Services.AddScoped<LanguageService>();


builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
    MonitorService.TracerProvider.ForceFlush();
}