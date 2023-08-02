using Hangfire;
using Hangfire.Storage.SQLite;
using Sentry;
using Sentry.Extensibility;
using SentryBackgroundJobShowcase;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


builder.Host.UseSerilog((_, loggerConfiguration) =>
{ 
    loggerConfiguration.ReadFrom.Configuration(configuration);
});

builder.WebHost.UseSentry(
    o =>
{
    o.AddEventProcessor(new MyEventProcessor());
}
    );

builder.Services
    .AddHangfireServer()
    .AddHangfire(configs => configs
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddAuthorization()
    .AddHostedService<AddBackgroundJobHostedService>()
    //.AddTransient<ISentryEventProcessor, MyEventProcessor>()
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Run();