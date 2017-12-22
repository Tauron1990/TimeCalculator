using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Windows.Shared;
using TimeCalculator.Data;

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

            using (var db = new JobDatabase())
            {
                db.Database.Migrate();
            }
        }
    }
}
