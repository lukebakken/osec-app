namespace OSecApp
{
    using System.Windows;
    using ViewModels;

    public partial class MainWindow : Window
    {
        private static readonly string[] AllowedPasteFormats = new[] {
            DataFormats.Text,
            DataFormats.UnicodeText,
            DataFormats.OemText
        };

        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(txtDocument, OnDocumentPaste);
            DataContext = viewModel;
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
                viewModel.AddDocument(txtDocumentName.Text, txtDocument.Text);
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
