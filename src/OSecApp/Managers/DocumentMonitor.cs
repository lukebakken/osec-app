namespace OSecApp.Managers
{
    using System;
    using System.Threading;
    using Models;

    public class DocumentMonitor : Monitor<Document, PendingDocumentManager>
    {
        private readonly DocumentIndexer documentIndexer;

        public DocumentMonitor(
            CancellationTokenSource cts,
            PendingDocumentManager pendingDocumentManager,
            DocumentIndexer documentIndexer)
            : base(cts, pendingDocumentManager)
        {

            this.documentIndexer = documentIndexer;
            if (this.documentIndexer == null)
            {
                throw new ArgumentNullException("documentIndexer");
            }
        }

        public event EventHandler<DocumentEventArgs> DocumentAdded;
        public event EventHandler<DocumentEventArgs> DocumentReplaced;

        protected override void OnItemAdded(Document item)
        {
            documentIndexer.Add(item);

            if (DocumentAdded != null)
            {
                DocumentAdded(this, new DocumentEventArgs(item));
            }
        }

        protected override void OnItemReplaced(Document item)
        {
            documentIndexer.Replace(item);

            if (DocumentReplaced != null)
            {
                DocumentReplaced(this, new DocumentEventArgs(item));
            }
        }
    }
}
