using Timesheets.Infrastructure;
using Timesheets.Models;
using Timesheets.Repositories;
using Timesheets.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews().AddRazorRuntimeCompilation();
services.AddDbContext<DataContext>();
services.AddScoped<ITimesheetService, TimesheetService>();
services.AddScoped<ITimesheetRepository, TimesheetRepository>();
services.AddScoped<ICsvExportService<ProjectTimesheet>, CsvExportService<ProjectTimesheet>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Timesheet/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Timesheet}/{action=Index}/{id?}");

app.Run();
