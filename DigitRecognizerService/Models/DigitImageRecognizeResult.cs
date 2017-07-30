using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    public class DigitImageRecognizeResult
    {
        /// <summary>
        /// Digit recognized, garbage if ErrorCode != 0
        /// </summary>
        [JsonProperty("digit")]
        public char Digit;

        /// <summary>
        /// Error Message
        /// </summary>
        [JsonProperty("error_msg")]
        public string ErrorMsg;

        /// <summary>
        /// Confidence Value
        /// </summary>
        [JsonProperty("confidence")]
        public double Confidence;

        /// <summary>
        /// Error Code
        /// </summary>
        [JsonProperty("error_code")]
        public ErrorCode ErrorCode;

        /// <summary>
        /// Generates error message based on error code.
        /// </summary>
        internal void GenerateErrorMessage()
        {
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                switch (ErrorCode)
                {
                    case (ErrorCode.APPLICATION_ERROR):
                        ErrorMsg = "An application error has occurred.";
                        break;
                    case (ErrorCode.BAD_FILE_COUNT_PER_REQUEST):
                        ErrorMsg = "One and only one file should be uploaded per post request.";
                        break;
                    case (ErrorCode.IMAGE_BAD_FORMAT):
                        ErrorMsg = "Image format is not supported.";
                        break;
                    case (ErrorCode.IMAGE_SIZE_EXCEED_LIMIT):
                        ErrorMsg = "Image size exceeded maximum limit. It should be between 1";
                        break;
                    case (ErrorCode.RECOGNITION_FAILURE):
                        ErrorMsg = "Could not recognize digit in image.";
                        break;
                    case (ErrorCode.SUCCESS):
                        ErrorMsg = "Success !";
                        break;
                    default:
                        ErrorMsg = "An unknown error has occurred";
                        break;
                }
            }
        }
    }

}