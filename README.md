Smaac Digit Recognition Task - Omar Enayet
============================================

Introduction
------------

This is the source code for an ASP.NET Web Api REST service using C# used for recognizing a digit in an image file uploaded to the service.
The code uses a C# wrapper of the open source OCR engine library Tesseract for the cause of digit detection in images, it's also designed 
to easily support any other digit detectors.

Current Deployment:
-------------------

Currently the service is deployed on Azure on a web site with URL: 
http://omarsmaacdigitdetector.azurewebsites.net

The service API controller URL is:
http://omarsmaacdigitdetector.azurewebsites.net/api/digitimageupload

Note: Requests to any other links in the azure web site other than the API controller link is currently not handled and not supported.

Service Use:
-------------

Send a POST request to :
http://omarsmaacdigitdetector.azurewebsites.net/api/digitimageupload containing only the Multipart encoded image file (with image filename) in message body containing the digit to be recognized.

A flat JSON structure with the following fields will be received:

* 'digit': Recognized digit, value is undefined in case error code != 0
* 'error_msg': Error Message
* 'confidence': Confidence value returned by classifier, , value is 0 in case error code != 0
* 'error_code': Error Code, which can be one of the following:
	* SUCCESS (0): Success
	* BAD_FILE_COUNT_PER_REQUEST (1) : One and only one file should be uploaded per post request.
	* IMAGE_SIZE_EXCEED_LIMIT (2): Image size exceeded maximum limit. It should be under 1 MB.
	* IMAGE_BAD_FORMAT (3): Image format is not supported.
	* APPLICATION_ERROR (4): An application error has occurred.
	* RECOGNITION_FAILURE (5): Could not recognize digit in image.

Example JSON structure returned:

{
    "digit": "0",
    "error_msg": "Success !",
    "confidence": 0.9100000262260437,
    "error_code": 0
}
	
Curl Command Example:
---------------------

curl -X POST \
  http://omarsmaacdigitdetector.azurewebsites.net/api/digitimageupload \
  -H 'cache-control: no-cache' \
  -H 'content-type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW' \
  -F Image1=@1.JPG
  
where '1.jpg' is the an example of the name of the image file.
	
Service Usage Constraints:
--------------------------

The following constraints currently apply when using the service:

* 1 & only 1 file can be uploaded per POST request.
* The size of the file should not exceed 1 MB.
* The type of the file should be either jpeg or png.
* Concurrent POST requests should not exceed 1000: The maxmimum number of storage allowed per the Azure account is 1 GB, consequently the concurrent POST image upload requests to the web server should not exceed around 1024 requests, as the maximum image size is 1 MB.

Future Work:
-------------

Features:
* Support passing file as URL or as base64 image of the file.
* Support using a distinct API key for use by API user (as a form of authentication)
* Support an ongoing job on server to delete an orphaned image files (due to any unexpected failures).
* Support more classifiers (digit recognizers) in an ensmble classifier to choose the digit with the maximum provided confidence or by using majority voting.
* Create and use custom trained classifier(s).
* Support uploading multiple image files per request.

Development:
* More Unit Test Coverage.
* Load Testing by sending hundreds of concurrent post requests.
* Enhance latency of handling single POST request by initializing the OCR library only once when the web application is started.

Internal Functions outline
--------------------------

- The image file is stored on disk with a random file name
- File path is passed to the Tesseract open source library's OCR engine to perform the OCR process constrained to number digits.
- File Path is deleted.
- JSON result is returned as a respone to the POST request

Compilation Requirements:
---------------------------

- Visual Studio 2015 Free Community Edition with NuGet.
- .NET framework 4.5.2

Code Structure:
------------------

The C# Visual Studio solution consists of 3 projects:

* DigitRecognition: Project for .NET DLL containing the digit recognition logic.
* DigitRecognizerService: Project for .NET DLL containing the ASP.NET Web API REST service.
* DigitRecognizerService.Tests: Project for unit/integration tests applied for project DigitRecognizerService. Currently it contains 1 unit test class for class DigitRecognizer.

Notes:
* Logging to Azure application web site logs is supported.

More About me:
-----------------

My AI-related Blog(s):
http://omarsbrain.wordpress.com/
https://rtsairesearch.wordpress.com/

My Resume: (Feb 2017)
https://app.box.com/s/wc8cjtrtfcx0a4b3mqol9ktmaol80q9q

LinkedIn Profile:
https://www.linkedin.com/in/omarenayet/



