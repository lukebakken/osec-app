namespace Test
{
    using System;
    using System.Threading;
    using OSecApp.Managers;
    using OSecApp.Models;
    using Xunit;

    public class DocumentMonitorTests
    {
        [Fact]
        public void DocumentMonitor_Tracks_Documents_And_Notifies_Of_Replacements()
        {
            var cts = new CancellationTokenSource();

            var o1 = new DocumentOptions("foo bar", "test-doc");
            var d1 = new Document(o1);
            var o2 = new DocumentOptions("baz bat", "test-doc");
            var d2 = new Document(o2);

            using (var pdm = new PendingDocumentManager())
            {
                using (var dm = new DocumentMonitor(cts, pdm))
                {
                    EventHandler<DocumentEventArgs> hdlr = (object s, DocumentEventArgs e) =>
                    {
                        Assert.Same(d1, e.Document);
                    };
                    dm.DocumentReplaced += hdlr;

                    pdm.Enqueue(d1);
                    pdm.Enqueue(d2);

                    dm.DocumentReplaced -= hdlr;
                }
            }
        }
    }
}
