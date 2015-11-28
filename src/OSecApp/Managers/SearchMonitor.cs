namespace OSecApp.Managers
{
    using System;
    using System.Threading;
    using Models;

    public class SearchMonitor : Monitor<Search, PendingSearchManager>
    {
        public SearchMonitor(CancellationTokenSource cts, PendingSearchManager itemManager)
            : base(cts, itemManager)
        {
        }

        public event EventHandler<SearchEventArgs> SearchAdded;
        public event EventHandler<SearchEventArgs> SearchReplaced;

        protected override void OnItemAdded(Search item)
        {
            if (SearchAdded != null)
            {
                SearchAdded(this, new SearchEventArgs(item));
            }
        }

        protected override void OnItemReplaced(Search item)
        {
            if (SearchReplaced != null)
            {
                SearchReplaced(this, new SearchEventArgs(item));
            }
        }
    }
}
