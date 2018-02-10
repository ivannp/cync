using System.IO;
using System.Text;

namespace CloudSync.Core
{
    public sealed class OSPath
    {
        // Go's path.filepath.VolumeName function
        //
        // VolumeName returns leading volume name.
        // Given "C:\foo\bar" it returns "C:" on Windows.
        // Given "\\host\share\foo" it returns "\\host\share".
        // On other platforms it returns "".
        public string VolumeName(string path)
        {
            return path.Substring(0, VolumeNameLength(path));
        }

        public static bool IsPathSeparator(char cc)
        {
            return cc == Path.AltDirectorySeparatorChar || cc == Path.DirectorySeparatorChar;
        }

        private static bool IsSlash(char cc)
        {
            return cc == '/' || cc == '\\';
        }

        // Go's path.filepath.ToSlash function
        //
        // ToSlash returns the result of replacing each separator character
        // in path with a slash ('/') character. Multiple separators are
        // replaced by multiple slashes.
        public static string ToSlash(string path)
        {
            if (Path.DirectorySeparatorChar == '/') return path;
            return path.Replace(Path.DirectorySeparatorChar, '/');
        }

        // Go's path.filepath.FromSlash function
        //
        // FromSlash returns the result of replacing each slash ('/') character
        // in path with a separator character. Multiple slashes are replaced
        // by multiple separators.
        public static string FromSlash(string path)
        {
            if (Path.DirectorySeparatorChar == '/') return path;
            return path.Replace('/', Path.DirectorySeparatorChar);
        }

        private static int VolumeNameLength(string path)
        {
            if (path.Length < 2) return 0;

            // With drive letter
            char cc = path[0];
            if (path[1] == ':' && ('a' <= cc && cc <= 'z' || 'A' <= cc && cc <= 'Z')) return 2;

            var ll = path.Length;
            // is it UNC? https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx
            if (ll > 4 && IsSlash(path[0]) && IsSlash(path[1]) && !IsSlash(path[2]) && path[2] != '.')
            {
                // first, leading `\\` and next shouldn't be `\`. its server name.
                for (var nn = 3; nn < ll - 1; ++nn)
                {
                    // second, next '\' shouldn't be repeated.
                    if (IsSlash(path[nn]))
                    {
                        ++nn;
                        // third, following something characters. its share name.
                        if (!IsSlash(path[nn]))
                        {
                            if (path[nn] == '.') break;
                        }

                        for (; nn < ll; ++nn)
                        {
                            if (IsSlash(path[nn]))
                            {
                                break;
                            }
                        }
                        return nn;
                    }
                    break;
                }
            }
            return 0;
        }

        // Go's path.filepath.Clean function
        //
        // Follows the design of Go's path.filepath.Clen method. Assumes that the
        // input path is unix style path.
        //
        // See Go's source code, which also cites:
        //      Rob Pike, "Lexical File Names in Plan 9 or Getting Dot-Dot Right"
        // https://9p.io/sys/doc/lexnames.html
        public static string Clean(string path)
        {
            if (path == null || path.Length == 0) return ".";

            var originalPath = path;
            var volumnLength = VolumeNameLength(path);
            if (volumnLength > 2)
            {
                throw new PathException(string.Format("UNC paths (like {0}) are not supported.", originalPath));
            }

            path = path.Substring(volumnLength);

            if (path.Length == 0)
            {
                if (volumnLength > 1 && originalPath[1] != ':')
                {
                    // Should be UNC
                    return FromSlash(originalPath);
                }
            }

            StringBuilder sb = new StringBuilder();
            var rooted = IsPathSeparator(path[0]);
            int dotDot;
            int ii;
            if (rooted)
            {
                sb.Append('/');
                ii = 1;
                dotDot = 1;
            }
            else
            {
                ii = 0;
                dotDot = 0;
            }
            while (ii < path.Length)
            {
                if (IsPathSeparator(path[ii]))
                {
                    // Empty path component
                    ++ii;
                }
                else if (path[ii] == '.' && ((ii + 1) == path.Length || IsPathSeparator(path[ii + 1])))
                {
                    // '.' component. Skip.
                    ++ii;

                }
                else if (path[ii] == '.' && path[ii + 1] == '.' && ((ii + 2) == path.Length || IsPathSeparator(path[ii + 2])))
                {
                    // '..' component. Remove to the last seaparator.
                    ii += 2;
                    if (sb.Length > dotDot)
                    {
                        var jj = sb.Length - 1;
                        while (jj > dotDot && !IsPathSeparator(sb[jj]))
                        {
                            --jj;
                        }
                        sb.Remove(jj, sb.Length - jj);
                    }
                    else if (!rooted)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("/");
                        }

                        sb.Append("..");
                        dotDot = sb.Length;
                    }
                }
                else
                {
                    // Real path element, add slash if needed.
                    if ((rooted && sb.Length != 1) || (!rooted && sb.Length != 0))
                    {
                        sb.Append("/");
                    }
                    for (; ii < path.Length && !IsPathSeparator(path[ii]); ++ii)
                    {
                        sb.Append(path[ii]);
                    }
                }
            }

            return originalPath.Substring(0, volumnLength) + sb.ToString();
        }

        // Go's path.filepath.Dir function
        //
        // Dir returns all but the last element of path, typically the path's directory.
        // After dropping the final element, Dir calls Clean on the path and trailing
        // slashes are removed.
        // If the path is empty, Dir returns ".".
        // If the path consists entirely of separators, Dir returns a single separator.
        // The returned path does not end in a separator unless it is the root directory.
        public string GetFileName(string path)
        {
            var vol = VolumeName(path);
            var ii = path.Length - 1;
            while (ii >= vol.Length && !IsPathSeparator(path[ii]))
            {
                --ii;
            }

            var res = Clean(path.Substring(vol.Length, ii + 1 - vol.Length));
            return vol + res;
        }
    }
}
