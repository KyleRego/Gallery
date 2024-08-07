using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Pages;

public class IndexModel : PageModel
{
    public List<UploadedImage> Images { get; set; } = [];

    private readonly ILogger<IndexModel> _logger;
    private readonly GalleryDbContext _dbContext;
    public IndexModel(ILogger<IndexModel> logger, GalleryDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        Images = [.. _dbContext.UploadedImages];
    }
}
