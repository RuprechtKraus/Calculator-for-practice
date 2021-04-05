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
            ReversePolishNotation rpn = new ReversePolishNotation();
            RCalculator MyCalc = new RCalculator(new List<IOperation>() 
            { 
                new Addition(),
                new Substraction(),
                new Multiplication(),
                new Division()
            });

            string inputString = Console.ReadLine();
            /* Проверка выражения на верную структуру */
            if (Regex.IsMatch(inputString, @"[^()0-9*/+-]|[*/+-]{2,}|\A[*/+-]|[*/+-]\z"))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(-1);
            }

            rpn.Parse(inputString);
            List<string> parsedExpression = rpn.RpnExpression;
            List<string> executionList = new List<string>();
            float leftOp = 0;
            float rightOp = 0;
            float result = 0;
            string operation = "";

            foreach (var item in parsedExpression)
            {
                if (!float.TryParse(item, out _)) //Если item не парсится во float, то item это символ операции
                {
                    leftOp = float.Parse(executionList[^2]);
                    rightOp = float.Parse(executionList[^1]);
                    operation = item;

                    executionList.RemoveAt(executionList.Count - 2);
                    executionList.RemoveAt(executionList.Count - 1);

                    try
                    {
                        result = MyCalc.Calculate(leftOp, rightOp, operation);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e.Message);
                        Environment.Exit(-1);
                    }
                    executionList.Add(result.ToString());
                }
                else
                {
                    executionList.Add(item);
                }
            }

            result = float.Parse(executionList[0]);
            Console.WriteLine("Result: {0}", result);
        }
    }
}
