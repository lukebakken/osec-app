namespace Test
{
    using NUnit.Framework;
    using OSecApp.Managers;
    using OSecApp.Models;

    [TestFixture, UnitTest]
    public class DocumentManagerTests
    {
        [Test]
        public void Adding_Document_Indexes_Words()
        {
            var o = new DocumentOptions("foo bar baz bat", "document-1");
            var d = new Document(o);

            var m = new DocumentManager();
            m.Add(d);

            var s = new Search("baz");
            m.Search(s);

            Assert.NotNull(s.Matches);
            CollectionAssert.Contains(s.Matches, d);
        }
    }
}
