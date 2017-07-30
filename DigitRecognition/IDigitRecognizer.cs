using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognition
{
    public interface IDigitRecognizer
    {
        char? RecognizeDigit(string filePath, out float confidence);
    }
}
