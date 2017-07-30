using DigitRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    /// <summary>
    /// Class responsible of processing image files and attempting to detect the digits.
    /// </summary>
    public class DigitRecognizer
    {
        #region Constants
        /// <summary>
        /// Maximum image size to be processed
        /// </summary>
        const int BYTES_IN_1_MB = 1048576;
        /// <summary>
        /// PNG Content type
        /// </summary>
        const string CONTENT_TYPE_PNG = "image/png";
        /// <summary>
        /// JPEG content type
        /// </summary>
        const string CONTENT_TYPE_JPEG = "image/jpeg";
        /// <summary>
        /// Tesseract language info directory
        /// </summary>
        const string TESS_DATA_VIRTUAL_PATH = "~/tessdata/";
        /// <summary>
        /// Tesseract config file
        /// </summary>
        const string TESS_CONFIG_VIRTUAL_PATH = "~/tessdata/configs/digits";
        /// <summary>
        /// Directory where files are temporarily saved.
        /// </summary>
        const string DOWNLOAD_VIRTUAL_PATH = "~/locker/";
        #endregion

        #region Private Data
        /// <summary>
        /// Supported content types.
        /// </summary>
        List<string> SupportedContentTypes = new List<string> { CONTENT_TYPE_PNG, CONTENT_TYPE_JPEG };

        /// <summary>
        /// File System Utility interface
        /// </summary>
        IFileSystemUtility fileSystemUtility;
        /// <summary>
        /// Ensemble Digit Recognizer Interface
        /// </summary>
        IEnsembleDigitRecognizer ensembleDigitRecognizer;
        #endregion

        /// <summary>
        /// Default Constructor, initializes Ensemble digit recognizer
        /// </summary>
        public DigitRecognizer()
        {
            this.fileSystemUtility = new FileSystemUtility();
            string strTessDataPath = fileSystemUtility.MapPathOnWebServer(TESS_DATA_VIRTUAL_PATH);
            string strTessConfig = fileSystemUtility.MapPathOnWebServer(TESS_CONFIG_VIRTUAL_PATH);
            this.ensembleDigitRecognizer = new EnsembleDigitRecognizer(strTessDataPath, strTessConfig);
        }

        /// <summary>
        /// Test Constructor, takes as parameters file system utility and the ensemble digit recognizer.
        /// </summary>
        /// <param name="fileSystemUtility"></param>
        /// <param name="ensembleDigitRecognizer"></param>
        public DigitRecognizer(IFileSystemUtility fileSystemUtility, IEnsembleDigitRecognizer ensembleDigitRecognizer)
        {
            this.fileSystemUtility = fileSystemUtility;
            this.ensembleDigitRecognizer = ensembleDigitRecognizer;
        }

        /// <summary>
        /// Process multiple image files and returns the digit classification of each.
        /// Currently only 1 image is supported at a time.
        /// </summary>
        /// <param name="digitImages"></param>
        /// <returns></returns>
        public DigitImageRecognizeResult ProcessImageFiles(List<IDigitImageFile> digitImages)
        {
            DigitImageRecognizeResult result = new Models.DigitImageRecognizeResult();

            //One & only 1 image file should be passed
            //TODO: Support multiple images
            if (digitImages != null && digitImages.Count == 1)
            {
                IDigitImageFile digitImageFile = digitImages[0];

                //Make sure file content type is supported
                if (SupportedContentTypes.Contains(digitImageFile.ContentType))
                {
                    //Make sure file size does not exceed maximum size
                    if (digitImageFile.ContentLength <= BYTES_IN_1_MB && digitImageFile.ContentLength > 0)
                    {
                        result = RecoginizeDigit(digitImageFile);
                    }
                    else
                    {
                        result.ErrorCode = ErrorCode.IMAGE_SIZE_EXCEED_LIMIT;
                    }
                }
                else
                {
                    result.ErrorCode = ErrorCode.IMAGE_BAD_FORMAT;
                }

            }
            else
            {
                result.ErrorCode = ErrorCode.BAD_FILE_COUNT_PER_REQUEST;
            }

            return result;
        }

        /// <summary>
        /// Saves file to disk, calls ensemble classifier to detect digit, then deletes the file.
        /// </summary>
        /// <param name="imageFile">Image File</param>
        /// <returns></returns>
        private DigitImageRecognizeResult RecoginizeDigit(IDigitImageFile imageFile)
        {
            string downloadDirectory = "";
            //Get download directory on web server.
            downloadDirectory = fileSystemUtility.MapPathOnWebServer(DOWNLOAD_VIRTUAL_PATH);
            
            //Construct image file name with a random GUID to avoid conflicts with multiple concurrent requests with same file name.
            DigitImageRecognizeResult result = new DigitImageRecognizeResult();
            string fileName = Path.GetFileName(imageFile.FileName) + "_" + Guid.NewGuid().ToString();
            
            //Construct file path
            string filePath = downloadDirectory + fileName;
            
            //Create download directory if not already found.
            if ( !fileSystemUtility.DirectoryExists(downloadDirectory))
            {
                fileSystemUtility.CreateDirectory(downloadDirectory);
            }
            
            //Save image file to disk, to be processed.
            imageFile.SaveAs(filePath);

            //Use the ensemble recognizer to recognize the digit.
            float confidence;
            char? ch = ensembleDigitRecognizer.RecognizeDigit(filePath, out confidence);
            if (ch == null)
            {
                //Could not recognize digit !
                result.ErrorCode = ErrorCode.RECOGNITION_FAILURE;
            }
            else
            {
                //Set Resultant values
                result.Digit = ch.Value;
                result.Confidence = confidence;
                result.ErrorCode = ErrorCode.SUCCESS;
            }

            //Delete the file from disk
            fileSystemUtility.DeleteFile(filePath);
            
            return result;
        }


    
    }
}