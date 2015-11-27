namespace OSecApp
{
    using System.Windows;
    using Managers;
    using ViewModels;

    public partial class App : Application
    {
        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();
        private readonly PendingDocumentManager pendingDocumentManager = new PendingDocumentManager();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(viewModel, pendingDocumentManager);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            pendingDocumentManager.Dispose();
        }

        // TODO show document replacement in UI
    }
}
