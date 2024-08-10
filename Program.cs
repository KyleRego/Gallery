using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Gallery.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Image");
    options.Conventions.AuthorizePage("/Images");
    options.Conventions.AuthorizePage("/UploadImage");
});

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    string dbConString = builder.Configuration.GetConnectionString("GalleryDatabase")
        ?? throw new InvalidOperationException("database connection string missing");

    builder.Services.AddDbContext<GalleryDbContext>(options => options.UseSqlite(dbConString));
}
else if (builder.Environment.IsProduction())
{
    string dbConString = builder.Configuration["DbConnectionString"]
            ?? throw new InvalidOperationException("db connection string missing");

    builder.Services.AddDbContext<GalleryDbContext>(options => options.UseSqlite(dbConString));
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<GalleryDbContext>();

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
