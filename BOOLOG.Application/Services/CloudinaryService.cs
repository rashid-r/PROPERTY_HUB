using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BOOLOG.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudname = configuration["CloudinarySettings = CloudName"];
            var apikey = configuration["CloudinarySettings = ApiKey"];
            var apisecret = configuration["CloudinarySettings = ApiSecret"];

            var account = new Account(cloudname, apikey, apisecret);

            _cloudinary = new Cloudinary(account);
        }
        public async Task<ApiResponse<string>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ApiResponse<string>(406, "File is null or empty");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                Console.WriteLine($"Cloudinary Upload Result - Status: {uploadResult.StatusCode}");
                Console.WriteLine($"Cloudinary Upload Result - SecureUrl: {uploadResult.SecureUrl}");
                Console.WriteLine($"Cloudinary Upload Result - Error: {uploadResult.Error?.Message}");

                if (uploadResult.Error != null)
                    return new ApiResponse<string>(400, uploadResult.Error.Message);

                var url = uploadResult.SecureUrl?.ToString();
                Console.WriteLine($"Final URL to return: {url}");

                return new ApiResponse<string>(200, "Upload successful", url);
            }
        }

        public async Task<ApiResponse<string>> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ApiResponse<string>(404, "File is null or empty");

            // Validate file type
            var supportedFormats = new[] { ".mp4", ".webm", ".mov", ".avi" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!supportedFormats.Contains(fileExtension))
                return new ApiResponse<string>(400, $"Unsupported video format: {fileExtension}");

            // Validate file size (e.g., max 100 MB)
            const long maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes
            if (file.Length > maxFileSize)
                return new ApiResponse<string>(406, "File size exceeds 100 MB limit");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fit") // Use "fit" for videos
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Log details for debugging
                Console.WriteLine($"Cloudinary Video Upload Result - Status: {uploadResult.StatusCode}");
                Console.WriteLine($"Cloudinary Video Upload Result - SecureUrl: {uploadResult.SecureUrl}");
                Console.WriteLine($"Cloudinary Video Upload Result - Error: {uploadResult.Error?.Message}");
                Console.WriteLine($"Cloudinary Video Upload Result - Full JSON: {uploadResult.JsonObj}");

                if (uploadResult.Error != null)
                    return new ApiResponse<string>(400, uploadResult.Error.Message);

                var url = uploadResult.SecureUrl?.ToString();
                if (string.IsNullOrEmpty(url))
                    return new ApiResponse<string>(406, "Video upload failed: No URL returned");

                return new ApiResponse<string>(200, "Video upload successful", url);
            }
        }
    }
}
