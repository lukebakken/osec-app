namespace OSecApp.Managers
{
    using Models;

    public class PendingSearchManager : PendingManager<Search>
    {
        protected override Search GetItemFor(string name, string content)
        {
            return new Search(name);
        }
    }
}
