using CatalogService.Infrastructure.Extensions;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSharedServices(Assembly.GetExecutingAssembly());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await CatalogDbContextSeed.SeedAsync(context);
}

app.MapEndpoints();

app.Run();
