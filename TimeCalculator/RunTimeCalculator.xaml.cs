using System;
using System.Windows;

namespace TimeCalculator
{
    /// <summary>
    /// Interaktionslogik für RunTimeCalculator.xaml
    /// </summary>
    public partial class RunTimeCalculator
    {
        public RunTimeCalculator()
        {
            InitializeComponent();
        }

        public TimeSpan? EffectiveTime => ((RunTimeCalculatorViewModel) DataContext).EffectiveTime;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Start_TimeSpanEdit_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RunTimeCalculatorItem item = (RunTimeCalculatorItem) d.GetValue(DataContextProperty);

            item.StartTime = (TimeSpan) e.NewValue;
        }

        private void End_TimeSpanEdit_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RunTimeCalculatorItem item = (RunTimeCalculatorItem) d.GetValue(DataContextProperty);

            item.EndTime = (TimeSpan) e.NewValue;
        }
    }
}
