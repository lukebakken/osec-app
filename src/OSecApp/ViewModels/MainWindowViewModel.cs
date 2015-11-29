namespace OSecApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private string documentName;
        private string documentContent;

        private readonly Dictionary<string, DocumentViewModel> documentLookup = new Dictionary<string, DocumentViewModel>();
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
            documentLookup[vm.Name] = vm;
        }

        public void ReplaceDocument(Document document)
        {
            foreach (var vm in documents)
            {
                if (vm.Name == document.Name)
                {
                    vm.Content = document.Content;
                    vm.IsReplaced = true;
                    break;
                }
            }
        }

        public void ClearDocument()
        {
            DocumentName = null;
            DocumentContent = null;
        }

        public void AddSearch(Search search)
        {
            var vm = new SearchViewModel
            {
                Term = search.Term,
            };
            searches.Add(vm);
        }

        public void SearchComplete(Search search)
        {
            // TODO: if necessary, this could be sped up
            // via use of sets and set operations

            foreach (var vm in documents)
            {
                vm.IsMatch = false;
            }

            if (search.HasMatches)
            {
                foreach (var d in search.Matches)
                {
                    var vm = documentLookup[d.Name];
                    vm.IsMatch = true;
                }
            }
        }

        public void ClearReplaced()
        {
            foreach (var d in documents)
            {
                d.IsReplaced = false;
            }
        }
    }
}
