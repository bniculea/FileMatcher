using System;
using DirectoryUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectoryUtilitiesTest
{
    [TestClass]
    public class FileHasherTest
    {
        [TestMethod]
        [DeploymentItem("TestFiles//FileHashingTest")]
        public void SameContentButDifferentName_GetHash_True()
        {
            FileHasher fileHasher = new FileHasher();

            string hashFile1 = fileHasher.GetHash("File1.txt");
            string hashFile2 = fileHasher.GetHash("File2.txt");

            Assert.IsTrue(hashFile1.Equals(hashFile2));
        }

        [TestMethod]
        [DeploymentItem("TestFiles//FileHashingTest")]
        public void SameFileButDifferentLocations_GetHash_True()
        {
            FileHasher fileHasher = new FileHasher();

            string hashFile1 = fileHasher.GetHash("Path1//File1.txt");
            string hashFile2 = fileHasher.GetHash("Path2//File1.txt");

            Assert.IsTrue(hashFile1.Equals(hashFile2));
        }

        [TestMethod]
        [DeploymentItem("TestFiles//FileHashingTest")]
        public void SameFileButDifferentLocationsAndContent_GetHash_False()
        {
            FileHasher fileHasher = new FileHasher();

            string hashFile1 = fileHasher.GetHash("Path1//File1.txt");
            string hashFile2 = fileHasher.GetHash("File1.txt");

            Assert.IsFalse(hashFile1.Equals(hashFile2));
        }
    }
}
