namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    public abstract class Monitor<TItem, TManager> : IDisposable where TManager : PendingManager<TItem>
    {
        protected readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly TManager itemManager;

        private readonly ISet<TItem> items = new HashSet<TItem>();

        private readonly AutoResetEvent queueEvent = new AutoResetEvent(false);
        private readonly CancellationToken ct;
        private readonly Task monitorTask;

        private volatile bool running = true;
        private bool disposed = false;

        public Monitor(CancellationTokenSource cts, TManager itemManager)
        {
            if (cts == null)
            {
                throw new ArgumentNullException("cts");
            }

            this.itemManager = itemManager;
            if (this.itemManager == null)
            {
                throw new ArgumentNullException("itemManager");
            }

            itemManager.Enqueued += ItemManager_Enqueued;

            ct = cts.Token;
            monitorTask = Task.Run((Action)MonitorTask);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                running = false;
                itemManager.Enqueued -= ItemManager_Enqueued;
                itemManager.Dispose();
                monitorTask.Wait(TimeSpan.FromSeconds(5));
                monitorTask.Dispose();
                disposed = true;
            }
        }

        protected abstract void OnItemReplaced(TItem item);

        protected abstract void OnItemAdded(TItem item);

        private void ItemManager_Enqueued(object sender, EventArgs e)
        {
            queueEvent.Set();
        }

        private void MonitorTask()
        {
            log.Debug("monitor is starting");

            while (running && ct.IsCancellationRequested == false)
            {
                if (queueEvent.WaitOne(TimeSpan.FromMilliseconds(125)))
                {
                    TItem item = default(TItem);
                    do
                    {
                        item = itemManager.Dequeue();
                        if (item == null)
                        {
                            break;
                        }

                        if (items.Contains(item))
                        {
                            // "Replace" action
                            items.Remove(item);
                            items.Add(item);

                            OnItemReplaced(item);
                        }
                        else
                        {
                            items.Add(item);

                            OnItemAdded(item);
                        }
                    }
                    while (item != null);
                }
                else
                {
                    log.Trace("no items enqueued");
                }
            }

            log.Debug("monitor is stopping");
        }
    }
}
