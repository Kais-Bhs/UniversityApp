// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Microsoft.AspNetCore.Http;

namespace BL.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string fileUrl);
        bool IsValidFileType(IFormFile file, string[] allowedExtensions);
        bool IsValidFileSize(IFormFile file, long maxSizeInBytes);
    }
}