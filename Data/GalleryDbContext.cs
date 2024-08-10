using Gallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gallery.Data;

public class GalleryDbContext(DbContextOptions<GalleryDbContext> options) : IdentityDbContext(options)
{
    public DbSet<UploadedImage> UploadedImages { get; set; }
}