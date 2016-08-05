using DirectoryUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectoryUtilitiesTest
{
    [TestClass]
    public class FileComparerTest
    {
        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void SameContent_CompareFiles_ReturnsTrue()
        {
            bool actual = FileComparer.CompareFiles("SameContent.dll", "SameContent2.dll");
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void DifferentContent_CompareFiles_ReturnsFalse()
        {
            bool actual = FileComparer.CompareFiles("Newtonsoft.Json.dll", "SameContent2.dll");
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
