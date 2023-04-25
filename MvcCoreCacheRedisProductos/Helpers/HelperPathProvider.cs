namespace MvcCoreCacheRedisProductos.Helpers
{
    public enum Folders { Images, Documents, Temporal }
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            else if (folder == Folders.Documents)
            {
                carpeta = "documents";
            }
            else if (folder == Folders.Temporal)
            {
                carpeta = "temp";
            }
            string path = Path.Combine(this.hostEnvironment.WebRootPath
                , carpeta, fileName);
            return path;
        }
    }


}
