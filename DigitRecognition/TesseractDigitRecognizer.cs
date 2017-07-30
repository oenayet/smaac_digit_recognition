using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace DigitRecognition
{
    /// <summary>
    /// Class uses Tesseract open source OCR engine.
    /// </summary>
    public class TesseractDigitRecognizer : IDigitRecognizer
    {
        /// <summary>
        /// Path to tesseract language info directory
        /// </summary>
        string tessdataPath;
        /// <summary>
        /// Path to tesseract config file
        /// </summary>
        string tessConfig;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tessdataPath">Path to tesseract language info directory</param>
        /// <param name="tessConfig">Path to tesseract config file</param>
        public TesseractDigitRecognizer(string tessdataPath,string tessConfig)
        {
            this.tessdataPath = tessdataPath;
            this.tessConfig = tessConfig;
        }

        /// <summary>
        /// Uses tesseract engine to detect digit in image.
        /// </summary>
        /// <param name="image">path to image files</param>
        /// <out param name="meanConf">mean confidence (value between 0 & 1)</param>
        /// <returns></returns>
        public char? RecognizeDigit(string image,out float meanConf)
        {
            var ENGLISH_LANGUAGE = @"eng";
            meanConf = 0;

            using (var ocrEngine = new TesseractEngine(tessdataPath, ENGLISH_LANGUAGE, EngineMode.Default, tessConfig))
            {
                using (var imageWithText = Pix.LoadFromFile(image))
                {
                    //Single character
                    PageSegMode mode = PageSegMode.SingleChar;

                    using (var page = ocrEngine.Process(imageWithText, mode))
                    {
                        var text = page.GetText();

                        meanConf =  page.GetMeanConfidence();

                        //Trim result to remove any whie space
                        text = text.Trim();

                        if (!string.IsNullOrEmpty(text))
                        {
                            //Get the first character as the result.
                            return text[0];
                        }
                    }
                }
            }

            //Return null on failure
            return null;
        }
    }
}
