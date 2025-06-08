using MyPresentationApp.Data;
using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); 

// Получаем строку подключения
var host = builder.Configuration["DB_HOST"];
var port = builder.Configuration["DB_PORT"];
var dbName = builder.Configuration["DB_NAME"];
var dbUser = builder.Configuration["DB_USER"];
var dbPassword = builder.Configuration["DB_PASSWORD"];

var connectionString = $"Host={host};Port={port};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PresentationService>();
builder.Services.AddScoped<ElementService>();
builder.Services.AddScoped<SlideService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
