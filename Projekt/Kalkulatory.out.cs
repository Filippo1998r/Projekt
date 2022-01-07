// Generated from Calculators.ecs by LeMP custom tool. LeMP version: 2.5.2.0
// Note: you can give command-line arguments to the tool via 'Custom Tool Namespace':
// --no-out-header       Suppress this message
// --verbose             Allow verbose messages (shown by VS as 'warnings')
// --timeout=X           Abort processing thread after X seconds (default: 10)
// --macros=FileName.dll Load macros from FileName.dll, path relative to this file 
// Use #importMacros to use macros in a given namespace, e.g. #importMacros(Loyc.LLPG);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyc;
using Loyc.Syntax;
using Loyc.Collections;
namespace Projekt
{
	using number = System.Double;   // Change this line to make a calculator for a different data type 
	class CalcRange
	{
		public number Lo;
		public number Hi;
		public int PxCount;
		// Generate a constructor and three public fields
		public CalcRange(number lo, number hi, int pxCount)
		{
			Lo = lo;
			Hi = hi;
			PxCount = pxCount;
			StepSize = (Hi - Lo) / Math.Max(PxCount - 1, 1);
		}
		public number StepSize;
		public number ValueToPx(number value) => (value - Lo) / (Hi - Lo) * PxCount;
		public number PxToValue(int px) => (number)px / PxCount * (Hi - Lo) + Lo;
		public number PxToDelta(int px) => (number)px / PxCount * (Hi - Lo);
		public CalcRange DraggedBy(int dPx) =>
		new CalcRange(Lo - PxToDelta(dPx), Hi - PxToDelta(dPx), PxCount);
		public CalcRange ZoomedBy(number ratio)
		{
			double mid = (Hi + Lo) / 2, halfSpan = (Hi - Lo) * ratio / 2;
			return new CalcRange(mid - halfSpan, mid + halfSpan, PxCount);
		}
	}

	// "alt class" generates an entire class hierarchy with base class CalculatorCore and 
	// read-only fields. Each "alternative" (derived class) is marked with the word "alt".
	abstract class CalculatorCore
	{
		static readonly Symbol sy_x = (Symbol)"x";
		// Base class constructor and fields
		public CalculatorCore(LNode Expr, Dictionary<Symbol, LNode> Vars, CalcRange XRange)
		{
			this.Expr = Expr;
			this.Vars = Vars;
			this.XRange = XRange;
		}

