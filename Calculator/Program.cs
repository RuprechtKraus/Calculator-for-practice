using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            List<string> expression;
            string inputString = Console.ReadLine();

            /* Проверка выражения на верную структуру */
            if (Regex.IsMatch(inputString, @"[^()0-9*/+-]|[*/+-]{2,}|\A[*/+-]|[*/+-]\z"))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(13);
            }

            expression = ExpressionToList(inputString);
            RPN.MakeReversePolishNotation(expression, out List<string> RevPolishNotation);
            float result = RPN.Calculate(RevPolishNotation);

            Console.WriteLine("Result: {0}", result); 
        }
    }
}
