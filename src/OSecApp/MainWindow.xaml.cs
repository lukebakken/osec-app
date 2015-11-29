namespace OSecApp
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Managers;
    using Models;
    using ViewModels;

    public partial class MainWindow : Window
    {
        private static readonly string[] AllowedPasteFormats = new[] {
            DataFormats.Text,
            DataFormats.UnicodeText,
            DataFormats.OemText
        };

        private readonly MainWindowViewModel viewModel;
        private readonly PendingDocumentManager pendingDocumentManager;
        private readonly PendingSearchManager pendingSearchManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(
            MainWindowViewModel viewModel,
            PendingDocumentManager pendingDocumentManager,
            PendingSearchManager pendingSearchManager)
            : this()
        {
            this.viewModel = viewModel;
            if (this.viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.pendingDocumentManager = pendingDocumentManager;
            if (this.pendingDocumentManager == null)
            {
                throw new ArgumentNullException("pendingDocumentManager");
            }

            this.pendingSearchManager = pendingSearchManager;
            if (this.pendingSearchManager == null)
            {
                throw new ArgumentNullException("pendingSearchManager");
            }

            DataObject.AddPastingHandler(txtDocument, OnDocumentPaste);
            DataContext = this.viewModel;
        }

        private void OnDocumentPaste(object sender, DataObjectPastingEventArgs e)
        {
            bool isAllowed = false;

            foreach (var f in AllowedPasteFormats)
            {
                isAllowed = e.SourceDataObject.GetDataPresent(f, true);
                if (isAllowed)
                {
                    break;
                }
            }

            if (isAllowed == false)
            {
                e.CancelCommand();
            }
        }

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CanAddDocument)
            {
                IsEnabled = false;

                string name = viewModel.DocumentName;
                string content = viewModel.DocumentContent;
                pendingDocumentManager.Enqueue(name, content);

                viewModel.ClearDocument();
            }
            else
            {
                MessageBox.Show(Properties.Resources.MainWindow_NonEmptyTextAndNameRequired,
                    Properties.Resources.MessageBox_ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddDocument(Document document)
        {
            Dispatcher.Invoke(() =>
            {
                viewModel.AddDocument(document);
                IsEnabled = true;
            });
        }

        public void ReplaceDocument(Document document)
        {
            Dispatcher.Invoke(() =>
            {
                viewModel.ReplaceDocument(document);
                IsEnabled = true;
            });
        }

        public void SearchComplete(Search search)
        {
            Dispatcher.Invoke(() =>
            {
                viewModel.SearchComplete(search);
                IsEnabled = true;
            });
        }

        private void btnAddSearch_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddSearch(new Search(txtSearchTerm.Text));
            txtSearchTerm.Text = null;
        }

        private void lstFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            var vm = (DocumentViewModel)lv.SelectedItem;

            viewModel.DocumentName = vm.Name;
            viewModel.DocumentContent = vm.Content;
            viewModel.ClearReplaced();
        }

        private void searchExeButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            var b = (Button)sender;
            var vm = (SearchViewModel)b.DataContext;
            pendingSearchManager.Enqueue(new Search(vm.Term));
        }
    }
}
