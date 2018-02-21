using CloudSync.Core;
using ColoredConsole;
using CommandLine;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace CloudSync.Tool
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

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

            [Option("google-drive", Default = null, HelpText = "Initialize a Google Drive repository at the given path.")]
            public string GoogleDrive { get; set; }

            [Option("onedrive", Default = null, HelpText = "Initialize a OneDrive repository at the given path.")]
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

            [Value(0, Min = 0, Max = 2)]
            public IList<string> Items { get; set; }
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

            [Value(0, Min = 0, Max = 2)]
            public IList<string> Items { get; set; }
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

            [Option("google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
            public string GoogleDrive { get; set; }

            [Option("plain-google-drive", Default = false, HelpText = "Synchronize to Google Drive path. No encryption, no compression.")]
            public bool PlainGoogleDrive { get; set; }

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

            [Option("google-drive", Default = null, HelpText = "The Google Drive repository to use.")]
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

        static void CmdInit(InitOptions io)
        {
            if (io.Verbose)
                LogHelper.MakeConsoleVerbose();

            string dotPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".cync");

            IObjectTree tree = null;

            Context context = new Context { RepoCfg = new RepoConfig(io.CompressionLevel, io.MaxVersions, io.Ciphers) };
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

            tree.CreateRoot(ref context);

            // Save the re-usable portion of the current context
            SaveContext(jcfg, dotPath);
        }

        static void CmdPush(PushOptions oo)
        {
            if (oo.Verbose)
                LogHelper.MakeConsoleVerbose();

            var jcfg = LoadContext(DotPath);

            Context context = new Context { };

            string src = "."; // Current folder by default
            string dest = "/"; // Repository root as default

            var items = new List<string>(oo.Items);

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

            if (oo.UseChecksums)
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

        static void CmdPull(PullOptions oo)
        {
            if (oo.Verbose)
                LogHelper.MakeConsoleVerbose();

            var jcfg = LoadContext(DotPath);

            Context context = new Context { };

            string src;
            string dest = Directory.GetCurrentDirectory(); // By default

            var items = new List<string>(oo.Items);

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

            if (oo.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(oo.KeyFile));
                jcfg["KeyFile"] = oo.KeyFile;
            }
            else if (jcfg["KeyFile"] != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(jcfg["KeyFile"].ToString()));
            }

            tree.Pull(ref context, src, dest);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdList(ListOptions oo)
        {
            var jcfg = LoadContext(DotPath);

            Context context = new Context { };

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
            if (options.Verbose)
                LogHelper.MakeConsoleVerbose();

            var jcfg = LoadContext(DotPath);

            Context context = new Context { };

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

            foreach(var item in options.Items)
                tree.Remove(ref context, item);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdMove(RemoveOptions options)
        {
            if (options.Verbose)
                LogHelper.MakeConsoleVerbose();

            var jcfg = LoadContext(DotPath);

            Context context = new Context { };

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

            tree.Move(ref context, src, dest);

            // Save the current context
            SaveContext(jcfg, DotPath);
        }

        static void CmdEncode(EncodeOptions oo)
        {
            if (oo.Verbose) LogHelper.MakeConsoleVerbose();

            Context context = new Context { };

            context.RepoCfg = new RepoConfig(oo.CompressionLevel, 1, oo.Ciphers);
            if (oo.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(oo.KeyFile));
            }

            byte[] hash = new byte[256];
            CodecHelper.EncodeFile(ref context, oo.Items[0], oo.Items[1], ref hash);
        }

        static void CmdDecode(DecodeOptions oo)
        {
            if (oo.Verbose) LogHelper.MakeConsoleVerbose();

            Context context = new Context { };

            // RepoCfg is initialized although it's not used - otherwise crash
            context.RepoCfg = new RepoConfig(0, 1, oo.Ciphers);
            if (oo.KeyFile != null)
            {
                context.Key = Convert.FromBase64String(File.ReadAllText(oo.KeyFile));
            }

            byte[] hash = new byte[256];
            CodecHelper.DecodeFile(ref context, oo.Items[0], oo.Items[1], ref hash);
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
                    string tokenPath = repo["Store"] != null ? repo["Store"].ToString() : DotPath;
                    res = new GoogleDriveStorage(repo["Path"].ToString(), tokenPath);
                    break;

                case "onedrive":
                    // TODO(ivannp) Implement
                    break;

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

        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<InitOptions, KeyGenOptions, EncodeOptions, DecodeOptions, PushOptions, PullOptions, ListOptions, RemoveOptions, MoveOptions>(args)
                    .WithParsed<InitOptions>(opts => CmdInit(opts))
                    .WithParsed<KeyGenOptions>(opts => CmdKeyGen(opts))
                    .WithParsed<EncodeOptions>(opts => CmdEncode(opts))
                    .WithParsed<DecodeOptions>(opts => CmdDecode(opts))
                    .WithParsed<PushOptions>(opts => CmdPush(opts))
                    .WithParsed<PullOptions>(opts => CmdPull(opts))
                    .WithParsed<ListOptions>(opts => CmdList(opts))
                    .WithParsed<RemoveOptions>(opts => CmdRemove(opts))
                    .WithParsed<MoveOptions>(opts => CmdMove(opts))
                    .WithNotParsed(errs => { foreach (var e in errs) _logger.Fatal($"cync encountered a fatal error: {e.ToString()}"); });
            }
            catch (Exception ee)
            {
                _logger.Fatal(ee, $"cync encountered a fatal error: {ee.ToString()}");
                if(ee.InnerException != null)
                    _logger.Fatal(ee, $"cync encountered a fatal error: {ee.ToString()}");
            }
        }
    }
}
