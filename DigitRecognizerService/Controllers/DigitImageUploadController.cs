using DigitRecognition;
using DigitRecognizerService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace DigitRecognizerService.Controllers
{

    /// <summary>
    /// API Controller for Digit recognition from files.
    /// </summary>
    public class DigitImageUploadController : ApiController
    {
        /// <summary>
        /// GET handling for api/DigitImageUpload. Just to make sure controller is working !
        /// </summary>
        /// <returns>"Controller Ready !"</returns>
        [HttpGet]
        [Route("api/DigitImageUpload")]
        public string Get()
        {
            return "Controller Ready !";
        }

        /// <summary>
        /// POST handling for api/DigitImageUpload. 
        /// Reads file from POST body, performs digit recognition and returns result as JSON response.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DigitImageUpload")]
        public DigitImageRecognizeResult DigitImageRecognize()
        {
            DigitImageRecognizeResult result = new DigitImageRecognizeResult();
            try
            {
                HttpFileCollection hfc = HttpContext.Current.Request.Files;

                //Convert each HttpFile into a IDigitImageFile object
                List<IDigitImageFile> digitImages = new List<IDigitImageFile>();
                for (int iCnt = 0; iCnt < hfc.Count ; iCnt++)
                {
                    HttpPostedFile hpf = hfc[iCnt];

                    DigitImageFile imageFile = new DigitImageFile(hpf);
                    digitImages.Add(imageFile);
                }

                //Process the image files passed in.
                DigitRecognizer digitRecognizer = new DigitRecognizer();
                result = digitRecognizer.ProcessImageFiles(digitImages);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"An error has occurred while detecting digit. {ex.Message}");
                result.ErrorCode = ErrorCode.APPLICATION_ERROR;
                result.ErrorMsg = $"An application error has occurred: {ex.Message}";
            }

            //Generate textual error message
            result.GenerateErrorMessage();
            
            return result;
        }
        
    }
}