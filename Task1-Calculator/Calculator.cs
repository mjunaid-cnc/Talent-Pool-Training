using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Calculator
{
    public abstract class Calculator
    {
        public decimal n1, n2;

        public Calculator(decimal _n1, decimal _n2)
        {
            n1 = _n1;
            n2 = _n2;
        }
    }
}
