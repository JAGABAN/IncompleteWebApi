using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MyPersonalProject.Data;

namespace MyPersonalProject.Services
{
    public class UploadService : IUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public UploadService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
       



        public async Task<Dictionary<string, string>> UploadFileAsync(IFormFile file, int id)
        {
            var findContact = await _dbContext.Contacts.FindAsync(id);
            var account = new Account
            {
                ApiKey = _configuration["Cloudinary:ApiKey"],
                Cloud = _configuration["Cloudinary:CloudName"],
                ApiSecret = _configuration["Cloudinary:ApiSecret"]
            };

            var cloudinary = new Cloudinary(account);

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be empty.");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                throw new ArgumentException("File must be an image.");
            }

            if (file.Length > (1024 * 1024 * 2))
            {
                throw new ArgumentException("File size cannot exceed 2MB.");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
            };

            try
            {
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                
                var result = new Dictionary<string, string>()
        {
            { "PublicId", uploadResult.PublicId },
            { "Url", uploadResult.Url.ToString() }
        };
                if (findContact != null)
                {
                    findContact.ImageUrl = uploadResult.Url.ToString();
                    _dbContext.Update(findContact);
                    await _dbContext.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file to Cloudinary.", ex);
            }
        }

    }
}
