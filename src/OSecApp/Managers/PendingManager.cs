namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class PendingManager<T> : IDisposable
    {
        public const ushort DefaultCapacity = 128;

        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private readonly Queue<T> items = new Queue<T>(DefaultCapacity);

        public event EventHandler<EventArgs> Enqueued;

        public int Count
        {
            get
            {
                sync.EnterReadLock();
                try
                {
                    return items.Count;
                }
                finally
                {
                    sync.ExitReadLock();
                }
            }
        }

        protected abstract T GetItemFor(string name, string content);

        public void Enqueue(string name, string content)
        {
            Enqueue(GetItemFor(name, content));
        }

        public void Enqueue(T item)
        {
            sync.EnterWriteLock();
            try
            {
                items.Enqueue(item);
            }
            finally
            {
                sync.ExitWriteLock();
            }
            OnItemEnqueued(item);
        }

        public T Dequeue()
        {
            sync.EnterWriteLock();
            try
            {
                T item = default(T);

                if (items.Count > 0)
                {
                    item = items.Dequeue();
                }

                return item;
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

        private void OnItemEnqueued(T item)
        {
            if (Enqueued != null)
            {
                Enqueued(this, new EventArgs());
            }
        }
    }
}
