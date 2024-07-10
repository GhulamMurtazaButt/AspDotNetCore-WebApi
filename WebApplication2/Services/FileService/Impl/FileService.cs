using WebApplication2.Utilities;

namespace WebApplication2.Services.FileService.Impl
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string AddFile(IFormFile image)
        {
            try
            {
                string folder_name = _configuration.GetSection(Constants.imagePath).Value;
                string filePath = Path.Combine(folder_name, image.FileName);
                image.CopyTo(new FileStream(filePath, FileMode.Create));
                string imageUrl = _configuration.GetSection(Constants.imageUrl).Value;
                string relativePath = Path.Combine(imageUrl, image.FileName);
                return relativePath;
            }
            catch(Exception exp)
            {
                return exp.Message;
            }

        }
        public string UpdateFile(string oldFileName, IFormFile newFile)
        {         
          
            if (DeleteFile(oldFileName))
            {
                return AddFile(newFile);
            }
            else
            {
                return MessageStrings.noimageAttach;
            }
        }

        public bool DeleteFile(string oldFileName)
        {
            string folderName = _configuration.GetSection(Constants.imagePath).Value;
            string oldFilePath = Path.Combine(folderName, oldFileName);
            FileInfo file = new FileInfo(oldFilePath);
            if (file.Exists)
            {
                file.Delete();
                
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
