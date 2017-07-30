Smaac Digit Recognition Task - Omar Enayet
============================================

Introduction
------------

This is the source code for an ASP.NET Web Api service using C# used for recognizing a digit in an image file uploaded to the service.

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
http://omarsmaacdigitdetector.azurewebsites.net/api/digitimageupload containing only the Multipart encoded image file with image filename containing the digit to be recognized.

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


Service Usage Constraints:
--------------------------

The following constraints currently apply when using the service:

* 1 & only 1 file can be uploaded per POST request.
* The size of the file should not exceed 1 MB.
* The type of the file should be either jpeg or png.
* Concurrent POST requests should not exceed 1000: The maxmimum number of storage allowed per the Azure account is 1 GB, consequently the concurrent POST image upload requests to the web server should not exceed around 1024 requests, as the maximum image size is 1 MB.

Future Work:
-------------

* Support passing file as URL or as base64 image of the file.
* Support using a distinct API key for use by API user (as a substitute to authentication)
* Support an ongoing job on server to delete an orphaned image files (due to any unexpected failures).
* Support more classifiers (digit recognizers) in an ensmble classifier to choose the digit with the maximum provided confidence or by using majority voting.
* Create and use custom trained classifier(s).
* More Unit Test Coverage.
* Load Testing by sending hundreds of concurrent post requests.
* Support logging.

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

