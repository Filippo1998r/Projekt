namespace Projekt
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Wpf;

    public class OxyColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OxyColor color)
                return color.ToColor();

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MainViewModel
    {
        public OxyColor LinieColor;

        public MainViewModel()
        {
            this.MyModel1 = new PlotModel { Title = "Wykres nr 1" };
            this.MyModel1.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            this.MyModel2 = new PlotModel { Title = "Wykres nr 2" };
            //this.MyModel2 = new OxyColor;
            this.MyModel2.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));
        }
        public PlotModel MyModel1 { get; private set; }
        public PlotModel MyModel2 { get; private set; }
    }
}