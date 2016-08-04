using System;
using DirectoryUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectoryUtilitiesTest
{

    [TestClass]
    public class FileNameExtractorTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyFilePath_Extract_ThrowsException()
        {
            FileNameExtractor fileNameExtractor = new FileNameExtractor();

            string actualName = fileNameExtractor.Extract(string.Empty);
        }

        [TestMethod]
        public void FileName_Extract_SameNameReturned()
        {
            FileNameExtractor fileNameExtractor = new FileNameExtractor();

            string actualName = fileNameExtractor.Extract("fileName.ext");
            string expectedName = "fileName.ext";

            Assert.IsTrue(expectedName.Equals(actualName));
        }

        [TestMethod]
        public void FileNameWithMorePathLevels_Extract_CorrectFileNameReturned()
        {
            FileNameExtractor fileNameExtractor = new FileNameExtractor();

            string actualName = fileNameExtractor.Extract("C:\\FileFolder\\fileName.ext");
            string expectedName = "fileName.ext";

            Assert.IsTrue(expectedName.Equals(actualName));
        }
    }
}
