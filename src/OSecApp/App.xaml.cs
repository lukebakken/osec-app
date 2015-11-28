namespace OSecApp
{
    using System.Threading;
    using System.Windows;
    using Managers;
    using NLog;
    using ViewModels;

    public partial class App : Application
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();
        private readonly PendingDocumentManager pendingDocumentManager = new PendingDocumentManager();
        private readonly DocumentIndexer documentIndexer = new DocumentIndexer();
        private readonly DocumentMonitor documentMonitor;

        private MainWindow mainWindow;

        public App()
        {
            documentMonitor = new DocumentMonitor(cts, pendingDocumentManager, documentIndexer);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log.Debug("Application_Startup");

            documentMonitor.DocumentAdded += DocumentMonitor_DocumentAdded;
            documentMonitor.DocumentReplaced += DocumentMonitor_DocumentReplaced;

            mainWindow = new MainWindow(viewModel, pendingDocumentManager);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            cts.Cancel();

            documentMonitor.DocumentAdded -= DocumentMonitor_DocumentAdded;
            documentMonitor.DocumentReplaced -= DocumentMonitor_DocumentReplaced;

            pendingDocumentManager.Dispose();
            documentIndexer.Dispose();
            documentMonitor.Dispose();
        }

        private void DocumentMonitor_DocumentAdded(object sender, DocumentEventArgs e)
        {
            mainWindow.AddDocument(e.Document);
        }

        private void DocumentMonitor_DocumentReplaced(object sender, DocumentEventArgs e)
        {
            mainWindow.ReplaceDocument(e.Document);
        }
    }
}
