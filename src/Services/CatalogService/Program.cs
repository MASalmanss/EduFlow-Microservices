using CatalogService.Infrastructure.Extensions;
using EduFlow.Shared.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSharedServices(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.Run();
