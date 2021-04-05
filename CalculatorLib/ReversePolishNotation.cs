using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public class ReversePolishNotation : IParser
    {
        static readonly List<string> opLevelOne = new List<string>()
        {
            "*",
            "/"
        };
        static readonly List<string> opLevelTwo = new List<string>()
        {
            "+",
            "-"
        };

        private List<string> _rpnExpression = new List<string>();
        private readonly List<IOperation> _supportedOperations = new List<IOperation>()
        { 
            new Addition(),
            new Substraction(),
            new Multiplication(),
            new Division()
        };

        public List<string> RpnExpression => _rpnExpression;

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
            _rpnExpression = new List<string>();
            Stack<string> operations = new();

            foreach (var item in expr)
            {
                if (float.TryParse(item, out _))
                {
                    _rpnExpression.Add(item);
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
    }
}