		public LNode Expr { get; private set; }
		public Dictionary<Symbol, LNode> Vars { get; private set; }
		public CalcRange XRange { get; private set; }
		public abstract CalculatorCore WithExpr(LNode newValue);
		public abstract CalculatorCore WithVars(Dictionary<Symbol, LNode> newValue);
		public abstract CalculatorCore WithXRange(CalcRange newValue);
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public LNode Item1
		{
			get
			{
				return Expr;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Dictionary<Symbol, LNode> Item2
		{
			get
			{
				return Vars;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public CalcRange Item3
		{
			get
			{
				return XRange;
			}
		}
		public object Results { get; protected set; }

		public abstract object Run();
		public abstract number? GetValueAt(int x, int y);

		public static CalculatorCore New(LNode expr, Dictionary<Symbol, LNode> vars, CalcRange xRange, CalcRange yRange)
		{
			// Find out if the expression uses the variable "y" (or is an equation with '=' or '==')
			// As an (unnecessary) side effect, this throws if an unreferenced var is used
			bool isEquation = expr.Calls(CodeSymbols.Assign, 2) || expr.Calls(CodeSymbols.Eq, 2), usesY = false;
			if (!isEquation)
			{
				LNode zero = LNode.Literal((double)0);
				Func<Symbol, double> lookup = null;
				lookup = name => name == sy_x ? 0 : Eval(vars[name], lookup);
				Eval(expr, lookup);
			}
			
			return new Calculator2D(expr, vars, xRange);
		}

		// Parse the list of variables provided in the GUI
		public static Dictionary<Symbol, LNode> ParseVarList(IEnumerable<LNode> varList)
		{
			var vars = new Dictionary<Symbol, LNode>();
			foreach (LNode assignment in varList)
			{
				{
					LNode expr, @var;
					if (assignment.Calls(CodeSymbols.Assign, 2) && (@var = assignment.Args[0]) != null && (expr = assignment.Args[1]) != null)
					{
						if (!@var.IsId)
							throw new ArgumentException("Left-hand side of '=' must be a variable name: {0}".Localized(@var));

						// For efficiency, try to evaluate the expression in advance
						try { expr = LNode.Literal(Eval(expr, vars)); } catch { }   // it won't work if expression uses X or Y
						vars.Add(@var.Name, expr);
					}
					else
						throw new ArgumentException("Expected assignment expression: {0}".Localized(assignment));
				};
			}
			return vars;
		}

		public static number Eval(LNode expr, Dictionary<Symbol, LNode> vars)
		{
			Func<Symbol, number> lookup = null;
			lookup = name => Eval(vars[name], lookup);
			return Eval(expr, lookup);
		}

		// Evaluates an expression
		public static number Eval(LNode expr, Func<Symbol, number> lookup)
		{
			if (expr.IsLiteral)
			{
				if (expr.Value is number)
					return (number)expr.Value;
				else
					return (number)Convert.ToDouble(expr.Value);
			}
			if (expr.IsId)
				return lookup(expr.Name);

			// expr must be a function or operator
			if (expr.ArgCount == 2)
			{
				{
					LNode a, b, hi, lo, tmp_10, tmp_11 = null;
					if (expr.Calls(CodeSymbols.Add, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Eval(a, lookup) + Eval(b, lookup);
					else if (expr.Calls(CodeSymbols.Mul, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Eval(a, lookup) * Eval(b, lookup);
					else if (expr.Calls(CodeSymbols.Sub, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Eval(a, lookup) - Eval(b, lookup);
					else if (expr.Calls(CodeSymbols.Div, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Eval(a, lookup) / Eval(b, lookup);
					else if (expr.Calls(CodeSymbols.Mod, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Eval(a, lookup) % Eval(b, lookup);
					else if (expr.Calls(CodeSymbols.Exp, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)Math.Pow(Eval(a, lookup), Eval(b, lookup));
					else if (expr.Calls((Symbol)"mod", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'MOD", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Mod(Eval(a, lookup), Eval(b, lookup));
					else if (expr.Calls((Symbol)"log", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Log(Eval(a, lookup), Eval(b, lookup));
				}
			}
			{
				LNode a, b, c, tmp_12;
				if (expr.Calls(CodeSymbols.Sub, 1) && (a = expr.Args[0]) != null) return -Eval(a, lookup);
				else if (expr.Calls(CodeSymbols.Add, 1) && (a = expr.Args[0]) != null) return Math.Abs(Eval(a, lookup));
				else if (expr.Calls(CodeSymbols.Not, 1) && (a = expr.Args[0]) != null) return Eval(a, lookup) == 0 ? (number)1 : (number)0;
				else if (expr.Calls(CodeSymbols.NotBits, 1) && (a = expr.Args[0]) != null) return (number)~(long)Eval(a, lookup);
				else if (expr.Calls(CodeSymbols.QuestionMark, 2) && (c = expr.Args[0]) != null && (tmp_12 = expr.Args[1]) != null && tmp_12.Calls(CodeSymbols.Colon, 2) && (a = tmp_12.Args[0]) != null && (b = tmp_12.Args[1]) != null)
					return Eval(c, lookup) != (number)0 ? Eval(a, lookup) : Eval(b, lookup);
				else if (expr.Calls((Symbol)"square", 1) && (a = expr.Args[0]) != null)
				{
					var n = Eval(a, lookup); return n * n;
				}
				else if (expr.Calls((Symbol)"sqrt", 1) && (a = expr.Args[0]) != null) return Math.Sqrt(Eval(a, lookup));
				else if (expr.Calls((Symbol)"sin", 1) && (a = expr.Args[0]) != null) return Math.Sin(Eval(a, lookup));
				else if (expr.Calls((Symbol)"cos", 1) && (a = expr.Args[0]) != null) return Math.Cos(Eval(a, lookup));
				else if (expr.Calls((Symbol)"tg", 1) && (a = expr.Args[0]) != null) return Math.Tan(Eval(a, lookup));
				else if (expr.Calls((Symbol)"cot", 1) && (a = expr.Args[0]) != null) return 1 / Math.Tan(Eval(a, lookup));
				else if (expr.Calls((Symbol)"sec", 1) && (a = expr.Args[0]) != null) return 1 / Math.Cos(Eval(a, lookup));
				else if (expr.Calls((Symbol)"csc", 1) && (a = expr.Args[0]) != null) return 1 / Math.Sin(Eval(a, lookup));
				else if (expr.Calls((Symbol)"ln", 1) && (a = expr.Args[0]) != null) return Math.Log(Eval(a, lookup));
				else if (expr.Calls((Symbol)"log", 1) && (a = expr.Args[0]) != null) return Math.Log10(Eval(a, lookup));
			}
			throw new ArgumentException("Expression not understood: {0}".Localized(expr));
		}

		static double Mod(double x, double y)
		{
			double m = x % y;
			return m + (m < 0 ? y : 0);
		}
		static double Factorial(double n) =>
		n <= 1 ? 1 : n * Factorial(n - 1);
		static double P(int n, int k) =>
		k <= 0 ? 1 : k > n ? 0 : n * P(n - 1, k - 1);
		static double C(ulong n, ulong k)
		{
			if (k > n)
				return 0;
			k = Math.Min(k, n - k);
			double result = 1;
			for (ulong d = 1; d <= k; ++d)
			{
				result *= n--;
				result /= d;
			}
			return result;
		}
		static Random _r = new Random();
	}
	// Derived class for 2D graphing calculator
	class Calculator2D : CalculatorCore
	{
		static readonly Symbol sy_x = (Symbol)"x";
		public Calculator2D(LNode Expr, Dictionary<Symbol, LNode> Vars, CalcRange XRange)
			 : base(Expr, Vars, XRange) { }
		public override CalculatorCore WithExpr(LNode newValue)
		{
			return new Calculator2D(newValue, Vars, XRange);
		}
		public override CalculatorCore WithVars(Dictionary<Symbol, LNode> newValue)
		{
			return new Calculator2D(Expr, newValue, XRange);
		}
		public override CalculatorCore WithXRange(CalcRange newValue)
		{
			return new Calculator2D(Expr, Vars, newValue);
		}
		public override object Run()
		{
			var results = new number[XRange.PxCount];
			number x = XRange.Lo;

			Func<Symbol, number> lookup = null;
			lookup = name => (name == sy_x ? x : Eval(Vars[name], lookup));

			for (int i = 0; i < results.Length; i++)
			{
				results[i] = Eval(Expr, lookup);
				x += XRange.StepSize;
			}
			return Results = results;
		}
		public override number? GetValueAt(int x, int _)
		{
				var tmp_14 = (uint)x;
				var r = ((number[])Results);
				return
				tmp_14 < (uint)r.Length ? r[x] : (number?)null;
		}
	}
}