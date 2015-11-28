﻿namespace OSecApp
{
    using System.Windows;
    using Managers;
    using NLog;
    using ViewModels;

    public partial class App : Application
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();
        private readonly PendingDocumentManager pendingDocumentManager = new PendingDocumentManager();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log.Debug("Application_Startup");
            pendingDocumentManager.DocumentEnqueued += PendingDocumentManager_DocumentEnqueued;
            var mainWindow = new MainWindow(viewModel, pendingDocumentManager);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            pendingDocumentManager.Dispose();
        }

        private void PendingDocumentManager_DocumentEnqueued(object sender, DocumentEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        // TODO show document replacement in UI
    }
}
