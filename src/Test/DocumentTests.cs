namespace Test
{
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void Documents_Behave_In_HashSet()
        {
            var o1 = new DocumentOptions("foo bar", "test-doc");
            var d1 = new Document(o1);

            var o2 = new DocumentOptions("foo bar", "test-doc");
            var d2 = new Document(o2);

            var hs = new HashSet<Document>();

            Assert.True(hs.Add(d1));
            Assert.False(hs.Add(d2));

            Assert.True(hs.Remove(d2));
            Assert.True(hs.Add(d2));

            Assert.Equal(1, hs.Count);
            var ary = new Document[1];
            hs.CopyTo(ary);
            Assert.Same(d2, ary[0]);
        }
    }
}
