using System;
using System.ComponentModel;
using System.Windows;
using Syncfusion.Windows.Tools.Controls;

namespace TimeCalculator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ((MainWindowViewModel) DataContext).IsOperationRunning = false;
        }

        private void TabControlExt_OnOnCloseButtonClick(object sender, CloseTabEventArgs e) => e.Cancel = true;

        private void TimeSpanEdit_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MainWindowViewModel) DataContext).RunTime = (TimeSpan) e.NewValue;

        private void BindableBase_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
                {
                    var model = (MainWindowViewModel) sender;

                    if (e.PropertyName == nameof(model.RunTime))
                        RunTimeElement.Value = model.RunTime;
                });
        }

        private bool _shown;

        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            if(_shown) return;

            _shown = true;
            ((MainWindowViewModel)DataContext).WindowLoaded();
        }
    }
}
