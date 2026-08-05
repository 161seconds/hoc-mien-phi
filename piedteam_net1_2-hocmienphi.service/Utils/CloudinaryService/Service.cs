using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace piedteam_net1_2_hocmienphi.service.Utils.CloudinaryService;

public class Service : MediaService.IService
{
    private readonly Cloudinary _cloudinary;
    private readonly CloudinaryOptions _cloudinaryOptions = new();

    public Service(IConfiguration configuration)
    {
        configuration.GetSection(nameof(CloudinaryOptions)).Bind(_cloudinaryOptions);
        _cloudinary = new Cloudinary(new Account(_cloudinaryOptions.CloudName, 
                                                    _cloudinaryOptions.ApiKey, 
                                                    _cloudinaryOptions.ApiSecret));
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file.Length == 0 || file == null) 
            throw new ArgumentNullException(nameof(file));
        if(!IsImageFile(file)) throw new Exception("Invalid image file");
        if(!IsValidImageSize(file)) throw new Exception("Invalid image size");
        //upload len cloudinary
        await using var stream = file.OpenReadStream();
        // cloudinary cần nhung cái này
        // mở cổng stream
        
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream)
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        
        if(uploadResult.Error != null) throw new Exception(uploadResult.Error.ToString());
        
        return uploadResult.SecureUrl.ToString();
    }
    
    private bool IsValidImageSize(IFormFile file)
    {
        const int maxAllowedFileSize = 5;
        long maxBytes = maxAllowedFileSize * 1024 * 1024;

        if (file.Length > maxBytes)
        {
            throw new Exception("File size is too large");
        }
        
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(fileExtension);
    }

    private bool IsImageFile(IFormFile file)
    {
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(fileExtension);
    }
}