using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public interface IParser
    {
        public abstract void Parse(string expression);
        public abstract float Calculate();
    }
}
