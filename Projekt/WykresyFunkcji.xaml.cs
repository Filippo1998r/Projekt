using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy WykresyFunkcji.xaml
    /// </summary>
    public partial class WykresyFunkcji : Page
    {
        private const string ZeroNIE = "Nie można dzielić przez ZERO!!!";
        private const string wpiszLiczbe = "Proszę wpisz liczbę!!!";
        private const string BrakObslugi = "NIESTETY!!! Takie działanie nie jest obsługiwane!";
        public WykresyFunkcji()
        {
            InitializeComponent();

            var nlBE = new System.Globalization.CultureInfo("nl-BE");/////
            nlBE.NumberFormat.CurrencyDecimalDigits = 2;//////////////////
            nlBE.NumberFormat.CurrencyDecimalSeparator = ",";/////////////W Polsce nie tolerujemy kropek. Tylko przecinki. Koniec kropka.
            nlBE.NumberFormat.CurrencyGroupSeparator = ".";///////////////
            System.Threading.Thread.CurrentThread.CurrentCulture = nlBE;//
        }

        private void Click_Kalkulator(object sender, RoutedEventArgs e)
        {
            var kalkulator = new Calculator();
            NavigationService.Navigate(kalkulator);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ResultText.Text = string.Empty;

#pragma warning disable CS8600 // Konwertowanie literału o wartości null lub możliwej wartości null na nienullowalny typ.
            Button button = sender as Button;
#pragma warning restore CS8600 // Konwertowanie literału o wartości null lub możliwej wartości null na nienullowalny typ.

            object currentNumber = button.Content;

            if (string.IsNullOrEmpty(CurrentOperationText.Text))// i jeśli zawiera "znak,znak" w całym ciągu znaków to nie wypisuj... i to w rozszerzonym kalkulatorze, NIE tu!!!
            {
                if (currentNumber.ToString() == ",")
                {
                    CurrentOperationText.Text = "0" + currentNumber;
                }
                else
                {
                    CurrentOperationText.Text += currentNumber;
                }
            }
            else if (CurrentOperationText.Text.EndsWith("∞")) { }
            else if (CurrentOperationText.Text == "0")
            {
                if (currentNumber.ToString() == ",")
                {
                    CurrentOperationText.Text += currentNumber;
                }
                else
                {
                    CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                    CurrentOperationText.Text += currentNumber;
                }
            }
            else if (CurrentOperationText.Text.EndsWith("+0")
                      || CurrentOperationText.Text.EndsWith("-0")
                      || CurrentOperationText.Text.EndsWith("*0")
                      || CurrentOperationText.Text.EndsWith(":0")
                      || CurrentOperationText.Text.EndsWith("/0")
                      || CurrentOperationText.Text.EndsWith("√0"))
            {
                if (currentNumber.ToString() == ",")
                {
                    CurrentOperationText.Text += currentNumber;
                }
                else
                {
                    CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                    CurrentOperationText.Text += currentNumber;
                }
            }
            else if (CurrentOperationText.Text.EndsWith("√0"))
            {
                if (currentNumber.ToString() == ",")
                {
                    CurrentOperationText.Text += currentNumber;
                }
                else
                {
                    CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                    CurrentOperationText.Text += currentNumber;
                }
            }
            else if (CurrentOperationText.Text.EndsWith("+")
                     || CurrentOperationText.Text.EndsWith("-")
                     || CurrentOperationText.Text.EndsWith("*")
                     || CurrentOperationText.Text.EndsWith(":")
                     || CurrentOperationText.Text.EndsWith("√")
                     || CurrentOperationText.Text.EndsWith("/"))
            {
                if (currentNumber.ToString() == ",")
                {
                    CurrentOperationText.Text += "0" + currentNumber;
                }
                else
                {
                    CurrentOperationText.Text += currentNumber;
                }
            }
            else if (CurrentOperationText.Text.EndsWith("²")) { }
            else if (CurrentOperationText.Text.Contains('E'))
            {
                if (Regex.Matches(CurrentOperationText.Text, "[+]").Count == 2)
                {
                    string[] elements = CurrentOperationText.Text.Split('+');
                    if (elements[2].Length >= 15) { }
                    else
                    {
                        if (elements[2].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (Regex.Matches(CurrentOperationText.Text, "[-]").Count == 2)
                {
                    string[] elements = CurrentOperationText.Text.Split('-');
                    if (elements[2].Length >= 15) { }
                    else
                    {
                        if (elements[2].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains("-") && CurrentOperationText.Text.Contains("+"))
                {
                    string[] elements = CurrentOperationText.Text.Split('-', '+');
                    if (elements[2].Length >= 15) { }
                    else
                    {
                        if (elements[2].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains("-"))
                {
                    string[] elements = CurrentOperationText.Text.Split('-');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains(":"))
                {
                    string[] elements = CurrentOperationText.Text.Split(':');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains("*"))
                {
                    string[] elements = CurrentOperationText.Text.Split('*');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else
                {
                    if (CurrentOperationText.Text.Length >= 15) { }
                    else
                    {
                        if (CurrentOperationText.Text.Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { } //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
            }
            else if (!CurrentOperationText.Text.Contains('E'))
            {
                if (CurrentOperationText.Text.Contains("+"))
                {
                    string[] elements = CurrentOperationText.Text.Split('+');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains(":"))
                {
                    string[] elements = CurrentOperationText.Text.Split(':');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains("*"))
                {
                    string[] elements = CurrentOperationText.Text.Split('*');

                    if (elements[1].Length >= 15) { }
                    else
                    {
                        if (elements[1].Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
                else if (CurrentOperationText.Text.Contains("-")) // zmieniona kolejność warunku z minusem, bo będąc przed pozostałymi znakami ma też pierszeństwo w wykonywaniu i nie można dopisać przecinka w drugiej liczbie xD
                {                                                 // TAK, bo tylko w przypadku minusa mogą siępojawić 2 znaki. Można go dać niżi. Ale jeszcze ten sam problem z minusem.
                    string[] elements = CurrentOperationText.Text.Split('-');

                    if (Regex.Matches(CurrentOperationText.Text, "[-]").Count == 2)
                    {
                        if (elements[1].Length >= 15) { }
                        else
                        {
                            if (elements[1].Contains(","))
                            {
                                if (currentNumber.ToString() == ",") { }     //jeśli istnieje już przecinek nie wypisuje kolejnego.
                                else
                                {
                                    CurrentOperationText.Text += currentNumber;
                                }
                            }
                            if (elements[2].Contains(","))
                            {
                                if (currentNumber.ToString() == ",") { }
                                else
                                {
                                    CurrentOperationText.Text += currentNumber;
                                }
                            }
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                    }
                    else
                    {
                        if (elements[1].Length >= 15) { }
                        else
                        {
                            if (elements[1].Contains(","))
                            {
                                if (currentNumber.ToString() == ",") { }
                                else
                                {
                                    CurrentOperationText.Text += currentNumber;
                                }
                            }
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                    }
                }
                else
                {
                    if (CurrentOperationText.Text.Length >= 15) { }
                    else
                    {
                        if (CurrentOperationText.Text.Contains(","))
                        {
                            if (currentNumber.ToString() == ",") { } //jeśli istnieje już przecinek nie wypisuje kolejnego.
                            else
                            {
                                CurrentOperationText.Text += currentNumber;
                            }
                        }
                        else
                        {
                            CurrentOperationText.Text += currentNumber;
                        }
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            const double margin = 10;
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymin = margin;
            double ymax = canGraph.Height - margin;
            const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(canGraph.Width, ymax)));
            for (double x = xmin + step;
                x <= canGraph.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)ymin, (int)ymax);

                PointCollection points = new PointCollection();
                for (double x = xmin; x <= xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < ymin) last_y = (int)ymin;
                    if (last_y > ymax) last_y = (int)ymax;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                canGraph.Children.Add(polyline);
            }
        }

        private void ResultText_Kopiuj(object sender, RoutedEventArgs e)
        {
            //używa metody obiektu schowka
            Clipboard.SetText(ResultText.Text);

            //to pozwoli na późniejsze wklejenie ZE schowka DO pola tekstowego, np. z kalku do zadania, ale trzeba określić konkretne miejsce wklejenia
            //ResultText.Text = Clipboard.GetText();
        }

    }
}