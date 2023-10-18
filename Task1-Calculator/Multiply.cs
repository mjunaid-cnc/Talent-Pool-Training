using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Calculator
{
    public class Multiply : Calculator
    {
        public Multiply(decimal _n1, decimal _n2) : base(_n1, _n2)
        {
        }

        public decimal MultiplyNumbers()
        {
            decimal result = 0;
            for (int i = 0; i < Math.Abs(n2); i++)
            {
                result += n1;
            }
            if ((n1 < 0 && n2 > 0) || (n2 < 0 && n1 > 0)) return -result;
            else if (n1 < 0 && n2 < 0) return Math.Abs(result);
            else return result;
        }
    }
}
