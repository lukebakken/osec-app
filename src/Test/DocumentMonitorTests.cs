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
            var evt = new ManualResetEvent(false);
            var cts = new CancellationTokenSource();

            var o1 = new DocumentOptions("foo bar", "test-doc");
            var d1 = new Document(o1);
            var o2 = new DocumentOptions("baz bat", "test-doc");
            var d2 = new Document(o2);

            using (var idx = new DocumentIndexer())
            {
                using (var pdm = new PendingDocumentManager())
                {
                    using (var dm = new DocumentMonitor(cts, pdm, idx))
                    {
                        bool handled = false;

                        EventHandler<DocumentEventArgs> hdlr = (object s, DocumentEventArgs e) =>
                        {
                            handled = true;
                            Assert.Equal(d1, e.Document); // NB: won't be same object ref
                            evt.Set();
                        };
                        dm.DocumentReplaced += hdlr;

                        pdm.Enqueue(d1);
                        pdm.Enqueue(d2);

                        Assert.True(evt.WaitOne(TimeSpan.FromSeconds(5)));
                        Assert.True(handled);

                        dm.DocumentReplaced -= hdlr;
                        cts.Cancel();
                    }
                }
            }
        }
    }
}
