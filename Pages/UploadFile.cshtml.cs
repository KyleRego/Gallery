using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
// using SampleApp.Utilities;

namespace Gallery.Pages;

public class UploadFileFormData
{
    public IFormFile? FormFile { get; set; }

    public string? Description { get; set; }
}

public class UploadFileModel : PageModel
{
    private readonly long _fileSizeLimit;
    private readonly string[] _permittedExtensions = [".png", ".jpg"];
    private readonly string _uploadedFilesPath;
    private readonly GalleryDbContext _dbContext;
    public UploadFileModel(IConfiguration config, GalleryDbContext dbContext)
    {
        _fileSizeLimit = config.GetValue<long>("FileSizeLimit");

        _uploadedFilesPath = config.GetValue<string>("StoredFilesPath")
            ?? throw new ApplicationException("no stored files path");

        _dbContext = dbContext;
    }

    [BindProperty]
    public UploadFileFormData FormData { get; set; }

    public string Result { get; private set; } = "";

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostUploadAsync()
    {
        if (FormData == null || FormData.FormFile == null)
        {
            Result = "No file was uploaded";
            return Page();
        }

        IFormFile formFile = FormData.FormFile;

        if (formFile.Length == 0)
        {
            Result = "File was empty";
            return Page();
        }
        else if (formFile.Length > _fileSizeLimit)
        {
            Result = "File was too big";
            return Page();
        }
        
        string fileName = formFile.FileName;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Result = "File name was missing";
            return Page();
        }

        string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) || !_permittedExtensions.Contains(fileExtension))
        {
            Result = "Not a valid file extension";
            return Page();
        }

        using var memoryStream = new MemoryStream();

        await formFile.CopyToAsync(memoryStream);

        if (memoryStream.Length == 0)
        {
            Result = "File content was empty";
            return Page();
        }
        
        byte[] formFileContent = memoryStream.ToArray();

        string trustedFileNameForFileStorage = Path.GetRandomFileName();
        string filePath = Path.Combine(_uploadedFilesPath, trustedFileNameForFileStorage);

        using var fileStream = System.IO.File.Create(filePath);
        
        await fileStream.WriteAsync(formFileContent);

        var contentType = (fileExtension == ".png") ? ContentTypeEnum.Png : ContentTypeEnum.Jpg;

        UploadedImage fileRecord = new()
        {
            OriginalName = formFile.FileName,
            RandomGeneratedName = trustedFileNameForFileStorage,
            ContentType = contentType,
            Description = FormData.Description ?? ""
        };

        _dbContext.UploadedImages.Add(fileRecord);

        await _dbContext.SaveChangesAsync();
        
        Result = "File successfully uploaded";
        return Page();
    }
}
