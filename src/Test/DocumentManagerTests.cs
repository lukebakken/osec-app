namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using OSecApp;

    [TestFixture, UnitTest]
    public class DocumentManagerTests
    {
        [Test]
        public void Can_Add_From_Multiple_Threads()
        {
            ushort c = DocumentManager.DefaultCapacity * 2;
            int enqueued = 0;

            var m = new DocumentManager();
            m.DocumentEnqueued += (s, e) =>
            {
                Assert.AreSame(m, s);
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

            Assert.AreEqual(c, m.Count);
            Assert.AreEqual(c, enqueued);

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

            Assert.AreEqual(c, docs.Count);
        }

        [Test]
        public void Returns_Matching_Documents_When_Executing_Search()
        {
        }
    }
}
