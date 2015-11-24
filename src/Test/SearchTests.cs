namespace Test
{
    using System;
    using NUnit.Framework;
    using OSecApp;

    [TestFixture, UnitTest]
    public class SearchTests
    {
        [Test]
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
