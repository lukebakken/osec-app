namespace OSecApp.Managers
{
    using Models;

    public class PendingDocumentManager : PendingManager<Document>
    {
        protected override Document GetItemFor(string name, string content)
        {
            var opts = new DocumentOptions(content, name);
            return new Document(opts);
        }
    }
}
