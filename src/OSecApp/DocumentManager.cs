namespace OSecApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class DocumentManager
    {
        public const ushort DefaultCapacity = 128;

        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private readonly Queue<Document> docs = new Queue<Document>(DefaultCapacity);

        public event EventHandler<DocumentEventArgs> DocumentEnqueued;

        public int Count
        {
            get
            {
                sync.EnterReadLock();
                try
                {
                    return docs.Count;
                }
                finally
                {
                    sync.ExitReadLock();
                }
            }
        }

        public void Enqueue(Document document)
        {
            sync.EnterWriteLock();
            try
            {
                docs.Enqueue(document);
            }
            finally
            {
                sync.ExitWriteLock();
            }
            OnDocumentEnqueued(document);
        }

        public Document Dequeue()
        {
            sync.EnterWriteLock();
            try
            {
                return docs.Dequeue();
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        private void OnDocumentEnqueued(Document document)
        {
            if (DocumentEnqueued != null)
            {
                DocumentEnqueued(this, new DocumentEventArgs(document));
            }
        }
    }
}
