namespace Test
{
    using System;
    using NUnit.Framework;
    using OSecApp;

    [TestFixture, UnitTest]
    public class DocumentOptionsTests
    {
        [Test]
        public void New_DocumentOptions_Argument_Validation()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DocumentOptions((string)null, null);
            });
        }
    }
}
