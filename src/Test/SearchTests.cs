namespace Test
{
    using System;
    using OSecApp.Models;
    using Xunit;

    public class SearchTests
    {
        [Fact]
        public void New_Search_Requires_Search_Term()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Search(null);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Search(string.Empty);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Search("  \t");
            });
        }
    }
}
