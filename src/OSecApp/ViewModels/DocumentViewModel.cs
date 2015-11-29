namespace OSecApp.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private string content;
        private char isMatchChar = Constants.BallotX;

        public string Name
        {
            get; set;
        }

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                SetProperty(ref content, value);
            }
        }

        public char IsMatchChar
        {
            get
            {
                return isMatchChar;
            }
            set
            {
                SetProperty(ref isMatchChar, value);
            }
        }

        public bool IsMatch
        {
            set
            {
                IsMatchChar = value ? Constants.CheckMark : Constants.BallotX;
            }
        }
    }
}
