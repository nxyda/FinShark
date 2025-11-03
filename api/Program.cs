using Microsoft.OpenApi.Models; // potrzebne do OpenApiInfo

var builder = WebApplication.CreateBuilder(args);

// Dodajemy klasyczny Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FinShark API",
        Version = "v1",
        Description = "Dokumentacja API dla FinShark"
    });
});

var app = builder.Build();

// W pipeline włączamy Swagger tylko w Development (możesz zmienić jeśli chcesz)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // udostępnia /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinShark API v1");
        // opcjonalnie: c.RoutePrefix = string.Empty; // ustawi swagger na root '/'
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
