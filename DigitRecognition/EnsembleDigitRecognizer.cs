using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognition
{
    /// <summary>
    /// Class to initialize multiple classifiers and use them as an ensemble classifier to get the best answer to the digit recognition.
    /// It chooses the result based on the maximum confidence provided by its recognizer.
    /// TODO: Support more classifiers (recognizers) other than Tesseract.
    /// </summary>
    public class EnsembleDigitRecognizer : IEnsembleDigitRecognizer
    {
        /// <summary>
        /// List of recognizers
        /// </summary>
        List<IDigitRecognizer> digitRecognizers;

        /// <summary>
        /// Constructor, initialize all recognizers
        /// </summary>
        /// <param name="digitRecognizers"></param>
        public EnsembleDigitRecognizer(List<IDigitRecognizer> digitRecognizers)
        {
            this.digitRecognizers = digitRecognizers;
        }
        /// <summary>
        /// Default Constructor, automatically adds the tesseract recognizer to the list.
        /// TODO: Support more classifiers (recognizers) other than Tesseract.
        /// </summary>
        /// <param name="tessdataPath">Path to tesseract language info directory</param>
        /// <param name="tessConfig">Path to tesseract config file</param>
        public EnsembleDigitRecognizer(string tessdataPath, string tessConfig)
        {
            digitRecognizers = new List<IDigitRecognizer>();

            //Initialize the tesseract recognizer
            TesseractDigitRecognizer tesseractDigitClassifier = new TesseractDigitRecognizer(tessdataPath, tessConfig);
            digitRecognizers.Add(tesseractDigitClassifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="confidence"></param>
        /// <returns></returns>
        public char? RecognizeDigit(string filePath,out float confidence)
        {
            confidence = 0;
            float meanConfidence;
            char ?chosenCh = null;
           
            foreach (IDigitRecognizer recognizer in digitRecognizers)
            {
                char? ch = recognizer.RecognizeDigit(filePath, out meanConfidence);
                if (ch != null)
                {
                    //Choose result from recognizer with maximum confidence.
                    if (meanConfidence > confidence)
                    {
                        confidence = meanConfidence;
                        chosenCh = ch;
                    }
                }
            }

            return chosenCh;
        }
    }
}
