using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public interface ICalculator
    {
        float Calculate(float left, float right, string operation);
    }
}
