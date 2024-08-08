using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Gallery.Pages;

public class ImageFormData
{
    public required string Description { get; set; }
}

public class ImageModel(GalleryDbContext dbContext, IConfiguration config) : PageModel
{
    private readonly GalleryDbContext _dbContext = dbContext;
    private readonly IConfiguration _config = config;

    public UploadedImage? Image { get; private set; }

    public string? Result { get; private set; }

    [BindProperty]
    public ImageFormData? FormData { get; set; }

    public async Task<IActionResult> OnGet(string imageId)
    {
        Image = await _dbContext.UploadedImages.FirstOrDefaultAsync(im => im.Id == imageId);

        if (Image == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string imageId)
    {
        Image = await _dbContext.UploadedImages.FirstOrDefaultAsync(im => im.Id == imageId);

        if (Image == null) return NotFound();

        Image.Description = FormData!.Description;

        await _dbContext.SaveChangesAsync();

        Result = "Update successful";
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string imageId)
    {
        Image = await _dbContext.UploadedImages.FirstOrDefaultAsync(im => im.Id == imageId);

        if (Image == null) return NotFound();

        string uploadedFilesPath = _config.GetValue<string>("StoredFilesPath")!;

        string pathToDeleteFile = Image.PathToFile(uploadedFilesPath);

        if (System.IO.File.Exists(pathToDeleteFile))
        {
            System.IO.File.Delete(pathToDeleteFile);
        }

        _dbContext.UploadedImages.Remove(Image);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("/Images");
    }
}