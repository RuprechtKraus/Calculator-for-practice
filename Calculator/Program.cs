using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {
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

        static void Main(string[] args)
        {
            List<string> opLevelOne = new();
            opLevelOne.Add("*");
            opLevelOne.Add("/");
            List<string> opLevelTwo = new();
            opLevelTwo.Add("+");
            opLevelTwo.Add("-");
            List<string> expression = new();
            List<string> output = new();
            Stack<string> operations = new();

            string inputString = Console.ReadLine();
            //Проверка выражения на верную структуру
            if (!Regex.IsMatch(inputString, @"[^()0-9*/+-]|[*/+-]{2,}"))//&& 
                //char.IsDigit(inputString[0]) &&
                //char.IsDigit(inputString[inputString.Length - 1]))
            {
                expression = ExpressionToList(inputString);
            }
            else
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(13);
            }

            foreach (var item in expression)
            {
                if (float.TryParse(item, out _))
                {
                    output.Add(item);
                    continue;
                }

                if (item == "(")
                {
                    operations.Push(item);
                    continue;
                }

                if (item == ")")
                {
                    string op = operations.Pop();
                    while (op != "(")
                    {
                        output.Add(op);
                        op = operations.Pop();
                    }
                    continue;
                }

                if (opLevelOne.Contains(item) || opLevelTwo.Contains(item))
                {
                    if (operations.Count == 0)
                    {
                        operations.Push(item);
                        continue;
                    }

                    string operation = operations.Peek();
                    if (opLevelTwo.Contains(item) && (opLevelOne.Contains(operation) || opLevelTwo.Contains(operation)) &&
                        operation != "(")
                    {
                        output.Add(operations.Pop());
                        operations.Push(item);
                        continue;
                    }

                    if (opLevelOne.Contains(item) && opLevelTwo.Contains(operation) &&
                        operation != "(")
                    {
                        operations.Push(item);
                        continue;
                    }

                    if (opLevelOne.Contains(item) && opLevelOne.Contains(operation) &&
                        operation != "(")
                    {
                        output.Add(operations.Pop());
                        operations.Push(item);
                        continue;
                    }

                    if (operation != "(")
                    {
                        output.Add(item);
                    }
                    else
                    {
                        operations.Push(item);
                    }
                }
            }

            while (operations.Count > 0)
            {
                output.Add(operations.Pop());
            }

            #region Вывод стека
            //Console.Write("Output: ");
            //output.ForEach(item => Console.Write(item + " "));
            //Console.WriteLine();
            #endregion

            List<string> result = new();
            float leftOp = 0;
            float rightOp = 0;
            foreach (var item in output)
            {
                if (!float.TryParse(item, out _))
                {
                    leftOp = float.Parse(result[result.Count - 2]);
                    rightOp = float.Parse(result[result.Count - 1]);
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

            Console.WriteLine("Result: " + result[0]); 
        }
    }
}
