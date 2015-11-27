namespace OSecApp
{
    using System;
    using System.Windows;
    using Managers;
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
            DataObject.AddPastingHandler(txtDocument, OnDocumentPaste);
            DataContext = viewModel;
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

                // TODO should be added to view model else where once collision managed
                viewModel.AddDocument(name);
                pendingDocumentManager.Enqueue(name, content);
            }
            else
            {
                MessageBox.Show(Properties.Resources.MainWindow_NonEmptyTextAndNameRequired,
                    Properties.Resources.MessageBox_ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddSearch_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
