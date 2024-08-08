using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Gallery.Data;
using Gallery.Models;

namespace Gallery.Pages;

public class ImagesModel(GalleryDbContext dbContext) : PageModel
{
    private readonly GalleryDbContext _dbContext = dbContext;

    public List<UploadedImage> Images { get; private set; } = [];

    public async void OnGet()
    {
        Images = await _dbContext.UploadedImages.ToListAsync();
    }
}