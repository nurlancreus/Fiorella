using Fiorella.App.Models;

namespace Fiorella.App.Areas.Admin.Extensions
{
    public static class AdminAreaExtensions
    {
        public static async Task<string> GetFileNameAsync(this IFormFile formFile, string root, string path)
        {
            string fileName = Guid.NewGuid().ToString() + formFile.FileName;
            string fullPath = Path.Combine(root, path, fileName);

            using (FileStream fileStream = new(fullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
