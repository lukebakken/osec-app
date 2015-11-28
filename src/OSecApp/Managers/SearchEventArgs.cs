namespace OSecApp.Managers
{
    using System;
    using Models;

    public class SearchEventArgs : EventArgs
    {
        private readonly Search search;

        public SearchEventArgs(Search search)
        {
            this.search = search;
            if (this.search == null)
            {
                throw new ArgumentNullException("document", Properties.Resources.SearchEventArgs_SearchRequiredException);
            }
        }

        public Search Search
        {
            get
            {
                return search;
            }
        }
    }
}
