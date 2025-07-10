using Microsoft.EntityFrameworkCore;
using PokerPlanning.Api;
using PokerPlanning.Api.Hubs;
using PokerPlanning.Application;
using PokerPlanning.Infrastructure;
using PokerPlanning.Infrastructure.src.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddSignalR();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    /*builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();*/
}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /*app.UseSwagger();
    app.UseSwaggerUI();*/
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PokerPlanningDbContext>();
        var IsTesting = builder.Configuration.GetValue<bool>("IsTesting");
        if (IsTesting)
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        else
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}

app.UseCors(options => options.WithOrigins(new[] { "http://localhost:3000", "https://localhost:3000", "https://poker-planning.io" }).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapHub<GameHub>("/hubs/game");
app.MapControllers();
app.Run();

public partial class Program { }