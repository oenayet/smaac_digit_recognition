using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    /// <summary>
    /// Interface abstracting file system operations performed.
    /// </summary>
    public interface IFileSystemUtility
    {
        string MapPathOnWebServer(string virtualPath);

        void DeleteFile(string filePath);

        void CreateDirectory(string directoryPath);

        bool DirectoryExists(string directoryPath);
    }
}