namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Models;

    public class PendingDocumentManager : IDisposable
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

        public void Enqueue(string name, string content)
        {
            var opts = new DocumentOptions(content, name);
            Enqueue(new Document(opts));
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
                Document d = null;

                if (docs.Count > 0)
                {
                    d = docs.Dequeue();
                }

                return d;
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            sync.Dispose();
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
