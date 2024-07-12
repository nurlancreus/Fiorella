namespace Fiorella.App.Helpers
{
    public class Helper
    {
        public static bool IsImage(IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool IsSizeOk(IFormFile file, int mb)
        {
            // Convert file length from bytes to megabytes
            double fileSizeInMB = file.Length / (1024.0 * 1024.0);
            return fileSizeInMB <= mb;
        }

        public static void RemoveImage(string root, string path, string fileName)
        {
            string fullPath = Path.Combine(root, path, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
