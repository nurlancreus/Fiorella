namespace Fiorella.App.Helpers
{
    public class Helper
    {

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
