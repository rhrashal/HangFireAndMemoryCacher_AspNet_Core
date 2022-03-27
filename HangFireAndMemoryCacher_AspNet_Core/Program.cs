using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseHangfireDashboard();

//execute on run time
var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world-- first time"));
//execute after given time from sarver run
BackgroundJob.Schedule(() => Console.WriteLine("Hello, world-- after given time"), TimeSpan.FromSeconds(10));
//execute continue after given time
RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello, world-- continue "), Cron.MinuteInterval(1));

app.UseEndpoints(endpoints => 
{
    endpoints.MapHangfireDashboard();
});

app.Run();

//https://docs.hangfire.io/en/latest/getting-started/aspnet-core-applications.html