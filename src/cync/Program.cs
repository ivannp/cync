using CloudSync.Core;
using ColoredConsole;
using CommandLine;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CloudSync.Tool
{
    class Program
    {
        [Verb("init", HelpText = "Initializes a cync repository.")]
        class InitOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option("compression-level", Default = 5, HelpText = "The compression level [0-9].")]
            public int CompressionLevel { get; set; }

            [Option("max-versions", Default = 4, HelpText = "The maximum number of versions to keep per file.")]
            public int MaxVersions { get; set; }

            [Option('c', "ciphers", Default = "aes", HelpText = "The encryption ciphers used.")]
            public string Ciphers { get; set; }

            [Option('l', "local", Default = null, HelpText = "Initialize a local repository at the given path.")]
            public string Local { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "Initialize a Google Drive repository at the given path.")]
            public string GoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "Initialize a OneDrive repository at the given path.")]
            public string OneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "Initialize a repository over SFTP, at the given path.")]
            public string Sftp { get; set; }

            [Option("sftp-key", Default = null, HelpText = "SFTP key file to use for authentication.")]
            public string SftpKeyPath { get; set; }

            [Option("sftp-user", Default = null, HelpText = "SFTP user to use for authentication.")]
            public string SftpUser { get; set; }

            [Option("sftp-password", Default = null, HelpText = "SFTP password to use for authentication.")]
            public string SftpPassword { get; set; }

            [Option("sftp-host", Default = null, HelpText = "SFTP server.")]
            public string SftpHost { get; set; }

            [Option("sftp-port", Default = null, HelpText = "SFTP port.")]
            public int? SftpPort { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }
        }

        [Verb("keygen", HelpText = "Generate a key.")]
        class KeyGenOptions
        {
            [Option('b', "bits", Default = 256U, HelpText = "The number of bits.")]
            public uint Bits { get; set; }

            [Option('h', "hex", Default = false, HelpText = "Hex output.")]
            public bool Hex { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }
        }

        [Verb("encode", HelpText = "Encode a single local file.")]
        class EncodeOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option("compression-level", Default = 5, HelpText = "The compression level [0-9].")]
            public int CompressionLevel { get; set; }

            [Option('c', "ciphers", Default = "aes", HelpText = "The encryption ciphers used.")]
            public string Ciphers { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }

            [Value(0, Min = 2, Max = 2)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("decode", HelpText = "Decode a single local file.")]
        class DecodeOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('c', "ciphers", Default = "aes", HelpText = "The encryption ciphers used.")]
            public string Ciphers { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }

            [Value(0, Min = 2, Max = 2)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("push", HelpText = "Push files and directories to the repository.")]
        class PushOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "Synchronize to Google Drive path. No encryption, no compression.")]
            public bool PlainGoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "The OneDrive repository to use.")]
            public string OneDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "Synchronize to a OneDrive path. No encryption, no compression.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "Synchronize to a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option("use-checksums", Default = false, HelpText = "Use checksums to verify file differences.")]
            public bool UseChecksums { get; set; }

            [Value(0, Max = 2, Required = false)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("pull", HelpText = "Pulls files and directories from the repository.")]
        class PullOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose mode.")]
            public bool Verbose { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "Synchronize Google Drive path to a local path. No encryption, no compression.")]
            public bool PlainGoogleDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "Synchronize an SFTP path to a local path.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option("use-checksums", Default = false, HelpText = "Use checksums to verify file differences.")]
            public bool UseChecksums { get; set; }

            [Value(0, Max = 2, Required = false)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("list", HelpText = "Lists the content of a repository path.")]
        class ListOptions
        {
            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "List a plain (no encryption, no compression) Google Drive folder.")]
            public bool PlainGoogleDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "List a plain (no encryption, no compression) OneDrive folder.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "List a path in a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option("long", Default = null, HelpText = "Use a long listing format.")]
            public bool Long { get; set; }

            [Value(0, Max = 1, Required = false)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("remove", HelpText = "Removes files or folders from the repository. Folders are removed recursively.")]
        class RemoveOptions
        {
            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "List a plain (no encryption, no compression) Google Drive folder.")]
            public bool PlainGoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "The OneDrive repository to use.")]
            public string OneDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "List a plain (no encryption, no compression) OneDrive folder.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "List a path in a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option('v', "verbose", Default = false, HelpText = "Verbose.")]
            public bool Verbose { get; set; }

            [Option("ignore-errors", Default = false, HelpText = "Ignore errors. Useful when fixing file system problems identified by the verify command.")]
            public bool IgnoreErrors { get; set; }

            [Value(0, Min = 1, Required = true)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("move", HelpText = "Move a file or a folder.")]
        class MoveOptions
        {
            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "List a plain (no encryption, no compression) Google Drive folder.")]
            public bool PlainGoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "The OneDrive repository to use.")]
            public string OneDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "List a plain (no encryption, no compression) OneDrive folder.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "List a path in a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Value(0, Min = 1, Required = true)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("verify", HelpText = "Verify the repository.")]
        class VerifyOptions
        {
            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "List a plain (no encryption, no compression) Google Drive folder.")]
            public bool PlainGoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "The OneDrive repository to use.")]
            public string OneDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "List a plain (no encryption, no compression) OneDrive folder.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "List a path in a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option("repair", Default = null, HelpText = "Attempt to repair identified problems.")]
            public bool Repair { get; set; }
        }

        [Verb("mkdir", HelpText = "Create directories.")]
        class MkdirOptions
        {
            [Option('l', "local", Default = null, HelpText = "The local repository to use.")]
            public string Local { get; set; }

            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('g', "google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "List a plain (no encryption, no compression) Google Drive folder.")]
            public bool PlainGoogleDrive { get; set; }

            [Option('o', "onedrive", Default = null, HelpText = "The OneDrive repository to use.")]
            public string OneDrive { get; set; }

            [Option("plain-onedrive", Default = false, HelpText = "List a plain (no encryption, no compression) OneDrive folder.")]
            public bool PlainOneDrive { get; set; }

            [Option("sftp", Default = null, HelpText = "List a path in a SFTP repository.")]
            public string Sftp { get; set; }

            [Option("token-path", Default = null, HelpText = "The path to store (for re-use) the authentication token.")]
            public string TokenPath { get; set; }

            [Option("parents", Default = null, HelpText = "Create parent directories as needed.")]
            public bool Parents { get; set; }

            [Value(0, Min = 1, Required = true)]
            public IEnumerable<string> Items { get; set; }
        }

        [Verb("test-codec", HelpText = "Tests the coded (encode, decode, compare) on all files in a directory.")]
        class TestCodecOptions
        {
            [Option("key-file", Default = null, HelpText = "The user provided key file.")]
            public string KeyFile { get; set; }

            [Option('c', "ciphers", Default = "aes", HelpText = "The encryption ciphers used.")]
            public string Ciphers { get; set; }

            [Option("compression-level", Default = 5, HelpText = "The compression level [0-9].")]
            public int CompressionLevel { get; set; }

            [Value(0, Min = 1, Required = true)]
            public IEnumerable<string> Items { get; set; }
        }

        static void CmdInit(InitOptions io)
        {
            string dotPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".cync");
            LocalUtils.CreateSecureFolder(dotPath);

            IObjectTree tree = null;

            var context = DefaultContext(io.Verbose);
            context.RepoCfg = new RepoConfig(io.CompressionLevel, io.MaxVersions, io.Ciphers);

            if (io.Local != null)
            {
                context.Storage = new LocalStorage(io.Local);

                tree = new ObjectTree();
            }
            else if (io.GoogleDrive != null)
            {
                string tokenPath = io.TokenPath ?? dotPath;
                context.Storage = new GoogleDriveStorage(io.GoogleDrive, tokenPath);

                tree = new ObjectTree();
            }
            else if (io.OneDrive != null)
            {
                string tokenPath = io.TokenPath ?? dotPath;
                context.Storage = new OneDriveStorage(io.OneDrive, tokenPath);

                tree = new ObjectTree();
            }
            else if (io.Sftp != null)
            {
                context.Storage = new SftpStorage(io.SftpUser, io.SftpPassword, io.SftpKeyPath, io.SftpHost, io.SftpPort, io.Sftp, createRoot: true);

                tree = new ObjectTree();
            }

            var jcfg = new JObject
            {
                ["Repository"] = context.Storage.ToJson()
            };

            if (io.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(io.KeyFile));
                jcfg["KeyFile"] = OSPath.ToSlash(Path.GetFullPath(io.KeyFile));
            }
            else
            {
                context.Key = GetKey();
            }

            context.Verbose = io.Verbose;

            tree.CreateRoot(ref context);

            // Save the re-usable portion of the current context
            SaveContext(jcfg, dotPath);
        }

        static void CmdPush(PushOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(options.Verbose);

            string src = "."; // Current folder by default
            string dest = "/"; // Repository root as default

            var items = new List<string>(options.Items);

            if (items.Count == 1)
            {
                src = items[0];
            }
            else if (items.Count == 2)
            {
                src = items[0];
                dest = items[1];
            }

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainGoogleDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else if (options.OneDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage(options.OneDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainOneDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            context.Verbose = options.Verbose;

            if (options.UseChecksums)
            {
                context.UseChecksums = true;
                jcfg["UseChecksums"] = true;
            }
            else if (jcfg["UseChecksums"] != null && jcfg["UseChecksums"].Value<bool>())
            {
                context.UseChecksums = true;
            }

            tree.Push(ref context, src, dest);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdPull(PullOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(options.Verbose);

            string src;
            string dest = Directory.GetCurrentDirectory(); // By default

            var items = new List<string>(options.Items);

            if (items.Count == 1)
            {
                src = items[0];
            }
            else if (items.Count == 2)
            {
                src = items[0];
                dest = items[1];
            }
            else
            {
                throw new CommandLineException("The 'pull' command requires a source.");
            }

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainGoogleDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage("/", tokenPath);
                // No need to initialize a repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            context.Verbose = options.Verbose;

            tree.Pull(ref context, src, dest);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdList(ListOptions oo)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(false);

            string path = "/";

            var items = new List<string>(oo.Items);

            if (items.Count == 1)
                path = items[0];

            IObjectTree tree = null;

            if (oo.Local != null)
            {
                context.Storage = new LocalStorage(oo.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (oo.GoogleDrive != null)
            {
                string tokenPath = oo.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(oo.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (oo.PlainGoogleDrive)
            {
                string tokenPath = oo.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else if (oo.PlainOneDrive)
            {
                string tokenPath = oo.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (oo.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(oo.KeyFile));
                jcfg["KeyFile"] = oo.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            void itemAction(string str, ItemInfo itemInfo)
            {
                if (itemInfo != null && itemInfo.IsDir)
                    ColorConsole.Write(str.DarkGreen());
                else
                    ColorConsole.Write(str);
            };

            var outputConfig = new OutputConfig
            {
                ItemAction = oo.Long ? (s, i) => ColorConsole.Write(s) : (Action<string, ItemInfo>)itemAction,
                EndOfLineAction = () => ColorConsole.WriteLine(),
                Type = oo.Long ? OutputConfig.OutputType.Long : OutputConfig.OutputType.Default
            };

            tree.List(ref context, path, outputConfig);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdRemove(RemoveOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(options.Verbose);

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainGoogleDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage("/", tokenPath);
                // No need to initialize a repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else if (options.OneDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage(options.OneDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainOneDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            context.IgnoreErrors = options.IgnoreErrors;

            foreach(var item in options.Items)
                tree.Remove(ref context, item);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdMove(MoveOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(false);

            string src;
            string dest = "/";

            var items = new List<string>(options.Items);

            if (items.Count == 1)
            {
                src = items[0];
            }
            else if (items.Count == 2)
            {
                src = items[0];
                dest = items[1];
            }
            else
            {
                throw new CommandLineException("The 'move' command requires requires source and destination.");
            }

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainGoogleDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage("/", tokenPath);
                // No need to initialize a repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else if (options.OneDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage(options.OneDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.PlainOneDrive)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage("/", tokenPath);
                // No need to initialize the repository

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new FileSystemTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            tree.Move(ref context, src, dest);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdVerify(VerifyOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(false);

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.OneDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage(options.OneDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            tree.Verify(ref context, options.Repair);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdMkdir(MkdirOptions options)
        {
            var jcfg = LoadContext(DotPath);

            Context context = DefaultContext(false);

            IObjectTree tree = null;

            if (options.Local != null)
            {
                context.Storage = new LocalStorage(options.Local);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.GoogleDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new GoogleDriveStorage(options.GoogleDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else if (options.OneDrive != null)
            {
                string tokenPath = options.TokenPath ?? DotPath;
                context.Storage = new OneDriveStorage(options.OneDrive, tokenPath);
                context.InitRepoFromStorage();

                jcfg["Repository"] = context.Storage.ToJson();

                tree = new ObjectTree();
            }
            else
            {
                context.Storage = RepoFromJson(jcfg);
                context.InitRepoFromStorage();

                tree = new ObjectTree();
            }

            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
                jcfg["KeyFile"] = options.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }
            else
            {
                context.Key = GetKey();
            }

            foreach (var item in options.Items)
            {
                var path = LexicalPath.Clean(item);
                if (path[0] != '/')
                    path = "/" + path;
                tree.CreateDirectory(ref context, path, options.Parents);
            }

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdEncode(EncodeOptions options)
        {
            Context context = DefaultContext(options.Verbose);

            context.RepoCfg = new RepoConfig(options.CompressionLevel, 1, options.Ciphers);
            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
            }
            else
            {
                context.Key = GetKey();
            }

            var items = options.Items.ToList();

            byte[] hash = new byte[256];
            CodecHelper.EncodeFile(ref context, items[0], items[1], ref hash);
        }

        static void CmdDecode(DecodeOptions options)
        {
            Context context = DefaultContext(options.Verbose);

            // RepoCfg is initialized although it's not used - otherwise crash
            context.RepoCfg = new RepoConfig(0, 1, options.Ciphers);
            if (options.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(options.KeyFile));
            }
            else
            {
                context.Key = GetKey();
            }

            var items = options.Items.ToList();

            byte[] hash = new byte[32];
            byte[] expectedHash = new byte[32];
            CodecHelper.DecodeFile(ref context, items[0], items[1], ref hash, ref expectedHash);
        }

        static void CmdTestCodec(TestCodecOptions io)
        {
            var context = DefaultContext(false);
            context.RepoCfg = new RepoConfig(io.CompressionLevel, 1, io.Ciphers);

            if (io.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(io.KeyFile));
            }
            else
            {
                context.ErrorWriteLine($"--key-file is required.");
                return;
            }

            var queue = new Queue<string>();
            foreach (var item in io.Items)
                queue.Enqueue(item);

            var rng = new RNGCryptoServiceProvider();
            byte[] keys = new byte[context.RepoCfg.Ciphers.Length * 32];
            rng.GetBytes(keys);

            var sourcePath = LocalUtils.GetTempFileName(suffix: "source");
            var encodedPath = LocalUtils.GetTempFileName(suffix: "encoded");
            var decodedPath = LocalUtils.GetTempFileName(suffix: "decoded");

            byte[] hash1 = new byte[32];
            byte[] hash2 = new byte[32];
            byte[] expectedHash = new byte[32];

            var total = 0UL;
            var errors = 0UL;

            var totalBytes = 0UL;

            var files = new Dictionary<string, long>();

            var encodingTotal = 0L;
            var decodingTotal = 0L;

            while (queue.Count > 0)
            {
                var dirPath = queue.Dequeue();
                try
                {
                    foreach (var item in Directory.EnumerateDirectories(dirPath))
                    {
                        queue.Enqueue(item);
                    }
                }
                catch(Exception)
                { }

                try
                {
                    foreach (var item in Directory.EnumerateFiles(dirPath))
                    {
                        try
                        {
                            var size = new FileInfo(item).Length;
                            files.Add(item, size);
                            totalBytes += (ulong)size;
                        }
                        catch (Exception)
                        { }
                    }
                }
                catch(Exception)
                { }
            }

            var cursorVisible = Console.CursorVisible;
            Console.WriteLine();

            var progress = new ProgressBar(totalBytes);
            var processedBytes = 0L;
            var skippedBytes = 0L;
            var encodedBytes = 0L;

            foreach(var kv in files)
            {
                progress.Text = $"Processing '{kv.Key}'";
                progress.Update(processedBytes);

                LocalUtils.TryDeleteFile(sourcePath);
                LocalUtils.TryDeleteFile(encodedPath);
                LocalUtils.TryDeleteFile(decodedPath);

                ++total;

                bool copied = false;
                try
                {
                    File.Copy(kv.Key, sourcePath, true);
                    copied = true;
                }
                catch(Exception)
                {
                    skippedBytes += kv.Value;
                }

                if(copied)
                {
                    // Remove read-only attribute
                    var attributes = File.GetAttributes(sourcePath);
                    File.SetAttributes(sourcePath, attributes & ~FileAttributes.ReadOnly);

                    var sw = Stopwatch.StartNew();
                    CodecHelper.EncodeFile(ref context, sourcePath, encodedPath, ref hash1);
                    encodingTotal += sw.ElapsedMilliseconds;

                    encodedBytes += new FileInfo(encodedPath).Length;

                    sw.Start();
                    CodecHelper.DecodeFile(ref context, encodedPath, decodedPath, ref hash2, ref expectedHash);
                    decodingTotal += sw.ElapsedMilliseconds;
                    var compare = LocalUtils.CompareFiles(sourcePath, decodedPath);

                    if (!compare)
                    {
                        context.ErrorWriteLine("");
                        context.ErrorWriteLine($"Codec produced different original for '{kv.Key}'");
                        ++errors;
                    }
                    else if (!hash1.SequenceEqual(expectedHash))
                    {
                        context.ErrorWriteLine("");
                        context.ErrorWriteLine($"Codec produced different hashes for '{kv.Key}'");
                        ++errors;
                    }
                    else if (!hash1.SequenceEqual(hash2))
                    {
                        context.ErrorWriteLine("");
                        context.ErrorWriteLine($"Codec produced different hashes for '{kv.Key}'");
                        ++errors;
                    }
                }

                processedBytes += kv.Value;
            }

            progress.Text = "";
            progress.Update(processedBytes);

            Console.WriteLine();

            LocalUtils.TryDeleteFile(sourcePath);
            LocalUtils.TryDeleteFile(encodedPath);
            LocalUtils.TryDeleteFile(decodedPath);

            if(errors == 0)
            {
                context.AlwaysWriteLine(string.Format("Successfully processed {0:n0} files", total));
                context.AlwaysWriteLine(string.Format("    {0:n0} total bytes", totalBytes));
                if(skippedBytes > 0)
                    context.AlwaysWriteLine(string.Format("    {0:n0} skipped bytes", skippedBytes));
                var ratio = ((double)totalBytes - skippedBytes) / encodedBytes;
                context.AlwaysWriteLine(string.Format("    {0:n0} encoded bytes ({1:F2}:1 compression ratio)", encodedBytes, ratio));
                var mbs = ((double)totalBytes - skippedBytes) / Math.Pow(1024, 2);
                var secs = (double)encodingTotal / 1000;
                var mbsPerSec = mbs / secs;
                context.AlwaysWriteLine(string.Format("    encoding at {0:F2} MB/s", mbsPerSec));
                secs = (double)decodingTotal / 1000;
                mbsPerSec = mbs / secs;
                context.AlwaysWriteLine(string.Format("    decoding at {0:F2} MB/s", mbsPerSec));
            }
            else
            {
                context.ErrorWriteLine(string.Format("{0:n0} errors in {1:n0} files processed", errors, total));
            }

            Console.CursorVisible = true;
            Console.WriteLine();
        }

        static void CmdKeyGen(KeyGenOptions o)
        {
            byte[] key = new byte[o.Bits / 8];
            CodecDll.GenerateKey(key, o.Bits / 8);
            if(o.Hex)
                Console.WriteLine(BitConverter.ToString(key).Replace("-", string.Empty));
            else
                Console.WriteLine(Convert.ToBase64String(key));
        }

        public static string DotPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".cync"); } }

        static IStorage RepoFromJson(JObject json)
        {
            if (json == null) return null;
            var repo = json["Repository"];
            if (repo == null) return null;
            var type = repo["Type"].ToString().ToLower();
            IStorage res = null;
            switch (type)
            {
                case "local":
                    res = new LocalStorage(repo["Path"].ToString());
                    break;

                case "googledrive":
                {
                    string tokenPath = repo["Store"] != null ? repo["Store"].ToString() : DotPath;
                    res = new GoogleDriveStorage(repo["Path"].ToString(), tokenPath);
                    break;
                }

                case "onedrive":
                {
                    string tokenPath = repo["Store"] != null ? repo["Store"].ToString() : DotPath;
                    res = new OneDriveStorage(repo["Path"].ToString(), tokenPath);
                    break;
                }

                case "sftp":
                    res = new SftpStorage(
                        user: repo["User"].Value<string>(),
                        password: repo["Password"]?.Value<string>(),
                        keyPath: repo["KeyPath"]?.Value<string>(),
                        host: repo["Host"].Value<string>(),
                        port: repo["Port"]?.Value<int>(),
                        rootPath: repo["Path"].Value<string>());
                    break;

                default:
                    throw new Exception($"Repository type {type} is not supported. Supported repositories are: local, googledrive, onedrive, sftp.");
            }
            return res;
        }

        static void SaveContext(JObject jcfg, string folderPath)
        {
            // Merge with any previous context
            var path = Path.Combine(folderPath, "saved-context.json");
            if (File.Exists(path))
            {
                var jsaved = JObject.Parse(File.ReadAllText(path));
                jsaved.Merge(jcfg, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                jcfg = jsaved;
            }
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(path, jcfg.ToString());
        }

        static JObject LoadContext(string folderPath)
        {
            var path = Path.Combine(folderPath, "saved-context.json");
            if (File.Exists(path))
            {
                return JObject.Parse(File.ReadAllText(path));
            }
            return new JObject();
        }

        static Context DefaultContext(bool verbose)
        {
            return new Context
            {
                AlwaysWriteLine = (s) => ColorConsole.WriteLine(s),
                ErrorWriteLine = (s) => ColorConsole.WriteLine(s.Red()),
                InfoWriteLine = (verbose ? (Action<string>)((s) => ColorConsole.WriteLine(s.Gray())) : (s) => {})
            };
        }

        static string GetPassword()
        {
            Console.Write("Password: ");
            var result = new StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (result.Length > 0)
                    {
                        result.Remove(result.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    result.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }
            return result.ToString();
        }

        static byte [] GetKey()
        {
            var password = GetPassword();
            var hash = new byte[32];
            var encoding = new UTF8Encoding();

            CodecHelper.ComputeDataHash(encoding.GetBytes(password), ref hash);
            return hash;
        }

        static void Main(string[] args)
        {
            // Show unicode characters
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                Parser.Default.ParseArguments<InitOptions, KeyGenOptions, EncodeOptions, DecodeOptions, PushOptions, PullOptions, ListOptions, RemoveOptions, MoveOptions, VerifyOptions, MkdirOptions, TestCodecOptions>(args)
                    .WithParsed<InitOptions>(opts => CmdInit(opts))
                    .WithParsed<KeyGenOptions>(opts => CmdKeyGen(opts))
                    .WithParsed<EncodeOptions>(opts => CmdEncode(opts))
                    .WithParsed<DecodeOptions>(opts => CmdDecode(opts))
                    .WithParsed<PushOptions>(opts => CmdPush(opts))
                    .WithParsed<PullOptions>(opts => CmdPull(opts))
                    .WithParsed<ListOptions>(opts => CmdList(opts))
                    .WithParsed<RemoveOptions>(opts => CmdRemove(opts))
                    .WithParsed<MoveOptions>(opts => CmdMove(opts))
                    .WithParsed<VerifyOptions>(opts => CmdVerify(opts))
                    .WithParsed<MkdirOptions>(opts => CmdMkdir(opts))
                    .WithParsed<TestCodecOptions>(opts => CmdTestCodec(opts))
                    .WithNotParsed(errs => { foreach (var e in errs) ColorConsole.WriteLine($"cync encountered a fatal error: {e.ToString()}".Red()); });
            }
            catch (Exception ee)
            {
                ColorConsole.WriteLine($"cync encountered a fatal error: {ee.ToString()}".Red());
                if(ee.InnerException != null)
                    ColorConsole.WriteLine($"cync encountered a fatal error: {ee.ToString()}".Red());
            }
        }
    }
}
