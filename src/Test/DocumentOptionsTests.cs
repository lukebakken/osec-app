namespace Test
{
    using System;
    using OSecApp.Models;
    using Xunit;

    public class DocumentOptionsTests
    {
        [Fact]
        public void New_DocumentOptions_Argument_Validation()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DocumentOptions((string)null, null);
            });
        }
    }
}
