using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CloudSync.Core
{
    public class ObjectTree : IObjectTree
    {
        static readonly HashSet<char> _hexDigits = new HashSet<char>()
        {
            'a', 'b', 'c', 'd', 'e', 'f',
            'A', 'B', 'C', 'D', 'E', 'F',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static FileId RootId = new FileId("7f5e4fcc312f49bf8a057dbd0008c7af");
        public const string ObjectsDir = "objects";
        private const string EyeCatcher = "clouddir";

        private static void Upload(ref Context context, string src, FileId fid)
        {
            var pp = Path.Combine(ObjectsDir, fid.Directory).Replace(@"\", @"/");
            context.Storage.CreateDirectory(pp);
            pp = Path.Combine(pp, fid.FileName).Replace(@"\", @"/");
            context.Storage.Upload(src, pp, true);
        }

        private static void Upload(ref Context context, string src, string dest)
        {
            context.Storage.Upload(src, dest, true);
        }

        public void CreateRoot(ref Context context)
        {
            context.Storage.CreateDirectory(ObjectsDir);

            BaseDir baseDir = new Core.BaseDir { Eyecatcher = EyeCatcher };

            // Serialize to a temp file
            using (var path = new LocalPath(LocalUtils.GetTempFileName(suffix: ".dir")))
            using (var encodedPath = new LocalPath(LocalUtils.GetTempFileName(suffix: ".edir")))
            {
                FileStream output = File.OpenWrite(path.Path);

                // Write the directory to a file
                baseDir.WriteTo(output);
                output.Close();

                byte[] hash = new byte[32];
                CodecHelper.EncodeFile(ref context, path.Path, encodedPath.Path, ref hash);

                // Upload the file. It's removed by Upload.
                Upload(ref context, encodedPath.Path, RootId);
            }

            // Write the config
            using (var configPath = new LocalPath(LocalUtils.GetTempFileName(suffix: ".config")))
            {
                context.RepoCfg.Save(configPath.Path);
                Upload(ref context, configPath.Path, RepoConfig.ConfigFile);
            }
        }

        class Dir
        {
            Context context;
            BaseDir data;
            // The cannonical path to this directory: /users/rake/documents
            string path;
            FileId fid;
            bool dirty;

            public static Dir OpenRoot(ref Context cc)
            {
                Dir dir = new Dir { context = cc, path = "/", fid = RootId, dirty = false };
                dir.Read();
                return dir;
            }

            public static bool IsDir(DirEntry de)
            {
                return de.Type == DirEntryType.Dir;
            }

            public static bool IsFile(DirEntry de)
            {
                return de.Type == DirEntryType.File;
            }

            public IEnumerator<KeyValuePair<string, DirEntry>> GetEnumerator()
            {
                return data.Entries.GetEnumerator();
            }

            public static Dir Open(ref Context context, string path, bool create = false, bool filePathOk = false)
            {
                Dir dir = OpenRoot(ref context);
                string[] dirs = PathComponents(path);
                for (int ii = 0; ii < dirs.Length; ++ii)
                {
                    DirEntry de;
                    bool exists = dir.data.Entries.TryGetValue(dirs[ii], out de);
                    if (exists)
                    {
                        if (IsFile(de))
                        {
                            if (ii == dirs.Length - 1 && filePathOk)
                            {
                                // The last component is a file, and this is allowed - success
                                return dir;
                            }
                            else
                            {
                                throw new PathException("The destination path [" + path + "] has non-directory components in it.");
                            }
                        }

                        // Ascend into the next entry
                        dir.path += "/" + dirs[ii];
                        dir.fid = new FileId(de.Versions[0].Uuid);
                        dir.Read();
                    }
                    else
                    {
                        if (create)
                        {
                            var tt = MakeNewDir(ref context);

                            // Add the new directory to the entry list
                            VersionEntry ve = new VersionEntry { Uuid = tt.Item2, LastWriteTime = DateTime.Now.Ticks };
                            DirEntry newDirEntry = new DirEntry { Type = DirEntryType.Dir, Latest = 0 };
                            newDirEntry.Versions.Add(ve);
                            dir.data.Entries.Add(dirs[ii], newDirEntry);
                            dir.Write();

                            // Acend into the created directory
                            dir.path = LexicalPath.Combine(dir.path, dirs[ii]);
                            dir.fid = new FileId(tt.Item2);
                            dir.data = tt.Item1.data;
                        }
                        else
                        {
                            throw new PathException("The destination path [" + path + "] doesn't exist.");
                        }
                    }
                }
                return dir;
            }

            public static Tuple<Dir, string> MakeNewDir(ref Context cc)
            {
                Dir dir = new Dir { context = cc };
                dir.data = new BaseDir();
                dir.data.Eyecatcher = EyeCatcher;

                var uuid = Guid.NewGuid().ToString("N");

                dir.fid = new FileId(uuid);

                dir.Write();

                return Tuple.Create(dir, uuid);
            }

            public void Read()
            {
                // Download the directory file
                using (var localPath = context.Storage.Download(fid.FullPath))
                using (var decodedPath = new LocalPath(LocalUtils.GetTempFileName(suffix: ".dir")))
                {
                    var fi = new FileInfo(localPath.Path);
                    if (fi.Length < 4)
                        throw new Exception($"The downloaded file is too short, only {fi.Length} bytes. Minimum is 4.");

                    byte[] hash = new byte[32];
                    CodecHelper.DecodeFile(ref context, localPath.Path, decodedPath.Path, ref hash);

                    // Read the directory content
                    using (Stream ss = OpenTempFileForReading(decodedPath.Path))
                    {
                        MessageParser<BaseDir> parser = new MessageParser<BaseDir>(() => new BaseDir());
                        data = parser.ParseFrom(ss);
                    }
                }
            }

            public void Write()
            {
                using (var rawContent = new LocalPath(LocalUtils.GetTempFileName(suffix: ".dir")))
                {
                    using (FileStream fs = File.OpenWrite(rawContent.Path))
                    {
                        // Serialize the content
                        data.WriteTo(fs);
                    }

                    if (!File.Exists(rawContent.Path))
                        throw new Exception($"The temporary file {rawContent.Path} disappeared!");

                    // Use another temp file to encode the content
                    using (var encodedPath = new LocalPath(LocalUtils.GetTempFileName(prefix: "cync", suffix: ".edir")))
                    {
                        byte[] hash = new byte[32];
                        CodecHelper.EncodeFile(ref context, rawContent.Path, encodedPath.Path, ref hash);

                        // Make sure the target directory exists
                        context.Storage.CreateDirectory(fid.DirectoryPath);

                        // Upload the encoded file
                        context.Storage.Upload(encodedPath.Path, fid.FullPath, true);

                        dirty = false;
                    }
                }
            }

            private FileStream OpenTempFileForWriting(string path)
            {
                return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.DeleteOnClose);
            }

            private FileStream OpenTempFileForReading(string path)
            {
                return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            }

            private static string[] PathComponents(string path)
            {
                return path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            }

            public bool TryGetEntry(string entry, out DirEntry de)
            {
                return data.Entries.TryGetValue(entry, out de);
            }

            public bool HasEntry(string entry)
            {
                return data.Entries.ContainsKey(entry);
            }

            public void AddEntry(string name, DirEntry de)
            {
                data.Entries.Add(name, de);
            }

            public void AddDir(string name, string existingPath = null)
            {
                if (!TryGetEntry(name, out DirEntry de))
                {
                    var tt = MakeNewDir(ref context);
                    long lastWriteTime, lastAccessTime, creationTime;
                    if (existingPath == null)
                    {
                        lastWriteTime = lastAccessTime = creationTime = DateTime.Now.Ticks;
                    }
                    else
                    {
                        var fi = new FileInfo(existingPath);
                        lastWriteTime = fi.LastWriteTimeUtc.Ticks;
                        lastAccessTime = fi.LastAccessTimeUtc.Ticks;
                        creationTime = fi.CreationTimeUtc.Ticks;
                    }

                    // Add the new directory to the entry list
                    VersionEntry ve = new VersionEntry { Uuid = tt.Item2, LastWriteTime = lastWriteTime, LastAccessTime = lastAccessTime, CreationTime = creationTime };
                    DirEntry newDirEntry = new DirEntry { Type = DirEntryType.Dir, Latest = 0 };
                    newDirEntry.Versions.Add(ve);
                    data.Entries.Add(name, newDirEntry);

                    // Write the updated directory
                    Write();
                }
            }

            public void ChangeDirDown(string subDir)
            {
                DirEntry de;
                if (!TryGetEntry(subDir, out de))
                {
                    throw new PathException("The sub directory " + subDir + " doesn't exist.");
                }

                if (IsFile(de))
                {
                    throw new PathException("The entry is a file, not a directory. Failed to change directory.");
                }

                path = LexicalPath.Combine(path, subDir);
                fid = new FileId(de.Versions[de.Latest].Uuid);
                Read();
            }

            public bool PullFile(string src, string dest, bool force)
            {
                bool exists = TryGetEntry(src, out DirEntry de);
                // Check whether the destination file exists.
                // TODO Check whether the destination is newer
                var ve = de.Versions[de.Latest];
                if (!force)
                {
                    if (File.Exists(dest))
                    {
                        if (File.GetLastWriteTimeUtc(dest).Ticks > ve.LastWriteTime)
                            return false;

                        var fi = new FileInfo(dest);
                        if ((ulong)fi.Length == ve.Length)
                        {
                            if (context.UseChecksums)
                            {
                                // If the hashes are also the same, skip the file
                                byte[] destHash = new byte[32];
                                CodecHelper.ComputeHash(dest, ref destHash);
                                if (destHash.SequenceEqual(ve.Checksum))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                var fid = new FileId(ve.Uuid);
                var encoded = context.Storage.Download(fid.FullPath);
                byte[] hash = new byte[32];
                CodecHelper.DecodeFile(ref context, encoded.Path, dest, ref hash);

                // Restore the file times
                File.SetCreationTimeUtc(dest, new DateTime(ve.CreationTime));
                File.SetLastAccessTimeUtc(dest, new DateTime(ve.LastAccessTime));
                File.SetLastWriteTimeUtc(dest, new DateTime(ve.LastWriteTime));

                return true;
            }

            public bool PushFile(string src, string dest, bool force)
            {
                if (dest == null || dest.Length == 0)
                    dest = Path.GetFileName(src);

                DirEntry de;
                FileInfo fi = new FileInfo(src);
                bool exists = TryGetEntry(dest, out de);
                if (exists)
                {
                    bool changeDetected = force;
                    if (!changeDetected)
                    {
                        VersionEntry ve = de.Versions[de.Latest];
                        // Compare the file time
                        if (fi.LastWriteTimeUtc.Ticks > ve.LastWriteTime)
                        {
                            // Compare the size
                            if ((ulong)fi.Length != ve.Length)
                            {
                                changeDetected = true;
                            }
                            else if(context.UseChecksums)
                            {
                                // Checksum check
                                byte[] sha256 = new byte[32];
                                ComputeHashParams bb = new ComputeHashParams { };
                                bb.Hash = "sha256";
                                bb.Path = src;

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bb.WriteTo(ms);
                                    CodecDll.ComputeHash(ms.GetBuffer(), (uint)ms.Length, sha256, (uint)sha256.Length);
                                }

                                // Compare the hash
                                changeDetected = !sha256.SequenceEqual(ve.Checksum);
                            }

                            if (!changeDetected)
                            {
                                // Update the timestamp only
                                de.Versions[de.Latest].LastWriteTime = fi.LastWriteTimeUtc.Ticks;
                                return true;
                            }
                        }
                    }

                    if (!changeDetected)
                        return false;
                }
                else
                {
                    de = new DirEntry { Type = DirEntryType.File, Latest = 0 };
                    AddEntry(dest, de);
                }

                VersionEntry versionToRemove = null;

                // Encode the file to a temp location
                using (var lp = new LocalPath(LocalUtils.GetTempFileName()))
                {
                    var hash = new byte[32];
                    CodecHelper.EncodeFile(ref context, src, lp.Path, ref hash);

                    var newVersionEntry = new VersionEntry
                    {
                        Uuid = Guid.NewGuid().ToString("N"),
                        LastWriteTime = fi.LastWriteTimeUtc.Ticks,
                        LastAccessTime = fi.LastAccessTimeUtc.Ticks,
                        CreationTime = fi.CreationTimeUtc.Ticks,
                        Attributes = (int)fi.Attributes,
                        Length = (ulong)fi.Length
                    };
                    newVersionEntry.Checksum = ByteString.CopyFrom(hash);

                    if (de.Versions.Count == 0)
                    {
                        // The very first version addition
                        de.Versions.Add(newVersionEntry);
                        Debug.Assert(de.Latest == 0);
                    }
                    else if (de.Versions.Count > 0 && de.Versions.Count >= context.RepoCfg.MaxVersions)
                    {
                        de.Latest = (de.Latest + 1) % context.RepoCfg.MaxVersions;
                        versionToRemove = de.Versions[de.Latest];
                        de.Versions[de.Latest] = newVersionEntry;
                    }
                    else
                    {
                        de.Versions.Add(newVersionEntry);
                        ++de.Latest;
                    }

                    // The in-memory directory is updated, perform the file operations
                    FileId fid = new FileId(newVersionEntry.Uuid);
                    context.Storage.CreateDirectory(fid.DirectoryPath);
                    context.Storage.Upload(lp.Path, fid.FullPath);
                }

                // Remove the previous version if necessary
                if (versionToRemove != null)
                    context.Storage.RemoveFile((new FileId(versionToRemove.Uuid)).FullPath);

                Write();

                return true;
            }

            /// <summary>
            /// Removes and entry from the directory. If removeData is true (default), the
            /// the data is also removed.
            /// 
            /// removeData is set to false by the 'move' command.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="removeData">true to remove the data</param>
            public void RemoveEntry(string name, bool removeData = true)
            {
                if (!TryGetEntry(name, out DirEntry de))
                    return;
                if(removeData)
                {
                    // Delete all versions
                    foreach (var version in de.Versions)
                    {
                        var fileId = new FileId(version.Uuid);
                        if(context.IgnoreErrors)
                        {
                            try
                            {
                                context.Storage.RemoveFile(fileId.FullPath);
                            }
                            catch (Exception)
                            { }
                        }
                        else
                        {
                            context.Storage.RemoveFile(fileId.FullPath);
                        }
                    }
                }
                // Remove the directory entry
                data.Entries.Remove(name);
                dirty = true;
                Write();
            }

            public void RemoveAllEntries()
            {
                foreach(var kv in data.Entries)
                {
                    // Delete all versions
                    foreach (var version in kv.Value.Versions)
                    {
                        context.Storage.RemoveFile(new FileId(version.Uuid).FullPath);
                    }
                }
            }
        }

        public sealed class ObjectInfo
        {
            public int Attributes { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime LastAccessTime { get; set; }
            public DateTime LastWriteTime { get; set; }
            public ulong Length { get; set; }
            public byte[] Checksum { get; set; }

            private bool directory;

            public bool IsFile()
            {
                return !directory;
            }

            public bool IsDir()
            {
                return directory;
            }

            public ObjectInfo(Context context, string path)
            {
                string dirName = LexicalPath.GetDirectoryName(path);
                Dir dir = Dir.Open(ref context, dirName, false, false);
                DirEntry de;
                bool exists = dir.TryGetEntry(LexicalPath.GetFileName(path), out de);
                if (exists)
                {
                    var ve = de.Versions[de.Latest];

                    Attributes = ve.Attributes;
                    CreationTime = new DateTime(ve.CreationTime);
                    LastAccessTime = new DateTime(ve.LastAccessTime);
                    LastWriteTime = new DateTime(ve.LastWriteTime);
                    Length = ve.Length;
                    Checksum = ve.Checksum.ToByteArray();

                    directory = Dir.IsDir(de);
                }
                else
                {
                    throw new PathException($"The path {path} doesn't exist.");
                }
            }

            public ObjectInfo()
            {
            }
        }

        public void Pull(ref Context context, string src, string dest)
        {
            bool srcEndsWithSeparator = OSPath.IsPathSeparator(src.Last());

            // src is a repository path. We use unix style for it.
            src = LexicalPath.Clean(src);

            // dest is local file system path
            dest = Path.GetFullPath(dest);

            Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();

            ObjectInfo oi = new ObjectInfo(context, src);

            if (oi.IsDir())
            {
                // src is a folder, let's define the desired behaviour, following
                // the design of rsync:
                //
                // cync pull /user/documents/ c:/home/user
                //    Content of /user/documents goes into c:/home/user
                //
                // cync pull /user/documents c:/home/user
                //    Content of /user/documents goes into c:/home/user/documents
                //
                // Notice the difference caused by the presence/lack of the final slash
                // in the source (documents/ vs just documents).
                var fileName = LexicalPath.GetFileName(src);
                var destPath = dest;
                if (fileName.Length > 0 && !srcEndsWithSeparator)
                {
                    destPath = Path.Combine(destPath, fileName);
                    Directory.CreateDirectory(destPath);
                }

                queue.Enqueue(Tuple.Create(src, destPath));
            }
            else
            {
                throw new NotSupportedException("Only directories are supported currently.");
            }

            // Process the queue
            while(queue.Count > 0)
            {
                var tt = queue.Dequeue();
                var srcDir = Dir.Open(ref context, tt.Item1, false, false);
                foreach(var pp in srcDir)
                {
                    if(Dir.IsDir(pp.Value))
                    {
                        var destDirPath = Path.Combine(tt.Item2, pp.Key);
                        var srcDirPath = LexicalPath.Combine(tt.Item1, pp.Key);
                        Directory.CreateDirectory(destDirPath);
                        queue.Enqueue(Tuple.Create(srcDirPath, destDirPath));
                        context.InfoWriteLine($"Queueing directory {srcDirPath} [target: {destDirPath}]");
                    }
                    else
                    {
                        var srcFullPath = LexicalPath.Combine(tt.Item1, pp.Key);
                        var destFullPath = Path.Combine(tt.Item2, pp.Key);
                        var bb = srcDir.PullFile(pp.Key, destFullPath, false);
                        if (bb)
                        {
                            context.InfoWriteLine($"Copied {srcFullPath} to {destFullPath}");
                        }
                        else
                        {
                            context.InfoWriteLine($"Skipped {srcFullPath}. The local file [{destFullPath}] is either newer or the same as the repository file.");
                        }
                    }
                }
            }
        }

        public void Push(ref Context context, string src, string dest)
        {
            bool srcEndsWithSeparator = OSPath.IsPathSeparator(src.Last());

            // src is a local file system path, thus, use the Path and the OSPath classes
            src = Path.GetFullPath(OSPath.Clean(src));

            FileAttributes fa = File.GetAttributes(src);
            Queue<Tuple<string, string>> queue = new Queue<Tuple<string, string>>();
            if ((fa & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // src is a folder, let's define the desired behaviour, following
                // the design of rsync:
                //
                // cync push c:/home/user/documents/ /user/documents
                //    Content of c:/home/user/documents goes into /user/documents
                //
                // cync push c:/home/user/documents /user/documents
                //    Content of c:/home/user/documents goes into /user/documents/documents
                //
                // Notice the difference caused by the presence/lack of the final 
                // directory separator (slash in the above example) in the source
                // (documents/ vs just documents).

                if(srcEndsWithSeparator)
                {
                    string fullDestPath = dest;
                    queue.Enqueue(Tuple.Create(src, fullDestPath));
                    context.InfoWriteLine($"Queueing directory '{src}' [target: '{fullDestPath}']");
                }
                else
                {
                    string fullDestPath = LexicalPath.Combine(dest, Path.GetFileName(src));
                    queue.Enqueue(Tuple.Create(src, fullDestPath));
                    context.InfoWriteLine($"Queueing directory '{src}' [target: '{fullDestPath}']");
                }

                // Compute total size
                var sizeQueue = new Queue<string>(queue.Select(t => t.Item1).ToList());
                var totalSize = 0L;
                while(sizeQueue.Count > 0)
                {
                    var path = sizeQueue.Dequeue();
                    foreach (var ee in Directory.EnumerateFileSystemEntries(path))
                    {
                        FileInfo fi = new FileInfo(ee);
                        if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                            sizeQueue.Enqueue(ee);
                        else
                            totalSize += fi.Length;
                    }
                }

                var progress = new ProgressBar(totalSize);
                var processedBytes = 0L;

                // Process the queue
                while (queue.Count > 0)
                {
                    var tt = queue.Dequeue();

                    // Process all directory entries. Files are added to the repo directly,
                    // sub directories are queued for further processing.
                    var entries = Directory.EnumerateFileSystemEntries(tt.Item1);

                    // Create the path if it doesn't exist
                    Dir curDestDir = Dir.Open(ref context, LexicalPath.GetDirectoryName(tt.Item2), true);
                    if (tt.Item2.Length > 0 && tt.Item2 != "/")
                    {
                        string destPath = LexicalPath.GetFileName(tt.Item2);
                        if (!curDestDir.HasEntry(destPath))
                            curDestDir.AddDir(destPath, tt.Item1);
                        curDestDir.ChangeDirDown(destPath);
                    }

                    foreach (var ee in entries)
                    {
                        FileInfo fi = new FileInfo(ee);
                        if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            // Enque the directory for later processing
                            var destFullPath = LexicalPath.Combine(tt.Item2, Path.GetFileName(ee));
                            queue.Enqueue(Tuple.Create(ee, destFullPath));
                            context.InfoWriteLine($"Queueing directory '{ee}' [target: '{destFullPath}']");
                        }
                        else
                        {
                            if(!context.Verbose)
                            {
                                progress.Text = $"Processing '{ee}'";
                                progress.Update(processedBytes);
                            }

                            // A file - copy it
                            var added = curDestDir.PushFile(ee, Path.GetFileName(ee), false);
                            if (added)
                                context.InfoWriteLine($"Added '{ee}' as '{LexicalPath.Combine(tt.Item2, Path.GetFileName(ee))}'");
                            else
                                context.InfoWriteLine($"Skipped '{ee}'. The repository file '{LexicalPath.Combine(tt.Item2, Path.GetFileName(ee))}' is newer or the same.");

                            processedBytes += fi.Length;
                        }
                    }
                }

                if(!context.Verbose)
                {
                    progress.Text = "";
                    progress.Update(processedBytes);
                }
            }
        }

        public void List(ref Context context, string path, OutputConfig config)
        {
            var dir = Dir.Open(ref context, path, false, false);
            var items = new List<ItemInfo>();
            foreach (var kv in dir)
            {
                var version = kv.Value.Versions[kv.Value.Latest];
                var isDir = (kv.Value.Type & DirEntryType.Dir) == DirEntryType.Dir;
                items.Add(new ItemInfo { Name = kv.Key, Size = (long)version.Length, IsDir = isDir, LastWriteTime = new DateTime(version.LastWriteTime, DateTimeKind.Utc) });
            }
            new ConsoleListOutputter(config).Output(items);
        }

        public void Remove(ref Context context, string path)
        {
            // Open the destination except the last path element
            path = LexicalPath.Clean(path);
            string pathDir = LexicalPath.GetDirectoryName(path);
            Dir dir = Dir.Open(ref context, pathDir, false, false);

            // Lookup the last path element into the directory
            var fileName = Path.GetFileName(path);
            if (!dir.TryGetEntry(fileName, out DirEntry de))
                return;
            if(Dir.IsFile(de))
            {
                dir.RemoveEntry(fileName);
            }
            else
            {
                var scanStack = new Stack<string>();
                context.InfoWriteLine($"Queueing directory '{path}'");
                scanStack.Push(path);

                var stack = new Stack<string>();
                while(scanStack.Count != 0)
                {
                    var top = scanStack.Pop();
                    dir = Dir.Open(ref context, top, false, false);
                    stack.Push(top);
                    foreach(var kv in dir)
                    {
                        if (Dir.IsDir(kv.Value))
                        {
                            var fullPath = LexicalPath.Combine(top, kv.Key);
                            context.InfoWriteLine($"Queueing directory '{fullPath}'");
                            scanStack.Push(fullPath);
                        }
                    }
                }

                while(stack.Count != 0)
                {
                    var top = stack.Pop();
                    context.InfoWriteLine($"Removing directory '{top}'");
                    dir = Dir.Open(ref context, top, false, false);
                    dir.RemoveAllEntries();
                    var parentDir = Dir.Open(ref context, LexicalPath.GetDirectoryName(top));
                    parentDir.RemoveEntry(LexicalPath.GetFileName(top));
                    context.InfoWriteLine($"Removed directory '{top}'");
                }
            }
        }

        public void Move(ref Context context, string src, string dest)
        {
            // Open the destination except the last path element
            src = LexicalPath.Clean(src);
            if(src == "/")
            {
                context.ErrorWriteLine($"Cannot move the root directory.");
                return;
            }
            string srcDirPath = LexicalPath.GetDirectoryName(src);
            var srcDir = Dir.Open(ref context, srcDirPath, false, false);

            // Lookup the last path element into the directory
            var srcFileName = LexicalPath.GetFileName(src);
            if (!srcDir.TryGetEntry(srcFileName, out DirEntry de))
                return;

            dest = LexicalPath.Clean(dest);
            string destDirPath = LexicalPath.GetDirectoryName(dest);
            var destDir = Dir.Open(ref context, destDirPath, false, false);

            var destFileName = Path.GetFileName(dest);
            if (destDir.TryGetEntry(srcFileName, out DirEntry destDirEntry))
            {
                context.ErrorWriteLine($"The destination exists.");
                return;
            }

            destDir.AddEntry(destFileName, de);
            destDir.Write();

            srcDir.RemoveEntry(srcFileName, false);
            srcDir.Write();
        }

        public void Verify(ref Context context, bool repair)
        {
            var uuids = ListUuids(context);
            var objects = ListObjects(ref context);

            var orphanUuids = uuids.Where(s => !objects.ContainsKey(s)).ToList();
            var orphanObjects = objects.Where(kv => !uuids.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);

            foreach(var uuid in orphanUuids)
                context.ErrorWriteLine($"Object {uuid} is an orphan.");

            foreach(var obj in orphanObjects)
                context.ErrorWriteLine($"Path {obj.Value} exists only in the file system metadata. {obj.Key} doesn't exist in the file system.");

            if(orphanUuids.Count > 0 || orphanObjects.Count > 0)
            {
                context.ErrorWriteLine(string.Format("Examined {0:n0} file system object. Found {1:n0} orphans.", uuids.Count, orphanUuids.Count));
                context.ErrorWriteLine(string.Format("Examined {0:n0} files and folders. Found {1:n0} orphans.", objects.Count, orphanObjects.Count));
            }

            if(orphanUuids.Count == 0 && orphanObjects.Count == 0)
                context.AlwaysWriteLine("Verification was successful. No errors were found.");

            if (!repair)
                return;

            var storage = context.Storage;
            var repaired = 0;
            foreach (var uuid in orphanUuids)
            {
                var fileId = new FileId(uuid);
                var path = LexicalPath.Combine(ObjectsDir, fileId.Directory, fileId.FileName);
                try
                {
                    storage.RemoveFile(path);
                    ++repaired;
                    context.AlwaysWriteLine($"Removed {uuid}");
                }
                catch(Exception)
                {
                    context.ErrorWriteLine($"Failed to remove {path}");
                }
            }
            context.ErrorWriteLine(string.Format("{0:n0} out of {1:n0} file system objects were successfully repaired.", repaired, orphanUuids.Count));
        }

        public void CreateDirectory(ref Context context, string path, bool createParents)
        {
            try
            {
                if (createParents)
                {
                    var dir = Dir.Open(ref context, path, true);
                    dir.Write();
                }
                else
                {
                    var parentDir = LexicalPath.GetDirectoryName(path);
                    var dir = Dir.Open(ref context, parentDir);
                    dir.AddDir(LexicalPath.GetFileName(path));
                    dir.Write();
                }
            }
            catch(Exception)
            {
                context.ErrorWriteLine($"Failed creating '{path}'");
            }
        }

        private bool IsHex(char c)
        {
            return _hexDigits.Contains(c);
        }

        private bool IsFolderName(string name)
        {
            return name.Length == 2 && IsHex(name[0]) && IsHex(name[1]);
        }

        private HashSet<string> ListUuids(Context context)
        {
            var storage = context.Storage;

            var folders = new List<string>();
            foreach (var item in storage.ListDirectory(ObjectsDir))
            {
                if (item.IsDir && IsFolderName(item.Name))
                    folders.Add(item.Name);
            }

            var uuids = new HashSet<string>();

            foreach (var folder in folders)
            {
                var files = storage.ListDirectory(LexicalPath.Combine(ObjectsDir, folder));
                foreach(var file in files)
                    uuids.Add(folder + file.Name);
            }

            return uuids;
        }

        private Dictionary<string, string> ListObjects(ref Context context)
        {
            var res = new Dictionary<string, string>();
            var queue = new Queue<string>();
            queue.Enqueue("/");
            while(queue.Count != 0)
            {
                var path = queue.Dequeue();
                var dir = Dir.Open(ref context, path);
                foreach(var kv in dir)
                {
                    var fullPath = LexicalPath.Combine(path, kv.Key);
                    foreach (var version in kv.Value.Versions)
                        res.Add(version.Uuid, fullPath);
                    if (kv.Value.Type == DirEntryType.Dir)
                        queue.Enqueue(fullPath);
                }
            }
            // The traversal is successfull, the root directory exists. Add it.
            res.Add(RootId.ToString(), "/");
            return res;
        }
    }
}
