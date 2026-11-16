using Microsoft.EntityFrameworkCore;
using StudyApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
}
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Teachers}/{action=Index}/{id?}"
);
app.Run();
