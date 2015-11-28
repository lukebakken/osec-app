namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using OSecApp.Managers;
    using OSecApp.Models;
    using Xunit;

    public class PendingDocumentManagerTests
    {
        [Fact]
        public void Can_Add_From_Multiple_Threads()
        {
            ushort c = PendingDocumentManager.DefaultCapacity * 2;
            int enqueued = 0;

            var m = new PendingDocumentManager();
            m.Enqueued += (s, e) =>
            {
                Assert.Same(m, s);
                Interlocked.Increment(ref enqueued);
            };

            var tasks = new Task[c];
            var o = new DocumentOptions("test");

            for (ushort i = 0; i < c; ++i)
            {
                Action a = () =>
                {
                    var d = new Document(o);
                    m.Enqueue(d);
                };

                tasks[i] = Task.Run(a);
            }

            Assert.True(Task.WaitAll(tasks, TimeSpan.FromSeconds(1)));

            Assert.Equal(c, m.Count);
            Assert.Equal(c, enqueued);

            var docs = new Dictionary<Document, ushort>();

            for (ushort i = 0; i < c; ++i)
            {
                Task t = tasks[i];
                Assert.True(t.IsCompleted);
                Assert.False(t.IsFaulted);
                Assert.False(t.IsCanceled);

                Document d = m.Dequeue();
                docs.Add(d, i);
            }

            Assert.Equal(c, docs.Count);
        }
    }
}
