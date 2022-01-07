using Loyc;
using Loyc.Collections;
using Loyc.Syntax;
using Loyc.Syntax.Les;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Projekt
{
    public partial class WykresyFunkcji : Form
    {
        BackgroundWorker _bw = new BackgroundWorker();
        OutputState _outState;    // bieżące dane, zakresy i bitmapy używane przez wątek GUI
        DirectBitmap _prevBitmap; // bitmap

        
        private const string cbVariables = "x=1";

        public WykresyFunkcji()
        {
            InitializeComponent();

            CurrentOperationText.Text = "sin(x) + x^2/10 - 1";
            txtZakres.Text = "-20..20;-20..20";

            // Przygotowanie do konstruowania funkcji na wykresie
            _bw.DoWork += (s, e) => {
                var state = (OutputState)(e.Result = e.Argument);
                try
                {
                    foreach (var c in state.Calcs) c.Run(); // wykonywanie obliczeń
                    state.RenderAll();
                }
                catch
                {
                    if (state.Bitmap != _outState?.Bitmap)
                        state.Bitmap.Dispose(); // bitmapa nie będzie użyta
                    //txtWynik.Text = "Błędne wyrażenie";
                }
            };
            _bw.RunWorkerCompleted += (s, e) => {
                if (panelError.Visible = (e.Error != null))
                {
                    txtWynik.Text = "Błąd";
                }
                else
                {
                    _outState = (OutputState)e.Result;
                    if (_prevBitmap != _outState.Bitmap)
                    {
                        using (_prevBitmap)
                            _prevBitmap = _outState.Bitmap;
                    }
                    graphPanel.Image = _prevBitmap.Bitmap;
                }
                if (_refreshRequested)
                {
                    _refreshRequested = false;
                    RefreshDisplay();
                }
            };
        }

        private void CalcForm_Load(object sender, EventArgs e)
        {
            SetUpTextBox(CurrentOperationText, "Formulas", "sin(x) + x**2/10 - 1");

            SetUpTextBox(txtZakres, "Ranges", "-20..20;-20..20\n");

            CurrentOperationText.SelectAll();

            RefreshDisplay();
        }

        private void SetUpTextBox(TextBox textBox, string cfgSection, string defaultList)
        {
            string savedData = Properties.Settings.Default[cfgSection]?.ToString();
            if (string.IsNullOrEmpty(savedData))
                savedData = defaultList;
            textBox.KeyPress += (s, e) => {
                if (e.KeyChar == '\r')
                {
                    Properties.Settings.Default.Save();
                    e.Handled = true;
                }
            };
            textBox.Resize += (s, e) => {
                textBox.Text = textBox.Tag as string ?? textBox.Text;
            };
            textBox.TextChanged += (s, e) => {
                textBox.Tag = textBox.Text;
                RefreshDisplay();
                lblMouseOver.Visible = false;
            };
        }

        private static void AddHistory(ComboBox comboBox, string text, bool permanent)
        {
            text = text.TrimEnd();
            if (permanent)
            {
                int i = comboBox.Items.IndexOf(text);
                if (i > -1)
                    comboBox.Items.RemoveAt(i);
            }
            else
            {
                text += " ";
            }
            if (comboBox.Items.Count > 0)
            {
                if (comboBox.Items[0].ToString() == text)
                    return;
                if (comboBox.Items[0].ToString().EndsWith(" "))
                    comboBox.Items.RemoveAt(0);
            }
            comboBox.Items.Insert(0, text);

            while (comboBox.Items.Count > 100)
                comboBox.Items.RemoveAt(100);
        }

        //private void CalcForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    //SaveComboBox(CurrentOperationText, "Funkcje");
        //    //SaveComboBox(cbVariables, "Variables");
        //    SaveComboBox(txtZakres, "Ranges", false);
        //    Properties.Settings.Default.Save();
        //}

        private void SaveComboBox(ComboBox comboBox, string cfgSection, bool saveTextAsPermanent = true)
        {
            AddHistory(comboBox, comboBox.Text, saveTextAsPermanent);
            Properties.Settings.Default[cfgSection] = string.Join("\n", comboBox.Items.Cast<string>());
        }

        bool _refreshRequested = false;

        OutputState PrepareCalculators()
        {
            try
            {
                // Parse the three combo boxes and build a dictionary of variables
                var exprs = ParseExprs("Formula", CurrentOperationText.Text);
                var variables = ParseExprs("Variables", string.Format(CultureInfo.InvariantCulture,
                    "pi={0};tau={1};e={2};phi=1.6180339887498948; {3}", Math.PI, Math.PI * 2, Math.E, cbVariables));
                var ranges = ParseExprs("Range", txtZakres.Text);
                var varDict = CalculatorCore.ParseVarList(variables);

                // Get display range.
                Size size = graphPanel.ClientSize;
                LNode xRangeExpr = ranges.TryGet(0, null), yRangeExpr = ranges.TryGet(1, null);
                var xRange = GraphRange.New("x", xRangeExpr, size.Width, varDict);
                var yRange = GraphRange.New("y", yRangeExpr ?? xRangeExpr, size.Height, varDict);
                var zRange = GraphRange.New("z", ranges.TryGet(2, null), 0, varDict);
                yRange.AutoRange = (yRangeExpr == null); // For functions of x only; ignored if y is used

                var calcs = exprs.Select(e => CalculatorCore.New(e, varDict, xRange, yRange)).ToList();

                //panelError.Visible = false;

                return new OutputState(calcs, xRange, yRange, zRange, _prevBitmap);
            }
            catch (Exception exc)
            {
                //ShowError("(Immediate) {msg}".Localized("msg", exc.Message));
                txtWynik.Text = "Błędne wyrażenie";
                return null;
            }
        }

        void RefreshDisplay()
        {
            OutputState os = PrepareCalculators();
            if (os != null)
            {
                // Refresh result label immediately
                string resultText = null;
                try
                {
                    foreach (var c in os.Calcs)
                    {
                        resultText = resultText == null ? "" : resultText + ", ";
                        resultText += CalculatorCore.Eval(c.Expr, c.Vars);
                    }
                    txtWynik.Enabled = true;
                }
                catch (Exception e)
                {
                    if (resultText == null)
                        resultText = e.Message;
                    txtWynik.Enabled = false;
                }
                txtWynik.Text = resultText;

                if (_bw.IsBusy)
                {
                    _refreshRequested = true;
                    return;
                }

                os.Bitmap = new DirectBitmap(graphPanel.ClientSize);

                _bw.RunWorkerAsync(os);
            }
        }

        private CalcRange DecodeRange(string rangeName, LNode range, int width)
        {
            if (range == null)
                return new CalcRange(-1, 1, width);
            if (range.Calls(CodeSymbols.Colon, 2) && range[0].IsId && string.Compare(range[0].Name.Name, rangeName, true) == 0)
                range = range[1];
            if (range.Calls(CodeSymbols.Sub, 2) || range.Calls(CodeSymbols.DotDot, 2))
                return new CalcRange(Convert.ToDouble(range[0].Value),
                                     Convert.ToDouble(range[1].Value), width);
            if (range.Calls("'..-", 2))
                return new CalcRange(Convert.ToDouble(range[0].Value),
                                     -Convert.ToDouble(range[1].Value), width);
            throw new FormatException("Invalid range for {axis}: {range}".Localized("axis", rangeName, "range", range));
        }

        static List<LNode> ParseExprs(string fieldName, string text)
        {
            // Rozdzielenie rzeczy takich jak *- na dwa oddzielne operatory (* -)), przyrównanie ^ do **
            text = System.Text.RegularExpressions.Regex.Replace(text, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\^", "**");

            var errorHandler = MessageSink.FromDelegate((severity, ctx, fmt, args) => {
                if (severity >= Severity.Error)
                {
                    var msg = fieldName + ": " + fmt.Localized(args);
                    if (ctx is SourceRange)
                        msg += $"\r\n{text}\r\n{new string('-', -1)}^";
                    throw new LogException(severity, ctx, msg);
                }
            });
            return Les3LanguageService.Value.Parse(text, errorHandler).ToList();
        }

        private void picPanel_Resize(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        bool _dragging = false;
        Point _dragStartPoint;
        CalcRange _originalXRange, _originalYRange;
        string _originalZRange, _originalRanges;

        private void picPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (_outState != null)
            {
                _dragging = graphPanel.Capture = (e.Button == MouseButtons.Left);
                _dragStartPoint = e.Location;
                _originalRanges = txtZakres.Text;
                _originalXRange = _outState.XRange;
                _originalYRange = _outState.YRange;
                _originalZRange = _outState.ZRange.RangeExpr?.Range.SourceText.ToString() ?? "";
            }
        }
        private void picPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                lblMouseOver.Visible = false;
                CalcRange newX = _originalXRange.DraggedBy(e.Location.X - _dragStartPoint.X);
                CalcRange newY = _originalYRange.DraggedBy(-(e.Location.Y - _dragStartPoint.Y));
                SetRanges(newX, newY, _originalZRange);
            }
            else
            {
                lblMouseOver.Text = _outState?.GetMouseOverText(e.Location) ?? "";
                lblMouseOver.Visible = lblMouseOver.Text.Length != 0;
            }
        }
        private void picPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = graphPanel.Capture = false;
        }
        private void picPanel_MouseLeave(object sender, EventArgs e)
        {
            lblMouseOver.Visible = false;
            if (_dragging)
            {
                _dragging = false;
                txtZakres.Text = _originalRanges;
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            Zoom(1 / Math.Sqrt(2));
        }
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            Zoom(Math.Sqrt(2));
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (graphPanel.Image != null)
                Clipboard.SetImage(graphPanel.Image);
        }

        private void button_Click_numberAndOther(object sender, EventArgs e)
        {
            //ResultText.Text = string.Empty;

            Button button = sender as Button;
            object currentNumber = button.Text;

            CurrentOperationText.Text += currentNumber;

        }

        private void kalkulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculator goKalkulator = new Calculator();
            goKalkulator.Show();
            this.Hide();
        }

        private void Button_Cofaj_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentOperationText.Text))
            {
                CurrentOperationText.Text = CurrentOperationText.Text.Remove(CurrentOperationText.Text.Length - 1);
            }
            else { }
        }

        void SetRanges(CalcRange xRange, CalcRange yRange, string zRangeText)
        {
            // Refreshes display as side effect
            var newRanges = string.Format(CultureInfo.InvariantCulture, "{0:G8}..{1:G8}; {2:G8}..{3:G8}; {4}",
                            xRange.Lo, xRange.Hi, yRange.Lo, yRange.Hi, zRangeText);
            Trace.WriteLine(newRanges);
            txtZakres.Text = newRanges;
        }
        private void Zoom(double ratio)
        {
            if (_outState != null)
            {
                string zRange = _outState.ZRange.RangeExpr?.Range.SourceText.ToString() ?? "";
                CalcRange newX = _outState.XRange.ZoomedBy(ratio);
                CalcRange newY = _outState.YRange.ZoomedBy(ratio);
                SetRanges(newX, newY, zRange);
            }
        }
    }
}
