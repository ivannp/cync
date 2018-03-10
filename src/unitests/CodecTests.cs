using CloudSync.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.UnitTests
{
    [TestClass]
    public class CodecTests
    {
        private bool CompareFiles(string p1, string p2)
        {
            var f1 = new FileInfo(p1);
            var f2 = new FileInfo(p2);
            if (f1.Length != f2.Length) return false;
            using (var fs1 = File.OpenRead(p1))
            {
                using (var fs2 = File.OpenRead(p2))
                {
                    for (var ii = 0; ii < f1.Length; ++ii)
                    {
                        if (fs1.ReadByte() != fs2.ReadByte()) return false;
                    }
                }
            }
            return true;
        }

        private void TryDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch(Exception)
            { }
        }

        private bool CodecRun(Context context, string raw, string encoded, string decoded)
        {
            byte[] hash = new byte[32];
            CodecHelper.EncodeFile(ref context, raw, encoded, ref hash);
            CodecHelper.DecodeFile(ref context, encoded, decoded, ref hash);
            return CompareFiles(raw, decoded);
        }

        [TestMethod]
        public void TestEncryption()
        {
            var cur = Directory.GetCurrentDirectory();

            // Random file size up to about 1MB
            var random = new Random();
            var fileSize = random.Next(1 * 1024 * 1024) + 1;
            // LocalPath holds a path and deletes it in the destructor
            var rawFile = new LocalPath(Path.GetTempFileName());
            using (var ff = File.OpenWrite(rawFile.Path))
            {
                for (var ii = 0; ii < fileSize; ++ii)
                {
                    var bb = new byte[1];
                    random.NextBytes(bb);
                    ff.WriteByte(bb[0]);
                }
            }

            byte[] keys = new byte[3 * 32];
            random.NextBytes(keys);

            // Setup a context
            Context context = new Context { };
            context.Key = keys;

            var encodedFile = new LocalPath(Path.GetTempFileName());
            var decodedFile = new LocalPath(Path.GetTempFileName());

            // Test aes and compression
            context.RepoCfg = new RepoConfig(6, 1, "aes");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));

            // Test twofish and compression
            context.RepoCfg = new RepoConfig(8, 1, "twofish");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));

            // Test twofish, aes chain and compression
            context.RepoCfg = new RepoConfig(3, 1, "twofish,aes");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));

            // Test serpent, twofish, aes chain and compression
            context.RepoCfg = new RepoConfig(3, 1, "serpent,twofish,aes");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));

            // Test only compression
            context.RepoCfg = new RepoConfig(6, 1, "none");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));

            // Test just storing
            context.RepoCfg = new RepoConfig(0, 1, "none");
            Assert.IsTrue(CodecRun(context, rawFile.Path, encodedFile.Path, decodedFile.Path));
        }

        [Ignore]
        [TestMethod]
        public void TestDirectoryEncryption()
        {
            var sourceDir = @"c:\digital";

            var cur = Directory.GetCurrentDirectory();

            var random = new Random();
            byte[] keys = new byte[3 * 32];
            random.NextBytes(keys);

            var queue = new Queue<string>();
            queue.Enqueue(sourceDir);

            var sourcePath = "source_file";
            var encodedPath = "encoded_file";
            var decodedPath = "decoded_file";

            // Setup a context
            Context context = new Context { };
            context.Key = keys;
            context.RepoCfg = new RepoConfig(6, 1, "serpent,twofish,aes");

            while (queue.Count > 0)
            {
                var dirPath = queue.Dequeue();
                foreach (var item in Directory.EnumerateDirectories(dirPath))
                {
                    queue.Enqueue(item);
                }
                foreach (var item in Directory.EnumerateFiles(dirPath))
                {
                    TryDeleteFile(sourcePath);
                    TryDeleteFile(encodedPath);
                    TryDeleteFile(decodedPath);

                    File.Copy(item, sourcePath);
                    Assert.IsTrue(CodecRun(context, sourcePath, encodedPath, decodedPath));
                }
            }

            TryDeleteFile(sourcePath);
            TryDeleteFile(encodedPath);
            TryDeleteFile(decodedPath);
        }
    }
}
