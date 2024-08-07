using Gallery.Models;
using Microsoft.EntityFrameworkCore;

namespace Gallery.Data;

public class GalleryDbContext(DbContextOptions<GalleryDbContext> options) : DbContext(options)
{
    public DbSet<UploadedImage> UploadedImages { get; set; }
}