namespace OSecApp.ViewModels
{
    using System.Collections.ObjectModel;
    using OSecApp.Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private string documentName;
        private string documentContent;

        private readonly ObservableCollection<DocumentViewModel> documents = new ObservableCollection<DocumentViewModel>();
        private readonly ObservableCollection<SearchViewModel> searches = new ObservableCollection<SearchViewModel>();

        public string DocumentCount
        {
            get
            {
                return string.Format("Count: {0}", documents.Count);
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

        public string DocumentContent
        {
            get
            {
                return documentContent;
            }
            
            set
            {
                SetProperty(ref documentContent, value);
            }
        }

        public bool CanAddDocument
        {
            get
            {
                return Validation.NonEmptyString(documentContent) &&
                       Validation.NonEmptyString(documentName);
            }
        }

        public ObservableCollection<DocumentViewModel> Documents
        {
            get
            {
                return documents;
            }
        }

        public ObservableCollection<SearchViewModel> Searches
        {
            get
            {
                return searches;
            }
        }

        public void AddDocument(Document document)
        {
            var vm = new DocumentViewModel
            {
                Name = document.Name,
                Content = document.Content
            };
            documents.Add(vm);
        }

        public void ReplaceDocument(Document document)
        {
            bool replaced = false;

            foreach (var vm in documents)
            {
                if (vm.Name == document.Name)
                {
                    vm.Content = document.Content;
                    replaced = true;
                    break;
                }
            }

            if (replaced)
            {
                OnPropertyChanged("Documents");
            }
        }

        public void AddSearch(Search search)
        {
            var vm = new SearchViewModel
            {
                Term = search.Term,
            };
            searches.Add(vm);
        }
    }
}
