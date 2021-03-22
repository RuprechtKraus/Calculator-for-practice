using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {
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

            expression = RPN.ExpressionToList(inputString);
            RPN.MakeReversePolishNotation(expression, out List<string> RevPolishNotation);
            float result = RPN.Calculate(RevPolishNotation);

            Console.WriteLine("Result: " + result); 
        }
    }
}
