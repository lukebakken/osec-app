namespace OSecApp
{
    using System;
    using System.IO;
    using System.Threading;

    public class DocumentOptions
    {
        public const string DefaultPrefix = "Document-";
        private static int idx = 0;

        private readonly string contents;
        private readonly string name;

        public DocumentOptions(string contents, string name = null)
        {
            this.contents = contents;
            if (this.contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.DocumentOptions_ContentIsRequiredException);
            }

            this.name = MakeName(name);
        }

        public DocumentOptions(FileInfo file, string name = null)
        {
            if (file == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.DocumentOptions_FileIsRequiredException);
            }

            if (file.Exists == false)
            {
                throw new FileNotFoundException(Properties.Resources.DocumentOptions_FileNotFoundException, file.FullName);
            }

            this.name = MakeName(name, file);
        }

        private static string MakeName(string name)
        {
            if (name == null)
            {
                name = string.Format("{0}{1}", DefaultPrefix, Interlocked.Increment(ref idx));
            }

            return name;
        }

        private static string MakeName(string name, FileInfo file)
        {
            return name;
        }
    }
}
