using System;
using System.Collections.Generic;

namespace CalculatorLib
{
    public class RCalculator : ICalculator
    {
        private readonly List<IOperation> _availableOperations;

        public RCalculator(List<IOperation> operations)
        {
            _availableOperations = operations;
        }

        public float Calculate(float left, float right, string operation)
        {
            IOperation _operation = _availableOperations.Find(x => x.OperationCode == operation);
            if (_operation != null)
            {
                return _operation.Apply(left, right);
            }
            else
            {
                throw new InvalidOperationException($"Operation {operation} isn't supported");
            }
        }
    }
}
