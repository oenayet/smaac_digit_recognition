using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitRecognizerService.Models
{
    public enum ErrorCode
    {
        SUCCESS = 0 ,
        BAD_FILE_COUNT_PER_REQUEST = 1,
        IMAGE_SIZE_EXCEED_LIMIT = 2,
        IMAGE_BAD_FORMAT = 3,
        APPLICATION_ERROR = 4,
        RECOGNITION_FAILURE = 5,
    }
}