namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal n1, n2;
            string op = "";
            try
            {


                while (op != "5")
                {
                    Console.WriteLine("Enter the first number: ");

                    n1 = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the second number: ");
                    n2 = decimal.Parse(Console.ReadLine());

                    Calculator calc = new Calculator(n1, n2);

                    Console.WriteLine("Select the operation:\n 1. Add\n 2. Subtract\n 3. Multiply\n 4. Divide\n 5. Exit");
                    op = Console.ReadLine();

                    switch (op)
                    {
                        case "1":
                            Console.WriteLine("The sum of the 2 numbers is " + calc.Add());
                            break;

                        case "2":
                            Console.WriteLine("The differnce between the 2 numbers is " + calc.Subtract());
                            break;

                        case "3":
                            Console.WriteLine("The product of the 2 numbers is " + calc.Multiply());
                            break;

                        case "4":
                            if (n2 == 0)
                            {
                                Console.WriteLine("Cannot divide a number by 0");
                                break;
                            }
                            calc.Divide();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nIncorrect input. You can only enter numbers");
            }
        }
    }

    class Calculator
    {
        private decimal n1, n2;

        public Calculator(decimal _n1, decimal _n2)
        {
            n1 = _n1;
            n2 = _n2;
        }

        public decimal Add()
        {
            return n1 + n2;
        }

        public decimal Subtract()
        {
            return n1 - n2;
        }

        public decimal Multiply()
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

        public void Divide()
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