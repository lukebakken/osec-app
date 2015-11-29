namespace OSecApp.Managers
{
    using System;
    using System.Threading;
    using Models;

    public class SearchMonitor : Monitor<Search, PendingSearchManager>
    {
        private readonly DocumentIndexer documentIndexer;

        public SearchMonitor(
            CancellationTokenSource cts,
            PendingSearchManager itemManager,
            DocumentIndexer documentIndexer)
            : base(cts, itemManager)
        {
            this.documentIndexer = documentIndexer;
            if (this.documentIndexer == null)
            {
                throw new ArgumentNullException("documentIndexer");
            }
        }

        public event EventHandler<SearchEventArgs> SearchComplete;

        protected override void OnItemAdded(Search search)
        {
            documentIndexer.Search(search);

            if (SearchComplete != null)
            {
                SearchComplete(this, new SearchEventArgs(search));
            }
        }
    }
}
