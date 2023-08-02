using Hangfire;

namespace SentryBackgroundJobShowcase;

public class AddBackgroundJobHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists("123");
        RecurringJob.AddOrUpdate<BackgroundJob>("123", x => x.Execute(), Cron.Daily);
        RecurringJob.TriggerJob("123");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
}