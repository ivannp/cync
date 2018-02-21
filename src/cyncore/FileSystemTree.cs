using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudSync.Core
{
    public class FileSystemTree : IObjectTree
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly HashSet<char> _invalidChars = new HashSet<char>(Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()).ToArray());

        public void CreateRoot(ref Context context)
        {
        }

        public void List(ref Context context, string path, OutputConfig config)
        {
            path = LexicalPath.Clean(path);
            if(path != "/")
            {
                var itemInfo = context.Storage.GetItemInfo(path);
                if (!itemInfo.IsDir)
                    throw new Exception($"The 'list' command is only supported for folders.");
            }

            new ConsoleListOutputter(config).Output(context.Storage.ListDirectory(path));
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
                //    Content of c:/home/user/documents goes into /user
                //
                // cync push c:/home/user/documents /user/documents
                //    Content of c:/home/user/documents goes into c:/home/user/documents
                //
                // Notice the difference caused by the presence/lack of the final 
                // directory separator (slash in the above example) in the source
                // (documents/ vs just documents).

                // We need to figure out whether the destionation exists, and if so, what
                // is the destionation's type [file vs folder].

                dest = LexicalPath.Clean(dest);

                var item = context.Storage.GetItemInfo(dest);

                if(item != null)
                {
                    // The destination exists
                    if(!item.IsDir)
                        throw new PathException("The directory '" + src + "' cannot be copied - a file with the same name exists.");

                    string fullDestPath = srcEndsWithSeparator ? dest : LexicalPath.Combine(dest, Path.GetFileName(src));
                    queue.Enqueue(Tuple.Create(src, fullDestPath));
                    _logger.Debug($"Queueing directory '{src}' [target: '{fullDestPath}']");
                }
                else
                {
                    string fullDestPath = srcEndsWithSeparator ? dest : LexicalPath.Combine(dest, Path.GetFileName(src));
                    queue.Enqueue(Tuple.Create(src, fullDestPath));
                    _logger.Debug($"Queueing directory '{src}' [target: '{fullDestPath}']");
                }

                // Process the queue
                while (queue.Count > 0)
                {
                    var tt = queue.Dequeue();

                    // Process all directory entries. Files are added to the repo directly,
                    // sub directories are queued for further processing.
                    var entries = Directory.EnumerateFileSystemEntries(tt.Item1);

                    // Create the path if it doesn't exist
                    if (tt.Item2.Length > 0 && tt.Item2 != "/")
                    {
                        context.Storage.CreateDirectory(tt.Item2);
                    }
                        
                    foreach (var ee in entries)
                    {
                        FileInfo fi = new FileInfo(ee);
                        if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            // Enque the directory for later processing
                            var destFullPath = LexicalPath.Combine(tt.Item2, Path.GetFileName(ee));
                            queue.Enqueue(Tuple.Create(ee, destFullPath));
                            _logger.Debug($"Queueing directory {ee} [target: '{destFullPath}']");
                        }
                        else
                        {
                            // A file - check size and timestamp
                            var fullDestPath = LexicalPath.Combine(tt.Item2, Path.GetFileName(ee));
                            var destItem = context.Storage.GetItemInfo(fullDestPath);
                            var upload = destItem == null || destItem.Size == null || fi.Length != destItem.Size || destItem.LastWriteTime == null || DateTime.Compare(destItem.LastWriteTime.Value, fi.LastWriteTime) < 0;
                            if(upload)
                            {
                                _logger.Debug($"Adding '{ee}' as '{fullDestPath}'");
                                context.Storage.Upload(ee, fullDestPath, finalizeLocal: false);
                                _logger.Debug($"Added '{ee}' as '{fullDestPath}'");
                            }
                            else
                            {
                                _logger.Debug($"Skipped '{ee}'. The repository file '{fullDestPath}' is newer or the same.");
                            }
                        }
                    }
                }
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

            if(src == "/")
            {
                queue.Enqueue(Tuple.Create(src, dest));
            }
            else if(context.Storage.GetItemInfo(src).IsDir)
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
                    destPath = Path.Combine(destPath, ValidLocalPath(fileName));
                    Directory.CreateDirectory(destPath);
                }

                queue.Enqueue(Tuple.Create(src, destPath));
            }
            else
            {
                throw new NotSupportedException("Only directories are supported currently.");
            }

            // Process the queue
            while (queue.Count > 0)
            {
                var tt = queue.Dequeue();
                var items = context.Storage.ListDirectory(tt.Item1);

                foreach (var item in items)
                {
                    if (item.IsDir)
                    {
                        var destDirPath = Path.Combine(tt.Item2, ValidLocalPath(item.Name));
                        var srcDirPath = LexicalPath.Combine(tt.Item1, item.Name);
                        Directory.CreateDirectory(destDirPath);
                        queue.Enqueue(Tuple.Create(srcDirPath, destDirPath));
                        _logger.Debug($"Queueing directory '{srcDirPath}' [target: '{destDirPath}']");
                    }
                    else
                    {
                        var srcFullPath = LexicalPath.Combine(tt.Item1, item.Name);
                        var destFullPath = Path.Combine(tt.Item2, ValidLocalPath(item.Name));
                        var fileInfo = new FileInfo(destFullPath);
                        var download = !fileInfo.Exists || fileInfo.Length != item.Size || item.LastWriteTime == null || DateTime.Compare(fileInfo.LastWriteTimeUtc, item.LastWriteTime.Value) < 0;
                        if (download)
                        {
                            _logger.Debug($"Copying '{srcFullPath}' to '{destFullPath}'");
                            context.Storage.Download(srcFullPath, destFullPath);
                            File.SetLastWriteTime(destFullPath, item.LastWriteTime.Value);
                            _logger.Debug($"Copied '{srcFullPath}' to '{destFullPath}'");
                        }
                        else
                        {
                            _logger.Debug($"Skipped '{srcFullPath}'. The local file ({destFullPath}) is either newer or the same as the repository file.");
                        }
                    }
                }
            }
        }

        public void Remove(ref Context context, string path)
        {
            try
            {
                context.Storage.RemoveFile(path);
            }
            catch(Exception)
            { }
        }

        public void Move(ref Context context, string src, string dest)
        {
            try
            {
                context.Storage.Move(src, dest);
            }
            catch (Exception)
            { }
        }

        private string ValidLocalPath(string path)
        {
            var sb = new StringBuilder(path.Length);
            for(var i = 0; i < path.Length; ++i)
                sb.Append(_invalidChars.Contains(path[i]) ? '_' : path[i]);
            return sb.ToString();
        }
    }
}
