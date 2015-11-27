namespace OSecApp.Managers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using NLog;

    public class DocumentMonitor : IDisposable
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly PendingDocumentManager pendingDocumentManager;
        private readonly AutoResetEvent queueEvent = new AutoResetEvent(false);
        private readonly CancellationToken ct;
        private readonly Task monitorTask;

        private volatile bool running = true;

        public DocumentMonitor(CancellationTokenSource cts, PendingDocumentManager pendingDocumentManager)
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

            pendingDocumentManager.DocumentEnqueued += PendingDocumentManager_DocumentEnqueued;

            ct = cts.Token;
            monitorTask = Task.Run((Action)Monitor);
        }

        public event EventHandler<DocumentEventArgs> DocumentReplaced;

        private void PendingDocumentManager_DocumentEnqueued(object sender, DocumentEventArgs e)
        {
            queueEvent.Set();
        }

        private void Monitor()
        {
            while (running && ct.IsCancellationRequested == false)
            {
                if (queueEvent.WaitOne(TimeSpan.FromSeconds(1)))
                {
                    Document d = null;
                    do
                    {
                        d = pendingDocumentManager.Dequeue();
                    }
                    while (d != null);
                }
                else
                {
                    log.Debug("no documents enqueued");
                }
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
            pendingDocumentManager.Dispose();
            monitorTask.Wait(TimeSpan.FromSeconds(5));
            monitorTask.Dispose();
        }
    }
}
