using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace AdminDashboard.Helpers
{
    public class ImageSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1- Get Path Of Folder 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName);
           
            // 2- Get File Name  and make it Unique
            string FileName = Guid.NewGuid() + file.FileName;
            
            // 3- Get File Path
            string FilePath = Path.Combine(FolderPath, FileName);
            
            // 4- Save File As Streams
            using var fs = new FileStream(FilePath, FileMode.Create);
          
            // 5- Copy file into Stream
            file.CopyTo(fs);
            
            // 6- Return FileName 
            return Path.Combine("images\\Products", FileName);
        }

        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName, FileName);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
