using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegexHelper;

namespace RegexHelperTest
{
    [TestClass]
    public class RegexHelperTest
    {
        [TestMethod]
        public void EmptyText_IsExtension_False()
        {
            string text = string.Empty;

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OneLetter_IsExtension_False()
        {
            string text = "a";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultipleLetters_IsExtension_False()
        {
            string text = "abc";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnlyDot_IsExtension_False()
        {
            string text = ".";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidExtesion_IsExtension_True()
        {
            string text = ".abc";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = true;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvalidExtension_IsExtension_False()
        {
            string text = ".abc.";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvalidLongerExtension_IsExtension_True()
        {
            string text = ".abc.ads.ads";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvalidExtensionWithDigitsAtBegginning_IsExtension_True()
        {
            string text = ".12";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InvalidExtensionWithDigitsInTheMiddle_IsExtension_True()
        {
            string text = ".av12a";

            RegexFileHelper regexFileHelper = new RegexFileHelper();

            bool actual = regexFileHelper.IsExtension(text);
            bool expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
