using System.Windows;
using TimeCalculator.Charts.Builder;

namespace TimeCalculator.Charts
{
    /// <summary>
    /// Interaktionslogik für ChartsWindow.xaml
    /// </summary>
    public partial class ChartsWindow : Window
    {
        public ChartsWindow(ChartDatabase database)
        {
            InitializeComponent();

            Content = database.CreateControl();
        }
    }
}
