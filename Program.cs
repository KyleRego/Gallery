using Gallery.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    string dbConString = builder.Configuration.GetConnectionString("GalleryDatabase")
        ?? throw new InvalidOperationException("database connection string missing");

    builder.Services.AddDbContext<GalleryDbContext>(options => options.UseSqlite(dbConString));
}
else if (builder.Environment.IsProduction())
{
    string dbConString = builder.Configuration["DbConnectionString"];

    builder.Services.AddDbContext<GalleryDbContext>(options => options.UseSqlite(dbConString));
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GalleryDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
