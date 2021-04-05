using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public class ReversePolishNotation : IParser
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

        private List<string> _rpnExpression = new();
        private readonly List<IOperation> _supportedOperations = new()
        { 
            new Addition(),
            new Substraction(),
            new Multiplication(),
            new Division()
        };

        public ReversePolishNotation() { }
        public ReversePolishNotation(List<IOperation> operations)
        {
            _supportedOperations = operations;
        }

        static private List<string> ExpressionToList(string expression)
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

        public void Parse(string expression)
        {
            List<string> expr = ExpressionToList(expression);
            _rpnExpression = new();
            Stack<string> operations = new();

            foreach (var item in expr)
            {
                if (float.TryParse(item, out _))
                {
                    _rpnExpression.Add(item);
                    continue;
                }

                if (_supportedOperations.Find(x => x.OperationCode == item) == null
                    && item != "(" && item != ")") //Поддерживается ли данная операция
                {
                    throw new Exception($"Operation {item} is not supported");
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
                        _rpnExpression.Add(op);
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
                        _rpnExpression.Add(operations.Pop());
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
                _rpnExpression.Add(operations.Pop());
        }

        public float Calculate()
        {
            List<string> result = new();
            IOperation operation;
            float leftOp = 0;
            float rightOp = 0;
            foreach (var item in _rpnExpression)
            {
                operation = _supportedOperations.Find(x => x.OperationCode == item);
                if (operation != null)
                {
                    leftOp = float.Parse(result[^2]);
                    rightOp = float.Parse(result[^1]);

                    if (operation.OperationCode == "/" && rightOp == 0)
                        throw new DivideByZeroException("You cannot divide by zero");

                    result.RemoveAt(result.Count - 2);
                    result.RemoveAt(result.Count - 1);

                    result.Add(operation.Apply(leftOp, rightOp).ToString());
                    continue;
                }
                else
                    result.Add(item);
            }

            return float.Parse(result[0]);
        }
    }
}
