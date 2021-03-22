using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {

        static readonly List<string> opLevelOne = new()
        {
            "*",
            "/"
        };
        static readonly List<string> opLevelTwo = new()
        {
            "+",
            "-"
        };
        static List<string> ExpressionToList(string expression)
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
        static void MakeReversePolishNotation(List<string> expression, out List<string> result)
        {
            result = new();
            Stack<string> operations = new();

            foreach (var item in expression)
            {
                if (float.TryParse(item, out _))
                {
                    result.Add(item);
                    continue;
                }

                if (operations.Count == 0 || item == "(")
                {
                    operations.Push(item);
                    continue;
                }

                if (item == ")")
                {
                    string op = operations.Pop();
                    while (op != "(")
                    {
                        result.Add(op);
                        op = operations.Pop();
                    }
                    continue;
                }

                string operation = operations.Peek();
                if (operation != "(")
                {
                    if (opLevelTwo.Contains(item) ||
                       (opLevelOne.Contains(item) && opLevelOne.Contains(operation)))
                    {
                        result.Add(operations.Pop());
                        operations.Push(item);
                        continue;
                    }

                    if (opLevelOne.Contains(item) && opLevelTwo.Contains(operation))
                    {
                        operations.Push(item);
                        continue;
                    }
                }
                else
                {
                    operations.Push(item);
                    continue;
                }
            }

            while (operations.Count > 0)
                result.Add(operations.Pop());
        }
        static float Calculate(List<string> ReversePolishNotation)
        {
            List<string> result = new();
            float leftOp = 0;
            float rightOp = 0;
            foreach (var item in ReversePolishNotation)
            {
                if (!float.TryParse(item, out _))
                {
                    leftOp = float.Parse(result[^2]);
                    rightOp = float.Parse(result[^1]);
                    result.RemoveAt(result.Count - 2);
                    result.RemoveAt(result.Count - 1);
                }

                switch (item)
                {
                    case "+":
                        result.Add((leftOp + rightOp).ToString());
                        break;
                    case "-":
                        result.Add((leftOp - rightOp).ToString());
                        break;
                    case "*":
                        result.Add((leftOp * rightOp).ToString());
                        break;
                    case "/":
                        result.Add((leftOp / rightOp).ToString());
                        break;
                    default:
                        result.Add(item);
                        break;
                }
            }

            return float.Parse(result[0]);
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
            MakeReversePolishNotation(expression, out List<string> RevPolishNotation);
            float result = Calculate(RevPolishNotation);

            Console.WriteLine("Result: " + result); 
        }
    }
}
