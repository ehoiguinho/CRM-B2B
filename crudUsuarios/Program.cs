using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

  builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/usuarios", async (AppDbContext context) =>
{
    var usuarios = await context.Usuarios.ToListAsync();

    return usuarios;
    
});

app.MapGet("/usuarios/{id}", async (AppDbContext context, int id) =>
{
    var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    
    if(usuario == null)
    {
       return Results.NotFound();
    }
       return Results.Ok(usuario);
    

});

app.MapPost("/usuarios", async (AppDbContext context, Usuario usuario) =>
{
    await context.Usuarios.AddAsync(usuario);

    var result = await context.SaveChangesAsync();

    return result;
});

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
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

