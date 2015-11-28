namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using NLog;

    public class DocumentMonitor : IDisposable
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly PendingDocumentManager pendingDocumentManager;
        private readonly DocumentIndexer documentIndexer;

        private readonly ISet<Document> documents = new HashSet<Document>();

        private readonly AutoResetEvent queueEvent = new AutoResetEvent(false);
        private readonly CancellationToken ct;
        private readonly Task monitorTask;

        private volatile bool running = true;

        public DocumentMonitor(
            CancellationTokenSource cts,
            PendingDocumentManager pendingDocumentManager,
            DocumentIndexer documentIndexer)
        {
            if (cts == null)
            {
                throw new ArgumentNullException("cts");
            }

            this.pendingDocumentManager = pendingDocumentManager;
            if (this.pendingDocumentManager == null)
            {
                throw new ArgumentNullException("pendingDocumentManager");
            }

            this.documentIndexer = documentIndexer;
            if (this.documentIndexer == null)
            {
                throw new ArgumentNullException("documentIndexer");
            }

            pendingDocumentManager.DocumentEnqueued += PendingDocumentManager_DocumentEnqueued;

            ct = cts.Token;
            monitorTask = Task.Run((Action)Monitor);
        }

        public event EventHandler<DocumentEventArgs> DocumentAdded;
        public event EventHandler<DocumentEventArgs> DocumentReplaced;

        private void PendingDocumentManager_DocumentEnqueued(object sender, DocumentEventArgs e)
        {
            queueEvent.Set();
        }

        private void Monitor()
        {
            log.Debug("monitor is starting");

            while (running && ct.IsCancellationRequested == false)
            {
                if (queueEvent.WaitOne(TimeSpan.FromMilliseconds(125)))
                {
                    Document d = null;
                    do
                    {
                        d = pendingDocumentManager.Dequeue();
                        if (d == null)
                        {
                            break;
                        }

                        if (documents.Contains(d))
                        {
                            // "Replace" action
                            documents.Remove(d);
                            documents.Add(d);

                            documentIndexer.Replace(d);

                            OnDocumentReplaced(d);
                        }
                        else
                        {
                            documents.Add(d);
                            documentIndexer.Add(d);

                            OnDocumentAdded(d);
                        }
                    }
                    while (d != null);
                }
                else
                {
                    log.Trace("no documents enqueued");
                }
            }

            log.Debug("monitor is stopping");
        }

        private void OnDocumentAdded(Document document)
        {
            if (DocumentAdded != null)
            {
                DocumentAdded(this, new DocumentEventArgs(document));
            }
        }

        private void OnDocumentReplaced(Document document)
        {
            if (DocumentReplaced != null)
            {
                DocumentReplaced(this, new DocumentEventArgs(document));
            }
        }

        public void Dispose()
        {
            running = false;
            pendingDocumentManager.DocumentEnqueued -= PendingDocumentManager_DocumentEnqueued;
            pendingDocumentManager.Dispose();
            monitorTask.Wait(TimeSpan.FromSeconds(5));
            monitorTask.Dispose();
        }
    }
}
