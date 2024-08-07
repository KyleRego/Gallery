using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers;

[ApiController, Route("api/[controller]")]
public class UploadedFilesController(ILogger<UploadedFilesController> logger, 
                                    IConfiguration config,
                                    GalleryDbContext dbContext) : ControllerBase
{
    private readonly ILogger<UploadedFilesController> _logger = logger;
    private readonly IConfiguration _config = config;
    private readonly GalleryDbContext _dbContext = dbContext;

    [HttpGet("{fileId}")]
    public IActionResult Get(string fileId)
    {
        UploadedImage? file = _dbContext.UploadedImages.FirstOrDefault(file => file.Id == fileId);

        if (file == null) return NotFound();

        string uploadedFilesPath = _config.GetValue<string>("StoredFilesPath")
            ?? throw new ApplicationException("no stored files path");

        string relativePath = Path.Combine(uploadedFilesPath, file.RandomGeneratedName!);

        string uploadedFilePath = Path.GetFullPath(relativePath);

        if (!System.IO.File.Exists(uploadedFilePath)) return NotFound();

        return PhysicalFile(uploadedFilePath, "image/png");
    }
}