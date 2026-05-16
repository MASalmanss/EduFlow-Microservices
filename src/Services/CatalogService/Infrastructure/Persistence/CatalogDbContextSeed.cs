using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence;

public static class CatalogDbContextSeed
{
    public static async Task SeedAsync(CatalogDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var backend = new Category("Backend");
        var frontend = new Category("Frontend");
        var mobile = new Category("Mobile");
        var devops = new Category("DevOps");

        var categories = new List<Category> { backend, frontend, mobile, devops };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var courses = new List<Course>
        {
            new("ASP.NET Core ile Mikroservis Geliştirme",
                "Docker, RabbitMQ ve API Gateway kullanarak üretim düzeyinde mikroservisler oluşturun.",
                499.99m, "user-seed-1", backend.Id, "https://placehold.co/600x400?text=ASP.NET+Core"),

            new("Entity Framework Core: Sıfırdan İleri Seviyeye",
                "EF Core ile Code-First yaklaşımı, migration'lar ve ileri düzey sorgulama tekniklerini öğrenin.",
                349.99m, "user-seed-1", backend.Id, "https://placehold.co/600x400?text=EF+Core"),

            new("React ile Modern Web Geliştirme",
                "Hooks, Context API ve Redux kullanarak ölçeklenebilir React uygulamaları geliştirin.",
                399.99m, "user-seed-2", frontend.Id, "https://placehold.co/600x400?text=React"),

            new("Flutter ile Çapraz Platform Uygulama Geliştirme",
                "iOS ve Android için Flutter ile native performanslı mobil uygulamalar oluşturun.",
                449.99m, "user-seed-2", mobile.Id, "https://placehold.co/600x400?text=Flutter"),

            new("Docker ve Kubernetes: Konteyner Orkestrasyonu",
                "Konteyner yönetimi, Kubernetes cluster kurulumu ve CI/CD pipeline oluşturmayı öğrenin.",
                549.99m, "user-seed-3", devops.Id, "https://placehold.co/600x400?text=K8s")
        };

        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();
    }
}
