using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;
using static Projekt.WykresyFunkcji;

namespace Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>

    public partial class Calculator : Window
    {
        private const string ZeroNIE = "Nie można dzielić przez ZERO!!!";
        private const string wpiszLiczbe = "Proszę wpisz liczbę!!!";
        private const string BrakObslugi = "NIESTETY!!! Takie działanie nie jest obsługiwane!";

        public Calculator()
        {
            InitializeComponent();
            ResultText.Text = "0";
            CurrentOperationText.Text = string.Empty;

            var nlBE = new System.Globalization.CultureInfo("nl-BE");/////
            nlBE.NumberFormat.CurrencyDecimalDigits = 2;//////////////////
            nlBE.NumberFormat.CurrencyDecimalSeparator = ",";/////////////W Polsce nie tolerujemy kropek. Tylko przecinki. Koniec kropka.
            nlBE.NumberFormat.CurrencyGroupSeparator = ".";///////////////
            System.Threading.Thread.CurrentThread.CurrentCulture = nlBE;//
        }
        private void Click_Wykresy(object sender, RoutedEventArgs e)
        {
            NavigationWindow window = new NavigationWindow
            {
                Source = new Uri("WykresyFunkcji.xaml", UriKind.Relative)
            };
            window.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void ListaHistorii_Click(object sender, RoutedEventArgs e)
        {
            //ListBox listBox1 = new ListBox();

            //ListaHistorii.Items.CopyTo(liczba);

            ////ListBoxItem wartosci = ((sender as ListBox).SelectedItems as ListBoxItem);

            //string liczba = ListaHistorii.ToString();

            if (ListaHistorii.SelectedItem == null) { }
            else
            {
#pragma warning disable CS8600 // Konwertowanie literału o wartości null lub możliwej wartości null na nienullowalny typ.
                string dzialanie = ListaHistorii.SelectedItem.ToString();
#pragma warning restore CS8600 // Konwertowanie literału o wartości null lub możliwej wartości null na nienullowalny typ.

                string[] elements = dzialanie.Split('=');

                string wynik = Regex.Replace(elements[1], @"\s+", "");

                if (EndsWithOperation(CurrentOperationText.Text))
                {
                    ResultText.Text = string.Empty;
                    CurrentOperationText.Text += wynik;
                }
                else if (wynik.Contains('E'))//Tymczasowo
                {
                    ResultText.Text = BrakObslugi;
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    ResultText.Text = wynik;
                    CurrentOperationText.Text = string.Empty;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultText.Text = string.Empty;

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
        private void Button_ClickDodawanie(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (operation.ToString() == "-") { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.StartsWith("√") && operation.Contains('E'))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '+';
            }
            else if (EndsWithOperation(operation))//zamienia z + na inne działanie
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                CurrentOperationText.Text += "+";
            }
            else if (CurrentOperationText.Text.EndsWith(",")) { }////////////////////////////////////////Nie doda znaku działania dopuki nie dopiszemy liczby po przecinku
            else if (CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '+';
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text += "+";
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz + '+';
                }
            }
            else if (CurrentOperationText.Text.Contains(':') && CurrentOperationText.Text.EndsWith("0") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
            }//////////////////////////////////////Nie dziel przez zero
            else if (ContainsOperation(operation) || CurrentOperationText.Text.Contains('-') && !EndsWithOperation(operation))
            {
                if (operation.StartsWith("-") && !ContainsOperation(operation))
                {
                    CurrentOperationText.Text += "+";
                }
                else
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                    CurrentOperationText.Text += "+";
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                CurrentOperationText.Text = ResultText.Text + "+";
            }
            else
            {
                CurrentOperationText.Text += "+";
            }
        }
        private void Button_ClickOdejmowanie(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (operation.ToString() == "-") { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation == "1/")
            {
                CurrentOperationText.Text += "-";
            }
            else if (operation.StartsWith("√") && operation.Contains('E'))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '-';
            }
            else if (EndsWithOperation(operation))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                CurrentOperationText.Text += "-";
            }
            else if (CurrentOperationText.Text.EndsWith(",") || CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '-';
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text += "-";
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz + '-';
                }
            }
            else if (CurrentOperationText.Text.Contains(":") && CurrentOperationText.Text.EndsWith("0") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
            }
            else if (ContainsOperation(operation) || operation.Contains('-') && !EndsWithOperation(operation))
            {
                if (operation.StartsWith("-"))
                {
                    CurrentOperationText.Text += "-";
                }
                else
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                    CurrentOperationText.Text += "-";
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                if (ResultText.Text.ToString() != "0")
                {
                    CurrentOperationText.Text = ResultText.Text + "-";
                }
                else
                {
                    CurrentOperationText.Text += "-";
                }
            }
            else
            {
                CurrentOperationText.Text += "-";
            }
        }
        private void Button_ClickMnozenie(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;//było var...
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (operation.ToString() == "-") { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.StartsWith("√") && operation.Contains('E'))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '*';
            }
            else if (EndsWithOperation(operation))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                CurrentOperationText.Text += "*";
            }
            else if (CurrentOperationText.Text.EndsWith(",")) { }////////////////////////////////////////nie doda znaku działania dopuki nie dopiszemy liczby po przecinku
            else if (CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + '*';
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text += '*';
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz + '*';
                }
            }
            else if (CurrentOperationText.Text.Contains(":") && CurrentOperationText.Text.EndsWith("0") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
            }
            else if (ContainsOperation(operation) || CurrentOperationText.Text.Contains('-') && !EndsWithOperation(operation))
            {
                if (operation.StartsWith("-") && !ContainsOperation(operation))
                {
                    CurrentOperationText.Text += "*";
                }
                else
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                    CurrentOperationText.Text += "*";
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                CurrentOperationText.Text = ResultText.Text + "*";
            }
            else
            {
                CurrentOperationText.Text += "*";
            }
        }
        private void Button_ClickDzielenie(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (operation.ToString() == "-") { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.StartsWith("√") && operation.Contains('E'))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + ':';
            }
            else if (EndsWithOperation(operation))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                CurrentOperationText.Text += ":";
            }
            else if (CurrentOperationText.Text.EndsWith(",")) { }////////////////////////////////////////nie doda znaku działania dopuki nie dopiszemy liczby po przecinku
            else if (CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz + ':';
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text += ':';
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz + ':';
                }
            }
            else if (CurrentOperationText.Text.Contains(":") && CurrentOperationText.Text.EndsWith("0") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
            }
            else if (ContainsOperation(operation) || CurrentOperationText.Text.Contains('-') && !EndsWithOperation(operation))
            {
                if (operation.StartsWith("-") && !ContainsOperation(operation))
                {
                    CurrentOperationText.Text += ":";
                }
                else
                {
                    if (CurrentOperationText.Text.EndsWith(":0"))
                    {
                        ResultText.Text = ZeroNIE;
                        CurrentOperationText.Text = string.Empty;
                    }
                    else
                    {
                        ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                        CurrentOperationText.Text = obliczIWpisz;
                        CurrentOperationText.Text += ":";
                    }
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                CurrentOperationText.Text = ResultText.Text + ":";
            }
            else
            {
                CurrentOperationText.Text += ":";
            }
        }
        private void Button_ClickPierwiastek(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (CurrentOperationText.Text.EndsWith(",") || CurrentOperationText.Text.EndsWith("²") || CurrentOperationText.Text.EndsWith("E") || ResultText.Text.StartsWith("-")) { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (CurrentOperationText.Text.Contains("/")) { }
            else if (CurrentOperationText.Text.Contains("√"))
            {
                //ResultText.Text = obliczIWpisz;
                //CurrentOperationText.Text = string.Empty;
            }
            else if (CurrentOperationText.Text.EndsWith("²")) { }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text = "√" + CurrentOperationText.Text;
                }
                else if (CzyMaZnaki(operation))
                {
                    if (operation.Contains('√')) { }
                    else
                    {
                        CurrentOperationText.Text = "√" + CurrentOperationText.Text;
                    }
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                if (ResultText.Text == "0")
                {
                    CurrentOperationText.Text = "√";
                }
                else
                {
                    CurrentOperationText.Text = "√" + ResultText.Text;
                }
            }
            else
            {
                CurrentOperationText.Text = "√" + CurrentOperationText.Text;//może
            }
        }
        private void Button_ClickPotega(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (CurrentOperationText.Text.EndsWith(",") || CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (operation.EndsWith("/-") || operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (EndsWithOperation(operation)) { }//nie wstawia potęgi za działaniem
            else if (CurrentOperationText.Text.Contains(":") && CurrentOperationText.Text.EndsWith("0"))
            {
                SprawdzCzyNieZero(operation);
            }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz;
            }
            else if (CurrentOperationText.Text.Contains("/"))
            {
                string[] elements = CurrentOperationText.Text.Split('/', '²');

                if (Convert.ToDouble(elements[1]) * 1 == 0)
                {
                    ResultText.Text = ZeroNIE;
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    CurrentOperationText.Text += "²";
                }
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text += '²';
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                }
            }
            else if (ResultText.Text == ZeroNIE || ResultText.Text == BrakObslugi || ResultText.Text == wpiszLiczbe) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe))
            {
                CurrentOperationText.Text = ResultText.Text + "²";
            }
            else
            {
                CurrentOperationText.Text += "²";
            }
        }
        private void Button_ClickUlamek(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");

            if (CurrentOperationText.Text.EndsWith(",") || CurrentOperationText.Text.EndsWith("√") || CurrentOperationText.Text.EndsWith("E")) { }
            else if (operation.EndsWith("/-"))
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.StartsWith("√") && operation.Contains('E'))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = "1/" + obliczIWpisz;
            }
            else if (operation.StartsWith("0") && operation.Contains(':'))
            {
                string[] elements = operation.Split(':');

                if (Convert.ToDouble(elements[0]) * 1 == 0)
                {
                    ResultText.Text = ZeroNIE;
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    ResultText.Text = obliczIWpisz;
                    CurrentOperationText.Text = string.Empty;
                }
            }
            else if (EndsWithOperation(operation))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
            }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = "1/" + obliczIWpisz;
            }
            else if (operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.Contains("E+") || operation.Contains("E-"))
            {
                if (CzyNieMaZnakow(operation))
                {
                    CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
                }
                else if (CzyMaZnaki(operation))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    ResultText.Text = obliczIWpisz;
                    CurrentOperationText.Text = string.Empty;
                }
            }
            else if (operation.Contains(':') && CurrentOperationText.Text.EndsWith("0") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
                //CurrentOperationText.Text = "1/" + obliczIWpisz;  // nie wiem czemu to dodałem
            }
            else if (ContainsOperation(operation) || CurrentOperationText.Text.Contains('-'))
            {
                if (Regex.Matches(operation, "[-]").Count == 2 || (operation.StartsWith("-") && ContainsOperation(operation)))
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                    CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
                }
                else if (operation.StartsWith("-"))
                {
                    CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
                }
                else
                {
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                    CurrentOperationText.Text = obliczIWpisz;
                    CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
                }
            }
            else if (operation == "1/0" || operation == "1/0,")
            {
                ResultText.Text = ZeroNIE;
                CurrentOperationText.Text = string.Empty;
            }
            else if (CurrentOperationText.Text.Contains("/") && CurrentOperationText.Text.EndsWith("/"))
            {
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + obliczIWpisz);
                CurrentOperationText.Text = obliczIWpisz;
                CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
            }
            else if (CurrentOperationText.Text.EndsWith("/")) { }
            else if (string.IsNullOrEmpty(CurrentOperationText.Text) && !(ResultText.Text == ZeroNIE) && !(ResultText.Text == BrakObslugi) && !(ResultText.Text == wpiszLiczbe) && !(ResultText.Text == "0"))
            {
                CurrentOperationText.Text = "1/" + ResultText.Text;
            }
            else
            {
                CurrentOperationText.Text = "1/" + CurrentOperationText.Text;
            }
        }
        private void Button_ClickWynik(object sender, RoutedEventArgs e)
        {
            string operation = CurrentOperationText.Text;
            string obliczIWpisz = CalculateResult(operation).ToString("G15");
            double arg2 = 0;
            bool canConvert = double.TryParse(operation, out arg2);

            if (Regex.Matches(operation, "[E]").Count >= 2 || Regex.Matches(ResultText.Text, "[E]").Count >= 2)
            {
                ResultText.Text = BrakObslugi;
                CurrentOperationText.Text = string.Empty;
            }
            else if (CurrentOperationText.Text.EndsWith("E") || CurrentOperationText.Text.EndsWith("E+"))
            {
                ResultText.Text = BrakObslugi;
                CurrentOperationText.Text = string.Empty;
            }
            else if (operation.ToString() == "-") { }
            else if (operation.EndsWith("/-") || operation == "1/")
            {
                ResultText.Text = wpiszLiczbe;
            }
            else if (operation.EndsWith("/-")) { }
            else if (CurrentOperationText.Text.Contains("²"))
            {
                ResultText.Text = obliczIWpisz;
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                CurrentOperationText.Text = string.Empty;
            }
            else if (operation.StartsWith("0") && operation.Contains(':'))
            {
                string[] elements = operation.Split(':');

                if (Convert.ToDouble(elements[1]) * 1 == 0)
                {
                    ResultText.Text = ZeroNIE;
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    ResultText.Text = obliczIWpisz;
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                    CurrentOperationText.Text = string.Empty;
                }
            }
            else if (operation.Contains(':') && !operation.EndsWith(":") || operation.Contains('/'))
            {
                SprawdzCzyNieZero(operation);
            }
            else if (operation == "1/0" || operation == "1/0,")
            {
                ResultText.Text = ZeroNIE;
                CurrentOperationText.Text = string.Empty;
            }
            else if (operation.EndsWith("√"))
            {
                ResultText.Text = "0";
                ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                CurrentOperationText.Text = string.Empty;
            }
            else if (canConvert == true)
            {
                if (operation.EndsWith(","))
                {
                    CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
                    ResultText.Text = CurrentOperationText.Text;
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                    CurrentOperationText.Text = string.Empty;
                }
                else if (operation.Contains(',') && !operation.EndsWith(","))
                {
                    double test = Convert.ToDouble(CurrentOperationText.Text) * 1;
                    ResultText.Text = test.ToString();
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    ResultText.Text = operation;
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                    CurrentOperationText.Text = string.Empty;
                }
            }
            else if (!string.IsNullOrEmpty(ResultText.Text) && string.IsNullOrEmpty(CurrentOperationText.Text)) { }
            else
            {
                ResultText.Text = obliczIWpisz;
                _ = ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);//a może by tą Listahistorii zamienić na funkcję w której łączy się z tamtym listboxem
                //string ile = ListaHistorii.SelectedItem.ToString();
                //setItems(wartosc.ToString);
                //mylist.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                CurrentOperationText.Text = string.Empty;
                //this.Controls.Add(mylist);
            }
        }

        private void Button_Cofaj(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentOperationText.Text))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
            }
            else { }
        }
        private void Button_ClickCzysc(object sender, RoutedEventArgs e)
        {
            ResultText.Text = "0";
            CurrentOperationText.Text = string.Empty;
        }
        private void Button_ClickCzyscHistorie(object sender, RoutedEventArgs e)
        {
            ListaHistorii.Items.Clear();
        }

        private bool ContainsOperation(string operation)
            => operation.Contains('+') || operation.Contains('*') || operation.Contains(':') || operation.Contains('²') || operation.Contains('√') || operation.Contains('/');//usunięto || operation.Contains('-')
        private bool ContainsOperationNoPlusNoMinus(string operation)
            => Regex.Matches(operation, "[+]").Count > 1 || Regex.Matches(operation, "[-]").Count > 1 || operation.Contains('*') || operation.Contains(':') || operation.Contains('²') || operation.Contains('√') || operation.Contains('/');
        private bool EndsWithOperation(string operation)
            => operation.EndsWith("+") || operation.EndsWith("-") || operation.EndsWith("*") || operation.EndsWith(":");
        private void SprawdzCzyNieZero(string operation)
        {
            string obliczIWpisz = CalculateResult(operation).ToString("G15");
            if (CurrentOperationText.Text.Contains(':'))
            {
                string[] elements = CurrentOperationText.Text.Split(':');

                if (elements[1].Contains(','))
                {
                    if (elements[1].Contains('√') || elements[1].Contains('²'))
                    {
                        if (CalculateResult(elements[1]) == 0)
                        {
                            ResultText.Text = ZeroNIE;
                            CurrentOperationText.Text = string.Empty;
                        }
                        else
                        {
                            ResultText.Text = obliczIWpisz;
                            ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                            CurrentOperationText.Text = string.Empty;
                        }
                    }
                    else if (Convert.ToDouble(elements[1]) * 1 == 0)
                    {
                        ResultText.Text = ZeroNIE;
                        CurrentOperationText.Text = string.Empty;
                    }
                    else
                    {
                        ResultText.Text = obliczIWpisz;
                        ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                        CurrentOperationText.Text = string.Empty;
                    }
                }
                else
                {
                    if (elements[1].ToString() == "√")
                    {
                        ResultText.Text = ZeroNIE;
                        CurrentOperationText.Text = string.Empty;
                    }
                    else if (elements[1].Contains('√') || elements[1].Contains('²'))
                    {
                        if (CalculateResult(elements[1]) == 0)
                        {
                            ResultText.Text = ZeroNIE;
                            CurrentOperationText.Text = string.Empty;
                        }
                        else
                        {
                            ResultText.Text = obliczIWpisz;
                            ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                            CurrentOperationText.Text = string.Empty;
                        }
                    }
                    else if (!(elements[1].Contains('√') || elements[1].Contains('²')) && Convert.ToDouble(elements[1]) * 1 == 0)
                    {
                        ResultText.Text = ZeroNIE;
                        CurrentOperationText.Text = string.Empty;
                    }
                    else
                    {
                        ResultText.Text = obliczIWpisz;
                        ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                        CurrentOperationText.Text = string.Empty;
                    }
                }
            }
            else if (CurrentOperationText.Text.Contains('/'))
            {
                string[] elements = CurrentOperationText.Text.Split('/');

                if (Convert.ToDouble(elements[1]) * 1 == 0)
                {
                    ResultText.Text = ZeroNIE;
                    CurrentOperationText.Text = string.Empty;
                }
                else
                {
                    ResultText.Text = CalculateResult(operation).ToString();
                    ListaHistorii.Items.Add(CurrentOperationText.Text + " = " + ResultText.Text);
                    CurrentOperationText.Text = string.Empty;
                }
            }
        }
        private bool CzyNieMaZnakow(string operation)
            => (Regex.Matches(operation, "[+]").Count < 2 && Regex.Matches(operation, "[-]").Count < 1 && !ContainsOperationNoPlusNoMinus(operation)) || (Regex.Matches(operation, "[-]").Count < 2 && Regex.Matches(operation, "[+]").Count < 1 && !ContainsOperationNoPlusNoMinus(operation));//bez znaków
        private bool CzyMaZnaki(string operation)
            => Regex.Matches(operation, "[+]").Count > 1 || Regex.Matches(operation, "[-]").Count > 1 || operation.Contains('*') || operation.Contains(':') || operation.Contains('√') || operation.Contains('²') || operation.Contains("1/");//ze znakami

        private double CalculateResult(string operation)
        {
            if (operation.Contains('E'))
            {
                if (operation.Contains("E+"))
                {
                    if (Regex.Matches(operation, "[+]").Count == 2)
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('+', '√', '²');
                                string firstNum = string.Join("+", elements.Take(2));

                                if (string.IsNullOrEmpty(elements[2]))
                                {
                                    return Convert.ToDouble(firstNum)
                                           + Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(firstNum)
                                           + (Convert.ToDouble(elements[2])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('+', '√');
                                string firstNum = string.Join("+", elements.Take(2));

                                if (string.IsNullOrEmpty(elements[2]))
                                {
                                    return Convert.ToDouble(firstNum)
                                           + Math.Sqrt(Convert.ToDouble(elements[3]));
                                }
                                else
                                {
                                    return Convert.ToDouble(firstNum)
                                           + (Convert.ToDouble(elements[2])
                                              * Math.Sqrt(Convert.ToDouble(elements[3])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('+', '²');
                            string firstNum = string.Join("+", elements.Take(2));

                            return Convert.ToDouble(firstNum)
                                    + Math.Pow(Convert.ToDouble(elements[2]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split(new[] { '+' }, 3);

                            if (string.IsNullOrEmpty(elements[2]))
                            {
                                elements[2] = elements[0] + elements[1];
                            }
                            string firstNum = string.Join("+", elements.Take(2));

                            return Convert.ToDouble(firstNum)
                                   + Convert.ToDouble(elements[2]);
                        }
                    }
                    else if (operation.Contains('-'))
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('-', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           - Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           - (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('-', '√');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           - Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           - (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('-', '²');

                            return Convert.ToDouble(elements[0])
                                   - Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split('-');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0] ;
                            }
                            return Convert.ToDouble(elements[0])
                                   - Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains('*'))
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('*', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           * (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('*', '√');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           * Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           * (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('*', '²');
                            return Convert.ToDouble(elements[0])
                                   * Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split('*');

                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }

                            return Convert.ToDouble(elements[0]) * Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains(':'))
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split(':', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           / (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split(':', '√');

                                if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                                {
                                    return Convert.ToDouble(elements[0])
                                           / Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           / (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split(':', '²');
                            return Convert.ToDouble(elements[0])
                                   / Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split(':');

                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }

                            return Convert.ToDouble(elements[0])
                                   / Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains('/'))
                    {
                        string[] elements = operation.Split('/');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0])
                               / Convert.ToDouble(elements[1]);

                    }
                    else if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('√', '²');
                            if (string.IsNullOrEmpty(elements[0]))
                            {
                                return Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('√');

                            if (string.IsNullOrEmpty(elements[0]))
                            {
                                return Math.Sqrt(Convert.ToDouble(elements[1]));
                            }

                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Convert.ToDouble(elements[1]));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('²');

                        return Math.Pow(Convert.ToDouble(elements[0]), 2);
                    }
                }

                else if (operation.Contains("E-"))
                {
                    if (Regex.Matches(operation, "[-]").Count == 2)
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('-', '√', '²');
                                string firstNum = string.Join("-", elements.Take(2));

                                if (string.IsNullOrEmpty(elements[2]))
                                {
                                    return Convert.ToDouble(firstNum)
                                           - Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(firstNum)
                                           - (Convert.ToDouble(elements[2])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('-', '√');
                                string firstNum = string.Join("-", elements.Take(2));

                                if (string.IsNullOrEmpty(elements[2]))
                                {
                                    return Convert.ToDouble(firstNum)
                                           - Math.Sqrt(Convert.ToDouble(elements[3]));
                                }
                                else
                                {
                                    return Convert.ToDouble(firstNum)
                                           - (Convert.ToDouble(elements[2])
                                              * Math.Sqrt(Convert.ToDouble(elements[3])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('-', '²');
                            string firstNum = string.Join("-", elements.Take(2));

                            return Convert.ToDouble(firstNum)
                                    - Math.Pow(Convert.ToDouble(elements[2]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split(new[] { '-' }, 3);

                            if (string.IsNullOrEmpty(elements[2]))
                            {
                                return 0;
                            }
                            else
                            {
                                string firstNum = string.Join("-", elements.Take(2));

                                return Convert.ToDouble(firstNum) - Convert.ToDouble(elements[2]); //Convert.ToDouble(firstNum) + Convert.ToDouble(secondNum);
                            }
                        }
                    }
                    else if (operation.Contains('+'))//w wypadku notacji np. "1E-15 + coś tam"
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('+', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           + Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           + (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('+', '√');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           + Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           + (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('+', '²');

                            return Convert.ToDouble(elements[0])
                                   + Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split('+');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }
                            return Convert.ToDouble(elements[0])
                                   + Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains('*'))
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('*', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           * (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('*', '√');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           * Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           * (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('*', '²');
                            return Convert.ToDouble(elements[0])
                                   * Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split('*');

                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }

                            return Convert.ToDouble(elements[0]) * Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains(':'))
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split(':', '√', '²');

                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           / (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[3]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split(':', '√');

                                if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                                {
                                    return Convert.ToDouble(elements[0])
                                           / Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           / (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split(':', '²');
                            return Convert.ToDouble(elements[0])
                                   / Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else
                        {
                            string[] elements = operation.Split(':');

                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }

                            return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);
                        }
                    }
                    else if (operation.Contains('/'))
                    {
                        string[] elements = operation.Split('/');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);

                    }
                    else if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('√', '²');
                            if (string.IsNullOrEmpty(elements[0]))
                            {
                                return Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('√');

                            if (string.IsNullOrEmpty(elements[0]))
                            {
                                return Math.Sqrt(Convert.ToDouble(elements[1]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Convert.ToDouble(elements[1]));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('²');

                        return Math.Pow(Convert.ToDouble(elements[0]), 2);
                    }
                }
            }
            else if (operation.StartsWith("-"))
            {
                if (operation.Contains('+'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('+', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       + Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       + (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('+', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       + Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       + (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('+', '²');
                        return Convert.ToDouble(elements[0]) + Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split('+');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) + Convert.ToDouble(elements[1]);
                    } // działa tak tlko gdy wymusimy postawienie plusa przy liczbie z minusem, blokuje to funkcja if(Contains operation) to oblicz bezzastanowienia xD
                }

                else if (Regex.Matches(operation, "[-]").Count == 2) /// dlaczego by niemogł być taki warunek w w warunku else w tej funkcji, a pojedyńczy minus na samym końcu i/więc dlaczego warunek z poj. - nie może być niżej
                {                                                    /// niż jest aktualnie (tam na dole)
                    string[] elements = operation.Split(new[] { '-' }, 3);

                    if (string.IsNullOrEmpty(elements[2]))
                    {
                        elements[2] = elements[0] + elements[1];
                    }

                    string firstNum = string.Join("-", elements.Take(2));

                    return Convert.ToDouble(firstNum) - Convert.ToDouble(elements[2]);
                }

                else if (operation.Contains('*'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('*', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('*', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('*', '²');
                        return Convert.ToDouble(elements[0]) * Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split('*');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) * Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains(':'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split(':', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       / (Convert.ToDouble(elements[1])
                                          / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split(':', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       / Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       / (Convert.ToDouble(elements[1])
                                          / Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split(':', '²');
                        return Convert.ToDouble(elements[0]) / Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split(':');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains('/'))
                {
                    string[] elements = operation.Split('/');

                    if (string.IsNullOrEmpty(elements[1]))
                    {
                        elements[1] = elements[0];
                    }

                    return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);

                }

                else if (operation.Contains('√'))
                {
                    if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('√', '²');
                        if (string.IsNullOrEmpty(elements[0]))
                        {
                            return Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                        }
                        else
                        {
                            return Convert.ToDouble(elements[0])
                                   * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                        }
                    }
                    else
                    {
                        string[] elements = operation.Split('√');

                        if (string.IsNullOrEmpty(elements[0]))
                        {
                            return Math.Sqrt(Convert.ToDouble(elements[1]));
                        }
                        else if (elements[0] == "-")
                        {
                            return -1 * Math.Sqrt(Convert.ToDouble(elements[1]));
                        }
                        else if (string.IsNullOrEmpty(elements[1]))
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(elements[0])
                                   * Math.Sqrt(Convert.ToDouble(elements[1]));
                        }
                    }
                }

                else if (operation.Contains('²'))//może być potrzebny warunek że nie zawiera pozostałych (niepewny)!!!
                {
                    string[] elements = operation.Split('²');

                    return Math.Pow(Convert.ToDouble(elements[0]), 2);
                }
            }
            else
            {
                if (operation.Contains('+'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('+', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       + Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       + (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('+', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       + Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       + (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('+', '²');
                        return Convert.ToDouble(elements[0]) + Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split('+');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) + Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains('-'))
                {
                    if (operation.StartsWith("-"))
                    {
                        string[] elements = operation.Split('-');

                        elements[0] = "0";

                        return Convert.ToDouble(elements[0])
                                   - Convert.ToDouble(elements[1]);
                    }
                    else
                    {
                        if (operation.Contains('√'))
                        {
                            if (operation.Contains('²'))
                            {
                                string[] elements = operation.Split('-', '√', '²');
                                if (string.IsNullOrEmpty(elements[1]))
                                {
                                    return Convert.ToDouble(elements[0])
                                           - Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           - (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                                }
                            }
                            else
                            {
                                string[] elements = operation.Split('-', '√');

                                if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                                {
                                    return Convert.ToDouble(elements[0])
                                           - Math.Sqrt(Convert.ToDouble(elements[2]));
                                }
                                else
                                {
                                    return Convert.ToDouble(elements[0])
                                           - (Convert.ToDouble(elements[1])
                                              * Math.Sqrt(Convert.ToDouble(elements[2])));
                                }
                            }
                        }
                        else if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('-', '²');
                            return Convert.ToDouble(elements[0])
                                   - Math.Pow(Convert.ToDouble(elements[1]), 2);
                        }
                        else if (operation.Contains("/-") && !operation.StartsWith("-"))
                        {
                            string[] elements = operation.Split('/', '-');

                            if (string.IsNullOrEmpty(elements[2]))
                            {
                                return Convert.ToDouble(elements[0]) / 0;
                            }

                            return (Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[2])) * -1;
                        }
                        else
                        {
                            string[] elements = operation.Split('-');

                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                elements[1] = elements[0];
                            }

                            return Convert.ToDouble(elements[0])
                                   - Convert.ToDouble(elements[1]);
                        }
                    }
                }

                else if (operation.Contains('*'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split('*', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split('*', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       * Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       * (Convert.ToDouble(elements[1])
                                          * Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('*', '²');
                        return Convert.ToDouble(elements[0]) * Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split('*');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) * Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains(':'))
                {
                    if (operation.Contains('√'))
                    {
                        if (operation.Contains('²'))
                        {
                            string[] elements = operation.Split(':', '√', '²');
                            if (string.IsNullOrEmpty(elements[1]))
                            {
                                return Convert.ToDouble(elements[0])
                                       / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       / (Convert.ToDouble(elements[1])
                                          / Math.Sqrt(Math.Pow(Convert.ToDouble(elements[2]), 2)));
                            }
                        }
                        else
                        {
                            string[] elements = operation.Split(':', '√');

                            if (string.IsNullOrEmpty(elements[1]))                          //Jeśli nic nie ma przed znakiem działania (.Split(...))
                            {
                                return Convert.ToDouble(elements[0])
                                       / Math.Sqrt(Convert.ToDouble(elements[2]));
                            }
                            else
                            {
                                return Convert.ToDouble(elements[0])
                                       / (Convert.ToDouble(elements[1])
                                          / Math.Sqrt(Convert.ToDouble(elements[2])));
                            }
                        }
                    }
                    else if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split(':', '²');
                        return Convert.ToDouble(elements[0]) / Math.Pow(Convert.ToDouble(elements[1]), 2);
                    }
                    else
                    {
                        string[] elements = operation.Split(':');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains('/'))
                {
                    if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('/', '²');

                        return Convert.ToDouble(elements[0]) / (Math.Pow(Convert.ToDouble(elements[1]), 2));
                    }
                    else
                    {
                        string[] elements = operation.Split('/');

                        if (string.IsNullOrEmpty(elements[1]))
                        {
                            elements[1] = elements[0];
                        }

                        return Convert.ToDouble(elements[0]) / Convert.ToDouble(elements[1]);
                    }
                }

                else if (operation.Contains('√'))
                {
                    if (operation.Contains('²'))
                    {
                        string[] elements = operation.Split('√', '²');
                        if (string.IsNullOrEmpty(elements[0]))
                        {
                            return Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                        }
                        else
                        {
                            return Convert.ToDouble(elements[0])
                                   * Math.Sqrt(Math.Pow(Convert.ToDouble(elements[1]), 2));
                        }
                    }
                    else
                    {
                        string[] elements = operation.Split('√');

                        if (string.IsNullOrEmpty(elements[0]))
                        {
                            return Math.Sqrt(Convert.ToDouble(elements[1]));
                        }
                        else if (string.IsNullOrEmpty(elements[1]))
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToDouble(elements[0])
                                   * Math.Sqrt(Convert.ToDouble(elements[1]));
                        }
                    }
                }

                else if (operation.Contains('²'))//może być potrzebny warunek że nie zawiera pozostałych (niepewny)!!!
                {
                    string[] elements = operation.Split('²');

                    return Math.Pow(Convert.ToDouble(elements[0]), 2);
                }

            }

            return default;
        }
        //Funkcja: po naciśnięciu na textbox ResultText prawym przyciskiem myszy jego zawartość zostaje skopiowana do schowka, używa eventu MouseLeftButtonUp
        private void ResultText_Kopiuj(object sender, RoutedEventArgs e)
        {
            //używa metody obiektu schowka
            Clipboard.SetText(ResultText.Text);

            //to pozwoli na późniejsze wklejenie ZE schowka DO pola tekstowego, np. z kalku do zadania, ale trzeba określić konkretne miejsce wklejenia
            //ResultText.Text = Clipboard.GetText();
        }
    }
}