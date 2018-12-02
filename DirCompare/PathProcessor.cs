namespace DirCompare
{
    public class PathProcessor
    {
        public static string GetConsistentPathWithoutBase(string fullPath, string basePath)
        {
            if (fullPath.Length < 1)
            {
                throw new System.ArgumentException("fullPath can't be empty");
            }
            // To compare the content of the directory we must remove the basePath.
            // It will be different which makes the diff useless.
            // We also need to have consistent path-separators for the same reason.
            var pathWithoutBase = fullPath.Replace(basePath, "");
            pathWithoutBase = EnforceWindowsPathSeparator(pathWithoutBase);
            pathWithoutBase = EnsureNoLeadingSeparator(pathWithoutBase);
            return pathWithoutBase;
        }

        private static string EnforceWindowsPathSeparator(string path)
        {
            return path.Replace("/", "\\");
        }

        private static string EnsureNoLeadingSeparator(string path)
        {
            if (path.StartsWith("\\"))
            {
                return path.Remove(0, 1);
            }
            return path;
        }
    }
}
