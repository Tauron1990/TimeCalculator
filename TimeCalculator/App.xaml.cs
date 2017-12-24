using System.Globalization;
using System.Threading;
using System.Windows;

namespace TimeCalculator
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var currentThread = Thread.CurrentThread;

            var culture = new CultureInfo("de-de");
                
            currentThread.CurrentCulture = culture;
            currentThread.CurrentUICulture = culture;
        }
    }
}
