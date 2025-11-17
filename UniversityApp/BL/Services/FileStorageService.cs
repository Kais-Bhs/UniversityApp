// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog;

namespace BL.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _uploadPath;

        public FileStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadPath = _configuration["FileStorage:UploadPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
                _logger.Info("Created upload directory at {Path}", _uploadPath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }

                var folderPath = Path.Combine(_uploadPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativeUrl = Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
                _logger.Info("File saved successfully: {FileName} -> {RelativeUrl}", file.FileName, relativeUrl);

                return relativeUrl;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to save file: {FileName}", file?.FileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                {
                    return false;
                }

                var filePath = Path.Combine(_uploadPath, fileUrl.Replace("/", "\\"));

                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                    _logger.Info("File deleted successfully: {FileUrl}", fileUrl);
                    return true;
                }

                _logger.Warn("File not found for deletion: {FileUrl}", fileUrl);
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete file: {FileUrl}", fileUrl);
                return false;
            }
        }

        public bool IsValidFileType(IFormFile file, string[] allowedExtensions)
        {
            if (file == null)
            {
                return false;
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }

        public bool IsValidFileSize(IFormFile file, long maxSizeInBytes)
        {
            if (file == null)
            {
                return false;
            }

            return file.Length <= maxSizeInBytes;
        }
    }
}