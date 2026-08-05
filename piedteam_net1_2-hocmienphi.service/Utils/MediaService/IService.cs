using Microsoft.AspNetCore.Http;

namespace piedteam_net1_2_hocmienphi.service.MediaService;

public interface IService
{
    public Task<string> UploadImageAsync(IFormFile file);
    
}