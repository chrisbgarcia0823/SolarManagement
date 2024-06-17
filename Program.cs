using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarManagement.BackgroundTask;
using SolarManagement.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SolarManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LiveServer") ?? throw new InvalidOperationException("Connection string 'SolarManagementContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//add this for hosted service
builder.Services.AddSingleton<IHostedService, TimedHostedService>();

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
    pattern: "{controller=Home}/{action=BatteryMonitoring}/{id?}");

app.Run();
