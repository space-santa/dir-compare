using DirCompare;
using NUnit.Framework;

namespace Tests
{
    public class PathProcessorTests
    {
        [Test]
        public void GetConsistentPathWithoutBase_BasePathWithOutTrailingSeparator_ResultWithoutLeadingSeparator()
        {
            string basePath = "\\lala";
            string path = "\\lala\\bubu\\dodo";
            string expected = "bubu\\dodo";
            Assert.AreEqual(expected, PathProcessor.GetConsistentPathWithoutBase(path, basePath));
        }

        [Test]
        public void GetConsistentPathWithoutBase_BasePathWithTrailingSeparator_ResultWithoutLeadingSeparator()
        {
            string basePath = "\\lala\\";
            string path = "\\lala\\bubu\\dodo";
            string expected = "bubu\\dodo";
            Assert.AreEqual(expected, PathProcessor.GetConsistentPathWithoutBase(path, basePath));
        }

        [Test]
        public void GetConsistentPathWithoutBase_PathWithUnixSeparator_ResultWithWindowsSeparator()
        {
            string basePath = "/lala/";
            string path = "/lala/bubu/dodo";
            string expected = "bubu\\dodo";
            Assert.AreEqual(expected, PathProcessor.GetConsistentPathWithoutBase(path, basePath));
        }

        [Test]
        public void GetConsistentPathWithoutBase_EmptyBasePath_ThrowsArgumentException()
        {
            string basePath = "";
            string path = "/lala/bubu/dodo";
            Assert.Throws<System.ArgumentException>(() => PathProcessor.GetConsistentPathWithoutBase(path, basePath));
        }

        [Test]
        public void GetConsistentPathWithoutBase_EmptyPath_ThrowsArgumentException()
        {
            string basePath = "/lala/";
            string path = "";
            Assert.Throws<System.ArgumentException>(() => PathProcessor.GetConsistentPathWithoutBase(path, basePath));
        }
    }
}
