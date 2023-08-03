using Hangfire;
using Hangfire.Storage.SQLite;
using Sentry;
using Sentry.Extensibility;
using SentryBackgroundJobShowcase;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.WebHost.UseSentry();

builder.Services
    .AddHangfireServer()
    .AddHangfire(configs => configs
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddAuthorization()
    .AddHostedService<AddBackgroundJobHostedService>()
    .AddTransient<ISentryEventProcessor, MyEventProcessor>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Run();