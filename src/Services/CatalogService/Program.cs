using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMongoDB(
        builder.Configuration.GetConnectionString("MongoDB")!,
        "CatalogDb"));

var app = builder.Build();

app.Run();
