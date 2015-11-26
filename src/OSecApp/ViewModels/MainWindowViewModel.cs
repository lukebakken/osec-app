using System.Collections.ObjectModel;

namespace OSecApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string documentText;
        private string documentName;
        private readonly ObservableCollection<string> documentNames = new ObservableCollection<string>();

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

        public string DocumentName
        {
            get
            {
                return documentName;
            }
            
            set
            {
                SetProperty(ref documentName, value);
            }
        }

        public bool CanAddDocument
        {
            get
            {
                return Validation.NonEmptyString(documentText) &&
                       Validation.NonEmptyString(documentName);
            }
        }

        public ObservableCollection<string> DocumentNames
        {
            get
            {
                return documentNames;
            }
        }

        public void AddDocument(string name, string text)
        {
            documentNames.Add(name);
        }
    }
}
