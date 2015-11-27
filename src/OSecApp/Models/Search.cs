namespace OSecApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Search
    {
        private readonly string term;

        public Search(string term)
        {
            this.term = term;
            if (string.IsNullOrWhiteSpace(this.term))
            {
                throw new ArgumentNullException("searchTerm", Properties.Resources.Search_SearchTermRequiredException);
            }
        }

        public string Term
        {
            get
            {
                return term;
            }
        }

        public IEnumerable<Document> Matches
        {
            get; set;
        }
    }
}
