namespace OSecApp.ViewModels
{
    public class DocumentViewModel
    {
        private const string checkMark = "\u2713";
        private const string ballotX = "\u2717";

        public string Name
        {
            get; set;
        }

        public string Content
        {
            get; set;
        }

        public string IsMatchChar
        {
            get
            {
                if (IsMatch)
                {
                    return checkMark;
                }
                else
                {
                    return ballotX;
                }
            }
        }

        public bool IsMatch
        {
            get; set;
        }
    }
}
