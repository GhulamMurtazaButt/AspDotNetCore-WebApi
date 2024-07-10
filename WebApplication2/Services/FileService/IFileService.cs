using WebApplication2.Models;

namespace WebApplication2.Services.FileService
{
    public interface IFileService
    {
       public string AddFile(IFormFile image);
       public string UpdateFile(string oldFileName, IFormFile newFile);
        public bool DeleteFile(string oldFileName);
    }
}
