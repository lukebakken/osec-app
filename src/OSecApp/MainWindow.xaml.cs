namespace OSecApp
{
    using System.Linq;
    using System.Windows;
    using ViewModels;

    public partial class MainWindow : Window
    {
        private static readonly string[] AllowedPasteFormats = new[] {
            DataFormats.Text,
            DataFormats.UnicodeText,
            DataFormats.OemText
        };

        public MainWindow()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(txtDocument, OnDocumentPaste);

            DataContext = new MainWindowViewModel();
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
    }
}
