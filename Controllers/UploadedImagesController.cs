using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers;

[ApiController, Route("api/[controller]")]
public class UploadedImagesController(ILogger<UploadedImagesController> logger, 
                                    IConfiguration config,
                                    GalleryDbContext dbContext) : ControllerBase
{
    private readonly ILogger<UploadedImagesController> _logger = logger;
    private readonly IConfiguration _config = config;
    private readonly GalleryDbContext _dbContext = dbContext;

    [HttpGet("{imageId}")]
    public IActionResult Get(string imageId)
    {
        UploadedImage? image = _dbContext.UploadedImages.FirstOrDefault(file => file.Id == imageId);

        if (image == null) return NotFound();

        string uploadedFilesPath = _config.GetValue<string>("StoredFilesPath")!;

        string uploadedFilePath = image.PathToFile(uploadedFilesPath);

        if (!System.IO.File.Exists(uploadedFilePath)) return NotFound();

        return PhysicalFile(uploadedFilePath, "image/png");
    }
}