using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Add services to the container.
builder.Services.AddControllersWithViews();
//

// var host = Environment.GetEnvironmentVariable("DB_HOST");
// var port = Environment.GetEnvironmentVariable("DB_PORT");
// var dbName = Environment.GetEnvironmentVariable("DB_NAME");
// var user = Environment.GetEnvironmentVariable("DB_USER");
// var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

// var connectionString = $"Server={host};Port={port};Database={dbName};User={user};Password={password};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(12, 1, 2)) 
    )
);

builder.Services.AddScoped<ExpenseRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ExpenseService>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseStaticFiles();
app.UseAuthorization();

app.MapStaticAssets();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // creates tables automatically
}

app.Run();
