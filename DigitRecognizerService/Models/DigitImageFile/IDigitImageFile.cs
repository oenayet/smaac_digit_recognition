using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    /// <summary>
    /// Interface abstracting image file uploaded
    /// </summary>
    public interface IDigitImageFile
    {
        string ContentType { get; }
        int ContentLength { get; }
        string FileName { get;  }
        void SaveAs(string strFilePath);
    }
}