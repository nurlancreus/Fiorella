﻿using Fiorella.App.Models;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Fiorella.App.Extensions
{
    public static class FileUpload
    {
        public static async Task<string> SaveFileAsync(this IFormFile formFile, string root, string path)
        {
            string fileName = Guid.NewGuid().ToString() + formFile.FileName;
            string fullPath = Path.Combine(root, path, fileName);

            using (FileStream fileStream = new(fullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public static async Task<ICollection<string>> SaveMultipleFileAsync(this ICollection<IFormFile> multipleFormFiles, string root, string path)
        {
            ICollection<string> fileNames = [];
            foreach (var file in multipleFormFiles)
            {

                if (file.Length > 0)
                {
                    string fileName = await file.SaveFileAsync(root, path);
                    fileNames.Add(fileName);
                }
            }

            return fileNames;
        }

        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains("image");
        }

        public static bool IsSizeOk(this IFormFile formFile, int mb)
        {
            // Convert file length from bytes to megabytes
            double fileSizeInMB = formFile.Length / (1024.0 * 1024.0);
            return fileSizeInMB <= mb;
        }

        public static bool RestrictExtension(this IFormFile formFile, string[]? permittedExtensions = null)
        {
            permittedExtensions ??= [".jpg", ".png", ".gif"];

            string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && permittedExtensions.Contains(extension);

        }

        public static bool RestrictMimeTypes(this IFormFile formFile, string[]? permittedMimeTypes = null)
        {
            permittedMimeTypes ??= ["image/jpeg", "image/png", "image/gif"];

            string mimeType = formFile.ContentType;
            return permittedMimeTypes.Contains(mimeType);

        }
    }
}
