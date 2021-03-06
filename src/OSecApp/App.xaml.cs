﻿namespace OSecApp
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
        private readonly PendingSearchManager pendingSearchManager = new PendingSearchManager();

        private readonly DocumentIndexer documentIndexer = new DocumentIndexer();

        private readonly DocumentMonitor documentMonitor;
        private readonly SearchMonitor searchMonitor;

        private MainWindow mainWindow;

        public App()
        {
            documentMonitor = new DocumentMonitor(cts, pendingDocumentManager, documentIndexer);
            searchMonitor = new SearchMonitor(cts, pendingSearchManager, documentIndexer);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log.Debug("Application_Startup");

            documentMonitor.DocumentAdded += DocumentMonitor_DocumentAdded;
            documentMonitor.DocumentReplaced += DocumentMonitor_DocumentReplaced;

            searchMonitor.SearchComplete += SearchMonitor_SearchComplete;

            mainWindow = new MainWindow(viewModel, pendingDocumentManager, pendingSearchManager);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            cts.Cancel();

            documentMonitor.DocumentAdded -= DocumentMonitor_DocumentAdded;
            documentMonitor.DocumentReplaced -= DocumentMonitor_DocumentReplaced;

            searchMonitor.SearchComplete -= SearchMonitor_SearchComplete;

            pendingDocumentManager.Dispose();
            pendingSearchManager.Dispose();

            documentIndexer.Dispose();

            documentMonitor.Dispose();
            searchMonitor.Dispose();
        }

        private void DocumentMonitor_DocumentAdded(object sender, DocumentEventArgs e)
        {
            mainWindow.AddDocument(e.Document);
        }

        private void DocumentMonitor_DocumentReplaced(object sender, DocumentEventArgs e)
        {
            mainWindow.ReplaceDocument(e.Document);
        }

        private void SearchMonitor_SearchComplete(object sender, SearchEventArgs e)
        {
            mainWindow.SearchComplete(e.Search);
        }
    }
}
