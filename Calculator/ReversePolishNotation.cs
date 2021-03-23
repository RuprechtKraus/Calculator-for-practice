using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class RPN
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

        static public void MakeReversePolishNotation(List<string> expression, out List<string> result)
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

        static public float Calculate(List<string> ReversePolishNotation)
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
    }
}
