namespace Test
{
    using System;
    using System.IO;
    using System.Reflection;
    using NUnit.Framework;
    using OSecApp;

    [TestFixture, UnitTest]
    public class DocumentTests
    {
        [Test]
        public void New_Document_Requires_Options()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Document(null);
            });
        }

        [Test]
        public void New_Document_Without_Name_Generates_One()
        {
            var o = new DocumentOptions("foo bar baz");
            var d = new Document(o);
            Assert.True(d.Name.StartsWith(Document.GeneratedNamePrefix));
        }

        [Test]
        public void New_Document_Without_Name_Uses_File_Name()
        {
            var l = Assembly.GetAssembly(this.GetType()).Location;
            var fi = new FileInfo(l);
            Assert.True(fi.Exists);
            var o = new DocumentOptions(fi);
            var d = new Document(o);
            Assert.AreEqual(fi.Name, d.Name);
        }

        [Test]
        public void Documents_With_Same_Name_Are_Equal()
        {
            var o = new DocumentOptions("foo bar baz", "TestDoc");
            var d1 = new Document(o);
            var d2 = new Document(o);

            Assert.AreEqual(d1, d2);
            Assert.AreNotSame(d1, d2);
        }
    }
}
