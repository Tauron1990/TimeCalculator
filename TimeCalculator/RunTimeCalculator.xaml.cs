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
        public RunTimeCalculator(bool addSetup, bool loadFormSessionManager)
        {
            InitializeComponent();


            ((RunTimeCalculatorViewModel) DataContext).Initialize(loadFormSessionManager);

            if (addSetup)
                ((RunTimeCalculatorViewModel) DataContext).AddItemSpecial();
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
            
            item.StartTime = (DateTime) e.NewValue;
        }

        private void End_TimeSpanEdit_OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RunTimeCalculatorItem item = (RunTimeCalculatorItem) d.GetValue(DataContextProperty);
            
            item.EndTime = (DateTime) e.NewValue;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
                ((RunTimeCalculatorViewModel)DataContext).AddItemSpecial();
        }
    }
}
