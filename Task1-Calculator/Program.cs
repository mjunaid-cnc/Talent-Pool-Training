using Task1_Calculator;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal n1, n2;
            string op = string.Empty;
            try
            {
                while (op != "5")
                {
                    Console.WriteLine("Enter the first number: ");
                    
                    n1 = decimal.Parse(Console.ReadLine()!);
                    Console.WriteLine("Enter the second number: ");
                    n2 = decimal.Parse(Console.ReadLine()!);

                    Console.WriteLine("Select the operation:\n 1. Add\n 2. Subtract\n 3. Multiply\n 4. Divide\n 5. Exit");
                    op = Console.ReadLine()!;

                    switch (op)
                    {
                        case "1":
                            var addObj = new Add(n1, n2);
                            Console.WriteLine("The sum of the 2 numbers is " + addObj.AddNumbers());
                            break;

                        case "2":
                            var subtractObj = new Subtract(n1, n2);
                            Console.WriteLine("The differnce between the 2 numbers is " + subtractObj.SubtractNumbers());
                            break;

                        case "3":
                            var multiplyObj = new Multiply(n1, n2);
                            Console.WriteLine("The product of the 2 numbers is " + multiplyObj.MultiplyNumbers());
                            break;

                        case "4":
                            if (n2 == 0)
                            {
                                Console.WriteLine("Cannot divide a number by 0");
                                break;
                            }
                            var divideObj = new Divide(n1, n2);
                            divideObj.DivideNumbers();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nIncorrect input. " + ex.Message);
            }
        }
    }
}