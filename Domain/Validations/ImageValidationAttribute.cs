using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Validations;

public class ImageValidationAttribute(int maxSizeInMb = 2) : ValidationAttribute
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private readonly string[] _allowedTypes = ["image/jpeg", "image/png", "image/gif"];
    private readonly int _maxSizeInBytes = maxSizeInMb * 1024 * 1024;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        var file = value as IFormFile;

        if (file == null)
            return new ValidationResult("Invalid file");

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            return new ValidationResult("Only .jpg, .jpeg, .png, .gif allowed");

        if (!_allowedTypes.Contains(file.ContentType))
            return new ValidationResult("Invalid image type");

        if (file.Length > _maxSizeInBytes)
            return new ValidationResult($"Max size: {_maxSizeInBytes / (1024 * 1024)} MB");

        using (var stream = file.OpenReadStream())
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);

            if (!IsValidImageSignature(buffer))
                return new ValidationResult("File is not a real image");
        }

        return ValidationResult.Success;
    }

    private bool IsValidImageSignature(byte[] bytes)
    {
        if (bytes[0] == 0xFF && bytes[1] == 0xD8)
            return true;

        if (bytes[0] == 0x89 && bytes[1] == 0x50 &&
            bytes[2] == 0x4E && bytes[3] == 0x47)
            return true;

        if (bytes[0] == 0x47 && bytes[1] == 0x49 &&
            bytes[2] == 0x46)
            return true;

        return false;
    }
}