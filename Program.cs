using PiecesAutoYoussefApp.NotificationCenter;
using PiecesAutoYoussefApp.UI.Screens;
using PiecesAutoYoussefApp.Utils;

namespace PiecesAutoYoussefApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (Mutex mutex = new Mutex(true, Constants.UNIQUE_APP_MUTEX_NAME, out createdNew))
            {
                if (!createdNew)
                {
                    // Application is already running; exit the new instance
                    ErrorHandler.NotifyAppAlreadyOpened();
                    return;
                }

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new MainFrom());
            }
        }
    }
}