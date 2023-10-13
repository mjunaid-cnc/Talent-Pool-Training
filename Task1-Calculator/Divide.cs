using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Calculator
{
    public class Divide : Calculator
    {
        public Divide(decimal _n1, decimal _n2) : base(_n1, _n2)
        {
        }

        public void DivideNumbers()
        {
            decimal quotient = 0; decimal remainder = 0;
            decimal diff = Math.Abs(n1) - Math.Abs(n2);

            if (diff < 0)
            {
                remainder = n1;
            }
            while (diff >= 0)
            {
                remainder = diff;
                diff = diff - Math.Abs(n2);
                quotient += 1;
            }
            if ((n1 < 0 && n2 > 0) || (n2 < 0 && n1 > 0))
            {
                if (remainder > 0)
                    Console.WriteLine("The division of the 2 numbers results in " + -quotient + " as the quotient and " + remainder + "/" + Math.Abs(n2) + " as the remainder");
                else
                    Console.WriteLine("The division of the 2 numbers results in " + -quotient);
            }
            else if (n1 < 0 && n2 < 0)
            {
                if (remainder > 0)
                    Console.WriteLine("The division of the 2 numbers results in " + quotient + " as the quotient and " + remainder + "/" + Math.Abs(n2) + " as the remainder");
                else
                    Console.WriteLine("The division of the 2 numbers results in " + quotient);
            }
            else
            {
                if (remainder > 0)
                    Console.WriteLine("The division of the 2 numbers results in " + quotient + " as the quotient and " + remainder + "/" + Math.Abs(n2) + " as the remainder");
                else
                    Console.WriteLine("The division of the 2 numbers results in " + quotient);
            }
        }
    }
}
