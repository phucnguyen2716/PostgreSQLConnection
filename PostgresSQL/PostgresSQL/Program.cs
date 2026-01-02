using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostgresSQL.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Database - PostgreSQL
// =======================
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
// Show detailed DB errors (dev only)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// =======================
// Identity
// =======================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // dev cho tiện
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// =======================
// MVC
// =======================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =======================
// Middleware
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // auto apply migrations
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
