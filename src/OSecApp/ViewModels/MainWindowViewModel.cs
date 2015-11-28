namespace OSecApp.ViewModels
{
    using System.Collections.ObjectModel;
    using OSecApp.Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private string documentText;
        private string documentName;

        private readonly ObservableCollection<string> documentNames = new ObservableCollection<string>();

        public string DocumentCount
        {
            get
            {
                return string.Format("Count: {0}", documentNames.Count);
            }
        }

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

        public void AddDocument(Document document)
        {
            documentNames.Add(document.Name);
        }

        // TODO argument is new document
        public void ReplaceDocument(Document document)
        {
        }
    }
}
