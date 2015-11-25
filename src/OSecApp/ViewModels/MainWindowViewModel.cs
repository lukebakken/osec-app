namespace OSecApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string documentText;

        public string DocumentText
        {
            get
            {
                return documentText;
            }
            
            set
            {
                SetProperty(ref documentText, value);
            }
        }
    }
}
