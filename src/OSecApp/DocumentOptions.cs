namespace OSecApp
{
    using System;
    using System.IO;

    public class DocumentOptions
    {
        private readonly string contents;
        private readonly FileInfo file;
        private readonly string name;

        private DocumentOptions(string name)
        {
            this.name = name;
        }

        public DocumentOptions(string contents, string name = null)
            : this(name)
        {
            this.contents = contents;
            if (this.contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.DocumentOptions_ContentIsRequiredException);
            }
        }

        public DocumentOptions(FileInfo file, string name = null)
            : this(name)
        {
            this.file = file;
            if (file == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.DocumentOptions_FileIsRequiredException);
            }

            if (file.Exists == false)
            {
                throw new FileNotFoundException(Properties.Resources.DocumentOptions_FileNotFoundException, file.FullName);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Contents
        {
            get
            {
                return contents;
            }
        }

        public FileInfo File
        {
            get
            {
                return file;
            }
        }
    }
}
