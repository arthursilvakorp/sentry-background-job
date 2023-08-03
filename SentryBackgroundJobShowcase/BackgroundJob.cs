using Sentry;

namespace SentryBackgroundJobShowcase;

public class BackgroundJob
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Execute()
    {
        Console.WriteLine("Background Job Running");
        try
        {
            throw new Exception("Showcase Exception");
        }
        catch (Exception exception)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<BackgroundJob>();
                logger.LogError(exception, "Error while processing background job");
            }
        }

        return Task.CompletedTask;
    }
}