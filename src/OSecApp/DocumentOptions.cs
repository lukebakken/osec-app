namespace OSecApp
{
    using System;

    public class DocumentOptions
    {
        private readonly string content;
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
    }
}
