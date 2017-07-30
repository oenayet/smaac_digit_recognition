using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    public class FileSystemUtility : IFileSystemUtility
    {
        public void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public string MapPathOnWebServer(string virtualPath)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
        }
    }
}