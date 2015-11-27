namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Models;

    public class DocumentIndexer : IDisposable
    {
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private readonly IDictionary<string, IList<Document>> index = new Dictionary<string, IList<Document>>();

        public void Add(Document document)
        {
            string[] words = document.Content.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            sync.EnterWriteLock();
            try
            {
                foreach (string w in words)
                {
                    IList<Document> docs;
                    if (index.TryGetValue(w, out docs))
                    {
                        docs.Add(document);
                    }
                    else
                    {
                        index[w] = new List<Document> { document };
                    }
                }
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }

        public void Search(Search search)
        {
            sync.EnterReadLock();
            try
            {
                IList<Document> docs;
                if (index.TryGetValue(search.Term, out docs))
                {
                    search.Matches = new List<Document>(docs);
                }
            }
            finally
            {
                sync.ExitReadLock();
            }
        }

        public void Dispose()
        {
            sync.Dispose();
        }
    }
}
