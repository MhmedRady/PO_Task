using Microsoft.EntityFrameworkCore;
using PO_Task.Api.Middleware;
using PO_Task.Domain.Users;
using PO_Task.Infrastructure;

namespace PO_Task.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static async void ApplyMigrations(this IApplicationBuilder applicationBuilder)
    {
        using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContext.Database.Migrate();
            await UsersSeeding(dbContext);
        }
        catch (Exception ex)
        {
            ILogger logger = applicationBuilder.ApplicationServices.GetRequiredService<ILogger<ApplicationDbContext>>();
            logger.LogError(
                ex,
                "An error occured During Migration");
        }
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }

    public static async Task UsersSeeding(ApplicationDbContext dbContext)
    {
        if (dbContext.user_profile.Any())
            return;

        for (int i = 1; i < 5; i++)
        {
            await dbContext.user_profile.AddAsync(User.CreateInstance(new("Mohamed"), new($"R{i}"), new($"MohamedR{i}@PO.com")));
        }

        await dbContext.SaveChangesAsync();
    }
}
