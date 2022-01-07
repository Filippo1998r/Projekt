namespace Projekt
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using static WykresyFunkcji;

    //public class OxyColorToColorConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is OxyColor color)
    //            return color.ToColor();

    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class MainViewModel
    {
        //public OxyColor LinieColor;

        public MainViewModel()
        {

            //this.MyModel1 = new PlotModel { Title = "Wykres nr 1" };
            //this.MyModel1.Series.Add(new FunctionSeries(f(x)=5, 0, 10, 0.1, "sin(x)"));

            //this.MyModel1 = new PlotModel { Title = "Wykres nr 1" };
            //this.MyModel1.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            //this.MyModel2 = new PlotModel { Title = "Wykres nr 2" };
            ////this.MyModel2 = new OxyColor;
            //this.MyModel2.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));

            //MyModel1 = new PlotModel { Title = "Wykres funkcji:" };

            //var funkcjaPierwsza = (x) => -3 * Math.Sqrt(1 - Math.Pow((x / 7), 2)) * Math.Sqrt(Math.Abs(Math.Abs(x) - 4) / (Math.Abs(x) - 4));
            //var funkcjaDruga = (x) => Math.Abs(x / 2) - 0.0913722 * (Math.Pow(x, 2)) - 3 + Math.Sqrt(1 - Math.Pow((Math.Abs(Math.Abs(x) - 2) - 1), 2));

            //Func<double, double> batFn1 = (x) => 5;//2 * Math.Sqrt(-Math.Abs(Math.Abs(x) - 1) * Math.Abs(3 - Math.Abs(x)) / ((Math.Abs(x) - 1) * (3 - Math.Abs(x)))) * (1 + Math.Abs(Math.Abs(x) - 3) / (Math.Abs(x) - 3)) * Math.Sqrt(1 - Math.Pow((x / 7), 2)) + (5 + 0.97 * (Math.Abs(x - 0.5) + Math.Abs(x + 0.5)) - 3 * (Math.Abs(x - 0.75) + Math.Abs(x + 0.75))) * (1 + Math.Abs(1 - Math.Abs(x)) / (1 - Math.Abs(x)));
            //Func<double, double> batFn2 = (x) => -3 * Math.Sqrt(1 - Math.Pow((x / 7), 2)) * Math.Sqrt(Math.Abs(Math.Abs(x) - 4) / (Math.Abs(x) - 4));
            //Func<double, double> batFn3 = (x) => //Math.Abs(x / 2) - 0.0913722 * (Math.Pow(x, 2)) - 3 + Math.Sqrt(1 - Math.Pow((Math.Abs(Math.Abs(x) - 2) - 1), 2));
            //Func<double, double> batFn4 = (x) => //(2.71052 + (1.5 - .5 * Math.Abs(x)) - 1.35526 * Math.Sqrt(4 - Math.Pow((Math.Abs(x) - 1), 2))) * Math.Sqrt(Math.Abs(Math.Abs(x) - 1) / (Math.Abs(x) - 1)) + 0.9;

            //MyModel1.Series.Add(new FunctionSeries(batFn1, -8, 8, 0.0001));
            //MyModel1.Series.Add(new FunctionSeries(batFn2, -8, 8, 0.0001));
            //MyModel1.Series.Add(new FunctionSeries(batFn3, -8, 8, 0.0001));
            //MyModel1.Series.Add(new FunctionSeries(batFn4, -8, 8, 0.0001));

            //MyModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MaximumPadding = 0.1, MinimumPadding = 0.1 });
            //MyModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1 });
        }

        public PlotModel MyModel1 { get; private set; }
        //public PlotModel MyModel2 { get; private set; }
    }
}