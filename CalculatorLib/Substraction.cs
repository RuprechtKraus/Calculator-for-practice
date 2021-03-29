using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public class Substraction : IOperation
    {
        public string OperationCode => "-";

        public float Apply(float left, float right)
        {
            return left - right;
        }
    }
}
