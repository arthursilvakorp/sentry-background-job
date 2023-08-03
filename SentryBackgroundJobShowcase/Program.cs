using Hangfire;
using Hangfire.Storage.SQLite;
using Sentry.Extensibility;
using SentryBackgroundJobShowcase;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry();

builder.Services
    .AddHangfireServer()
    .AddHangfire(configs => configs
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddHostedService<AddBackgroundJobHostedService>()
    .AddTransient<ISentryEventProcessor, MyEventProcessor>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.Run();