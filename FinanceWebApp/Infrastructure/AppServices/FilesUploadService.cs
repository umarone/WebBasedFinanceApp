using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace MudasirRehmanAlp.Infrastructure.AppServices
{
    public class FilesUploadService
    {
        
        public string UploadFile(string file, Stream stream, string folder)
        {

            var _hostingEnvironment = HttpContext.Current.Server.MapPath("~/Content/wwwroot");

            var uploads = Path.Combine(_hostingEnvironment, folder);
            var exists = Directory.Exists(uploads);
            if (!exists)
            {
                Directory.CreateDirectory(uploads);

            }
            var fileName = GetFileName(file);
            var filePath = Path.Combine(uploads, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
            string savePathToDatabase = Path.Combine(folder, fileName);
            return savePathToDatabase;//filePath
        }

        public string UploadImages(string file, Stream stream, string folder,string extension)
        {
            var _hostingEnvironment = HttpContext.Current.Server.MapPath("~/Content/wwwroot");
            var uploads = Path.Combine(_hostingEnvironment, folder);
            var exists = Directory.Exists(uploads);
            if (!exists)
            {
                Directory.CreateDirectory(uploads);

            }
            var fileName = GetImageName(file, extension);
            var filePath = Path.Combine(uploads, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
            string savePathToDatabase = Path.Combine(folder, fileName);
            return savePathToDatabase;//filePath
        }
        public string GetFileName(string file)
        {
            return file + DateTime.Now.Ticks + ".pdf";
        }
        public string GetImageNameWithExtension(string file)
        {
            return file + DateTime.Now.Ticks + ".jpg";
        }
        public string GetImageName(string file, string extension)
        {
            return file + DateTime.Now.Ticks+ extension;
        }
    }
}