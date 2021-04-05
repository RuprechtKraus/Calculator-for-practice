using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CalculatorLib;

namespace Calculator
{
    class Program
    {
        static public List<string> ExpressionToList(string expression)
        {
            List<string> expr = new();
            int lastNumberCell = 0;

            expr.Add("");
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsDigit(expression[i]))
                {
                    expr[lastNumberCell] += expression[i];
                }
                else
                {
                    expr.Add(expression[i].ToString());
                    expr.Add("");
                    lastNumberCell = expr.Count - 1;
                }
            }

            expr.RemoveAll(item => item == "");
            return expr;
        }

        static void Main(string[] args)
        {
            ReversePolishNotation rpn = new();
            RCalculator MyCalc = new(rpn);

            string inputString = Console.ReadLine();
            /* Проверка выражения на верную структуру */
            if (Regex.IsMatch(inputString, @"[^()0-9*/+-]|[*/+-]{2,}|\A[*/+-]|[*/+-]\z"))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(-1);
            }

            float result = 0;
            try
            {
                result = MyCalc.Calculate(inputString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }

            Console.WriteLine("Result: {0}", result);
        }
    }
}
