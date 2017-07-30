using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DigitRecognizerService.Models;
using DigitRecognition;
using System.Collections.Generic;

namespace DigitRecognizerService.Tests
{
    /// <summary>
    /// Unit Test for class 'DigitRecognizer'
    /// </summary>
    [TestClass]
    public class DigitRecognizerTest
    {
        /// <summary>
        /// List of digit image files
        /// </summary>
        List<IDigitImageFile> digitImagesFiles;
        /// <summary>
        /// File system utility mock object
        /// </summary>
        Mock<IFileSystemUtility> fileSystemUtil;
        /// <summary>
        /// ensemble digit recognizer mock object
        /// </summary>
        Mock<IEnsembleDigitRecognizer> ensembleDigitRecognizer;
        /// <summary>
        /// Unit test target class: digit recognizer
        /// </summary>
        DigitRecognizer recognizer;

        /// <summary>
        /// Method called before all test methods to initialize mock objects
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            digitImagesFiles = new List<IDigitImageFile>();
            fileSystemUtil = new Mock<IFileSystemUtility>();
            ensembleDigitRecognizer = new Mock<IEnsembleDigitRecognizer>();
            recognizer = new DigitRecognizer(fileSystemUtil.Object, ensembleDigitRecognizer.Object);
        }

        /// <summary>
        /// Test case where multiple files are uploaded
        /// </summary>
        [TestMethod]
        public void TestMultipleFilesUploaded()
        {
            digitImagesFiles.Add(new Mock<IDigitImageFile>().Object);
            digitImagesFiles.Add(new Mock<IDigitImageFile>().Object);

            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.BAD_FILE_COUNT_PER_REQUEST);
        }

        /// <summary>
        /// Test case where no files were uploaded
        /// </summary>
        [TestMethod]
        public void TestNoFilesUploaded()
        {
            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.BAD_FILE_COUNT_PER_REQUEST);
        }

        /// <summary>
        /// Test case where file with unsupported type is uploaded
        /// </summary>
        [TestMethod]
        public void TestBadImageFormat()
        {
            var imageFileMock = new Mock<IDigitImageFile>();

            imageFileMock.Setup(imf => imf.ContentType).Returns("App");
            digitImagesFiles.Add(imageFileMock.Object);

            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.IMAGE_BAD_FORMAT);
        }

        /// <summary>
        /// Test case where file exceeds 1 MB
        /// </summary>
        [TestMethod]
        public void TestImageSizeExceededLimit()
        {
            var imageFileMock = new Mock<IDigitImageFile>();

            imageFileMock.Setup(imf => imf.ContentType).Returns("image/jpeg");
            imageFileMock.Setup(imf => imf.ContentLength).Returns(99999999);
            digitImagesFiles.Add(imageFileMock.Object);

            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.IMAGE_SIZE_EXCEED_LIMIT);
        }


        /// <summary>
        /// Test successful case
        /// </summary>
        [TestMethod]
        public void TestSuccess()
        {
            var imageFileMock = new Mock<IDigitImageFile>();

            imageFileMock.Setup(imf => imf.ContentType).Returns("image/jpeg");
            imageFileMock.Setup(imf => imf.ContentLength).Returns(123);
            digitImagesFiles.Add(imageFileMock.Object);

            float confidence = 10;
            char digit = '0';
            ensembleDigitRecognizer.Setup(dr => dr.RecognizeDigit(It.IsAny<string>(), out confidence)).Returns(digit);

            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.SUCCESS);
            Assert.AreEqual(result.Confidence, confidence);
            Assert.AreEqual(result.Digit, digit);
        }

        /// <summary>
        /// Test case where recognizer failed to recognize the digit in the image
        /// </summary>
        [TestMethod]
        public void TestRecognitionFailure()
        {
            var imageFileMock = new Mock<IDigitImageFile>();

            imageFileMock.Setup(imf => imf.ContentType).Returns("image/jpeg");
            imageFileMock.Setup(imf => imf.ContentLength).Returns(123);
            digitImagesFiles.Add(imageFileMock.Object);

            DigitImageRecognizeResult result = recognizer.ProcessImageFiles(digitImagesFiles);

            Assert.AreEqual(result.ErrorCode, ErrorCode.RECOGNITION_FAILURE);
        }
    }
}
