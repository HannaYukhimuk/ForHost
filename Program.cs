using MyPresentationApp.Data;
using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация сервисов
builder.Services.AddControllersWithViews();

// Настройка БД
var connectionString = GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString));

// Другие сервисы
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PresentationService>();
builder.Services.AddScoped<ElementService>();
builder.Services.AddScoped<SlideService>();

var app = builder.Build();

// Применение миграций ДО настройки middleware
try 
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Migration failed!");
    throw; // Прерываем запуск приложения при ошибке миграции
}

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

string GetConnectionString(IConfiguration config)
{
    var host = config["DB_HOST"];
    var port = config["DB_PORT"];
    var dbName = config["DB_NAME"];
    var dbUser = config["DB_USER"];
    var dbPassword = config["DB_PASSWORD"];
    
    // Логирование для отладки (удалите в продакшене)
    Console.WriteLine($"Connecting to: Host=dpg-d12v1595pdvs73d66el0-a;Port=5432;Database=mypresentationapp;Username=mypresentationapp_user");
    
    return $"Host=dpg-d12v1595pdvs73d66el0-a;Port=5432;Database=mypresentationapp;Username=mypresentationapp_user";
}