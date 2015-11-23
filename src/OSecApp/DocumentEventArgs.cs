namespace OSecApp
{
    using System;

    public class DocumentEventArgs : EventArgs
    {
        private readonly Document document;

        public DocumentEventArgs(Document document)
        {
            this.document = document;
            if (this.document == null)
            {
                throw new ArgumentNullException("document", Properties.Resources.DocumentEventArgs_DocumentRequiredException);
            }
        }

        public Document Document
        {
            get
            {
                return document;
            }
        }
    }
}
