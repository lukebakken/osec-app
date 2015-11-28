namespace OSecApp
{
    using System;
    using System.Windows;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel, PendingDocumentManager pendingDocumentManager)
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
                string name = txtDocumentName.Text;
                string content = txtDocument.Text;
                pendingDocumentManager.Enqueue(name, content);
            }
            else
            {
                MessageBox.Show(Properties.Resources.MainWindow_NonEmptyTextAndNameRequired,
                    Properties.Resources.MessageBox_ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddDocument(Document document)
        {
            Dispatcher.Invoke(() => viewModel.AddDocument(document));
        }

        public void ReplaceDocument(Document document)
        {
            Dispatcher.Invoke(() => viewModel.ReplaceDocument(document));
        }

        private void btnAddSearch_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
