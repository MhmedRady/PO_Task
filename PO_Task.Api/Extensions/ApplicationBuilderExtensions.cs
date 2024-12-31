using Aswaq.Api.Middleware;
using Aswaq.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PO_Task.Infrastructure;

namespace Aswaq.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
    {
        using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContext.Database.Migrate();
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
}
