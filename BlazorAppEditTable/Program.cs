using BlazorAppEditTable.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazored.Modal;
using Blazored.Toast;
using BlazorAppEditTable.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<MyDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddSingleton<ApplicationState>();
builder.Services.AddScoped<DatabaseMetaDataService>();
builder.Services.AddScoped<TableStructureServices>();
builder.Services.AddScoped<DynamicUpdates>();
builder.Services.AddScoped<IDynamicTableRepository, DynamicTableRepository>();
builder.Services.AddScoped<IDynamicTableDataService, DynamicTableDataService>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
