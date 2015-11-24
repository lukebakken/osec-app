namespace OSecApp
{
    using System;

    public class Search
    {
        private readonly string searchTerm;

        public Search(string searchTerm)
        {
            this.searchTerm = searchTerm;
            if (string.IsNullOrWhiteSpace(this.searchTerm))
            {
                throw new ArgumentNullException("searchTerm", Properties.Resources.Search_SearchTermRequiredException);
            }
        }
    }
}
