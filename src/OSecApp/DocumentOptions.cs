namespace OSecApp
{
    using System;
    using System.IO;

    public class DocumentOptions
    {
        private readonly string content;
        private readonly FileInfo file;
        private readonly string name;

        private DocumentOptions(string name)
        {
            this.name = name;
        }

        public DocumentOptions(string content, string name = null)
            : this(name)
        {
            this.content = content;
            if (this.content == null)
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

        public string Content
        {
            get
            {
                return content;
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
