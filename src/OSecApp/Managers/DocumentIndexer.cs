namespace OSecApp.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Models;

    public class DocumentIndexer : IDisposable
    {
        private readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private readonly IDictionary<string, ISet<Document>> index = new Dictionary<string, ISet<Document>>();

        public void Add(Document document)
        {
            DoAdd(document, isReplace: false);
        }

        public void Replace(Document document)
        {
            DoAdd(document, isReplace: true);
        }

        public void Search(Search search)
        {
            sync.EnterReadLock();
            try
            {
                ISet<Document> docs;
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

        private void DoAdd(Document document, bool isReplace)
        {
            string[] words = document.Content.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            sync.EnterWriteLock();
            try
            {
                if (isReplace)
                {
                    // TODO: this could be sped up by a reverse index of words-by-document
                    foreach (string w in index.Keys)
                    {
                        index[w].Remove(document);
                    }
                }

                foreach (string w in words)
                {
                    ISet<Document> docs;
                    if (index.TryGetValue(w, out docs))
                    {
                        docs.Add(document);
                    }
                    else
                    {
                        index[w] = new HashSet<Document> { document };
                    }
                }
            }
            finally
            {
                sync.ExitWriteLock();
            }
        }
    }
}
