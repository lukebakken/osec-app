namespace Test
{
    using System;
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
    }
}
