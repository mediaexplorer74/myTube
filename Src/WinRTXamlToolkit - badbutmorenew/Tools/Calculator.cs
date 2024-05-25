// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.Calculator
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace WinRTXamlToolkit.Tools
{
  public static class Calculator
  {
    private static Func<double, double, double> AddFunc = (Func<double, double, double>) ((d0, d1) => d0 + d1);
    private static Func<double, double, double> SubtractFunc = (Func<double, double, double>) ((d0, d1) => d0 - d1);
    private static Func<double, double, double> MultiplyFunc = (Func<double, double, double>) ((d0, d1) => d0 * d1);
    private static Func<double, double, double> DivideFunc = (Func<double, double, double>) ((d0, d1) => d0 / d1);
    private static Func<double, double, double> ModuloFunc = (Func<double, double, double>) ((d0, d1) => d0 % d1);
    private static Func<double, double, double> PowerFunc = (Func<double, double, double>) ((d0, d1) => Math.Pow(d0, d1));
    private static Func<double, double, double> NumberFunc = (Func<double, double, double>) ((d0, d1) => d0);

    private static bool IsLeftFirst(
      this Func<double, double, double> f1,
      Func<double, double, double> f2)
    {
      return (!(f1 == Calculator.AddFunc) && !(f1 == Calculator.SubtractFunc) || !(f2 != Calculator.AddFunc) || !(f2 != Calculator.SubtractFunc)) && (!(f1 == Calculator.MultiplyFunc) && !(f1 == Calculator.DivideFunc) || !(f2 == Calculator.PowerFunc));
    }

    public static double Calculate(string expression)
    {
      double result;
      Calculator.TryCalculate(expression, out result, true);
      return result;
    }

    public static bool TryCalculate(string expression, out double result) => Calculator.TryCalculate(expression, out result, false);

    private static bool TryCalculate(string expression, out double result, bool throwOnError)
    {
      if (expression == null)
      {
        if (throwOnError)
          throw new ArgumentNullException(expression);
        result = double.NaN;
        return false;
      }
      expression = expression.Replace(" ", "");
      Stack<Calculator.Operation> operationStack = new Stack<Calculator.Operation>();
      int startIndex = 0;
      int length = expression.Length;
      if (length == 0)
      {
        if (throwOnError)
          throw new ArgumentException("Empty expression can't be calculated.");
        result = double.NaN;
        return false;
      }
      int parentheses = 0;
      bool flag = false;
      char ch1 = CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol[0] : '$';
      char ch2 = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator[0] : '.';
      char ch3 = CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator.Length == 1 ? CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator[0] : '\'';
      for (int index = 0; index < length; ++index)
      {
        char ch4 = expression[index];
        if (ch4 >= '0' && ch4 <= '9' || ch4 == 'E' || ch4 == 'e' || (int) ch4 == (int) ch2 || (int) ch4 == (int) ch3 || (int) ch4 == (int) ch1 || ch4 == '-' && index == startIndex && index + 1 < length && expression[index + 1] >= '0' && expression[index + 1] <= '9')
        {
          if (ch4 == 'E' || ch4 == 'e')
          {
            flag = true;
            if (index + 1 == length)
            {
              if (throwOnError)
                throw new FormatException(string.Format("{0} at position {1} is not an expected character at end of {2}", (object) ch4, (object) index, (object) expression));
              result = double.NaN;
              return false;
            }
            continue;
          }
          if (ch4 != '-')
            flag = true;
          if (index + 1 < length)
            continue;
        }
        else if (operationStack.Count == 0 && !flag && ch4 != '(')
        {
          if (throwOnError)
            throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", (object) ch4, (object) index, (object) expression));
          result = double.NaN;
          return false;
        }
        if (index + 1 == length)
        {
          double result1;
          if (flag)
          {
            string s = ch4 != ')' || parentheses != 1 ? expression.Substring(startIndex) : expression.Substring(startIndex, index - startIndex);
            if (!double.TryParse(s, out result1))
            {
              if (throwOnError)
                double.Parse(s);
              result = double.NaN;
              return false;
            }
          }
          else
          {
            if (operationStack.Count == 0)
            {
              if (throwOnError)
                throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", (object) ch4, (object) index, (object) expression));
              result = double.NaN;
              return false;
            }
            result1 = 0.0;
          }
          while (operationStack.Count > 0)
            result1 = operationStack.Pop().GetResult(result1);
          if (parentheses != 0 && (ch4 != ')' || parentheses != 1))
          {
            if (throwOnError)
              throw new FormatException(string.Format("Expression missing a closing parenthesis character - {0} ", (object) expression));
            result = double.NaN;
            return false;
          }
          result = result1;
          return true;
        }
        double result2;
        if (flag)
        {
          if (!double.TryParse(expression.Substring(startIndex, index - startIndex), out result2))
          {
            if (throwOnError)
              double.Parse(expression.Substring(startIndex, index - startIndex));
            result = double.NaN;
            return false;
          }
        }
        else
          result2 = 0.0;
        startIndex = index + 1;
        Func<double, double, double> func = (Func<double, double, double>) null;
        switch (ch4)
        {
          case '%':
            func = Calculator.ModuloFunc;
            break;
          case '(':
            if (operationStack.Count > 0 && operationStack.Peek().Function == Calculator.NumberFunc || flag)
            {
              if (throwOnError)
                throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", (object) ch4, (object) index, (object) expression));
              result = double.NaN;
              return false;
            }
            ++parentheses;
            break;
          case ')':
            while (operationStack.Count > 0 && operationStack.Peek().Parentheses == parentheses)
              result2 = operationStack.Pop().GetResult(result2);
            --parentheses;
            if (parentheses < 0)
            {
              if (throwOnError)
                throw new FormatException(string.Format("Closing unopened parenthesis at position {0} in {1}.", (object) index, (object) expression));
              result = double.NaN;
              return false;
            }
            if (index + 1 == length)
            {
              result = result2;
              return true;
            }
            operationStack.Push(new Calculator.Operation(result2, Calculator.NumberFunc, parentheses));
            break;
          case '*':
            func = Calculator.MultiplyFunc;
            break;
          case '+':
            func = Calculator.AddFunc;
            break;
          case '-':
            func = Calculator.SubtractFunc;
            break;
          case '/':
            func = Calculator.DivideFunc;
            break;
          case '^':
            func = Calculator.PowerFunc;
            break;
          default:
            if (throwOnError)
              throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", (object) ch4, (object) index, (object) expression));
            result = double.NaN;
            return false;
        }
        if (func != null)
        {
          if (index == length - 1 || expression[index + 1] == '+' || expression[index + 1] == '-' || expression[index + 1] == '*' || expression[index + 1] == '/' || expression[index + 1] == '%' || expression[index + 1] == '^' || !flag && operationStack.Count == 0)
          {
            if (throwOnError)
              throw new FormatException(string.Format("{0} at position {1} is not an expected character in {2}", (object) expression[index + 1], (object) (index + 1), (object) expression));
            result = double.NaN;
            return false;
          }
          while (operationStack.Count > 0 && operationStack.Peek().Parentheses == parentheses && operationStack.Peek().Function.IsLeftFirst(func))
            result2 = operationStack.Pop().GetResult(result2);
          operationStack.Push(new Calculator.Operation(result2, func, parentheses));
        }
        flag = false;
      }
      if (throwOnError)
        throw new InvalidOperationException();
      result = double.NaN;
      return false;
    }

    private struct Operation
    {
      private readonly double leftValue;
      public readonly Func<double, double, double> Function;
      public readonly int Parentheses;

      public double GetResult(double rightValue) => this.Function(this.leftValue, rightValue);

      public Operation(double leftValue, Func<double, double, double> function, int parentheses)
      {
        this.leftValue = leftValue;
        this.Function = function;
        this.Parentheses = parentheses;
      }
    }
  }
}
