using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Calculator
{
    public class Add : Calculator
    {
        public Add(decimal _n1, decimal _n2) : base(_n1, _n2)
        {
        }

        public decimal AddNumbers()
        {
            return n1 + n2;
        }
    }
}
