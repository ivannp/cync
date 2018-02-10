using CloudSync.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudSync.UnitTests
{
    [TestClass]
    public class LexicalPathTests
    {
        [TestMethod]
        public void TestClean()
        {
            string path = @"/adir/bdir/cdir/ddir/file.txt";
            string newPath = LexicalPath.Clean(path);
            Assert.AreEqual(path, newPath);

            path = @"/adir/bdir/cdir/../../ddir/file.txt";
            newPath = LexicalPath.Clean(path);
            Assert.AreEqual(@"/adir/ddir/file.txt", newPath);

            path = @"/adir/bdir/././//////cdir/../../ddir/file.txt";
            newPath = LexicalPath.Clean(path);
            Assert.AreEqual(@"/adir/ddir/file.txt", newPath);

            path = @"./././././//adir/bdir/././//////cdir/../../ddir/file.txt";
            newPath = LexicalPath.Clean(path);
            Assert.AreEqual(@"adir/ddir/file.txt", newPath);

            path = @"../.././//./adir/bdir/cdir/ddir/file.txt";
            newPath = LexicalPath.Clean(path);
            Assert.AreEqual(@"../../adir/bdir/cdir/ddir/file.txt", newPath);
        }

        [TestMethod]
        public void TestGetFileName()
        {
            var path = @"/bdir/cdir/ddir/file.txt";
            var newPath = LexicalPath.GetFileName(path);
            Assert.AreEqual(@"file.txt", newPath);

            path = @"file.txt";
            newPath = LexicalPath.GetFileName(path);
            Assert.AreEqual(@"file.txt", newPath);

            path = @"/adir/././././///bdir/file////";
            newPath = LexicalPath.GetFileName(path);
            Assert.AreEqual(@"file", newPath);

            path = @"/";
            newPath = LexicalPath.GetDirectoryName(path);
            Assert.AreEqual(@"/", newPath);
        }

        [TestMethod]
        public void TestGetDirectoryName()
        {
            var path = @"/bdir/cdir/ddir/file.txt";
            var newPath = LexicalPath.GetDirectoryName(path);
            Assert.AreEqual(@"/bdir/cdir/ddir", newPath);

            path = @"bdir/.././cdir/ddir/file.txt";
            newPath = LexicalPath.GetDirectoryName(path);
            Assert.AreEqual(@"cdir/ddir", newPath);

            path = @"//./////////cdir";
            newPath = LexicalPath.GetDirectoryName(path);
            Assert.AreEqual(@"/", newPath);

            path = @"/";
            newPath = LexicalPath.GetDirectoryName(path);
            Assert.AreEqual(@"/", newPath);
        }
    }
}
