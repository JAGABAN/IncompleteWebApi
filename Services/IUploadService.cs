namespace MyPersonalProject.Services
{
    public interface IUploadService
    {
        Task<Dictionary<string, string>> UploadFileAsync(IFormFile file, int id);
    }
}
