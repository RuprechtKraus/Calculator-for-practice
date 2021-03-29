using System;
using System.Collections.Generic;

namespace CalculatorLib
{
    public class RCalculator : ICalculator
    {
        private readonly IParser _parser;

        public RCalculator(IParser parser)
        {
            _parser = parser;
        }

        public float Calculate(string expression)
        {
            _parser.Parse(expression);
            return _parser.Calculate();
        }
    }
}
