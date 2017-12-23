using System;
using System.Windows;
using System.Windows.Input;

namespace TimeCalculator
{
    /// <summary>
    /// Interaktionslogik für RunTimeCalculator.xaml
    /// </summary>
    public partial class RunTimeCalculator
    {
        public RunTimeCalculator(bool addSetup)
        {
            InitializeComponent();

            if(addSetup)
                ((RunTimeCalculatorViewModel)DataContext).AddItemSpecial();
        }

        public RuntimeCalculatorResult EffectiveTime => Dispatcher.Invoke(() => (RunTimeCalculatorViewModel) DataContext).EffectiveTime;

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

            DateTime temp = (DateTime) e.NewValue;

            item.StartTime = new TimeSpan(temp.Hour, temp.Minute, 0);
        }

        private void End_TimeSpanEdit_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RunTimeCalculatorItem item = (RunTimeCalculatorItem) d.GetValue(DataContextProperty);

            DateTime temp = (DateTime) e.NewValue;

            item.EndTime = new TimeSpan(temp.Hour, temp.Minute, 0);
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((RunTimeCalculatorViewModel)DataContext).AddItemSpecial();
        }
    }
}
