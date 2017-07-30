using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    public class DigitImageFile : IDigitImageFile
    {
        HttpPostedFile hpf;
        public DigitImageFile (HttpPostedFile hpf)
        {
            this.hpf = hpf;
        }

        public int ContentLength
        {
            get
            {
                return hpf.ContentLength;
            }
        }

        public string ContentType
        {
            get
            {
                return hpf.ContentType;
            }
        }

        public string FileName
        {
            get
            {
                return hpf.FileName;
            }
        }

        public void SaveAs(string strFilePath)
        {
            hpf.SaveAs(strFilePath);
        }
    }
}