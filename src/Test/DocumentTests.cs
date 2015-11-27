namespace Test
{
    using System;
    using OSecApp.Models;
    using Xunit;

    public class DocumentTests
    {
        [Fact]
        public void New_Document_Requires_Options()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Document(null);
            });
        }

        [Fact]
        public void New_Document_Without_Name_Generates_One()
        {
            var o = new DocumentOptions("foo bar baz");
            var d = new Document(o);
            Assert.True(d.Name.StartsWith(Document.GeneratedNamePrefix));
        }

        [Fact]
        public void Documents_With_Same_Name_Are_Equal()
        {
            var o = new DocumentOptions("foo bar baz", "TestDoc");
            var d1 = new Document(o);
            var d2 = new Document(o);

            Assert.Equal(d1, d2);
            Assert.NotSame(d1, d2);
        }
    }
}
