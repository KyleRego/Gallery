using System.ComponentModel.DataAnnotations;

namespace Gallery.Models;

public enum ContentTypeEnum
{
    Jpg,
    Png
}

public class UploadedImage
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required ContentTypeEnum ContentType { get; set; }

    [Required]
    public required string OriginalName { get; set; }

    [Required]
    public required string Description { get; set; } = "";

    [Required]
    public required string RandomGeneratedName { get; set; }
}