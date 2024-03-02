namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wellcome to this fancy calculator :)\n");
            bool exit = false;
            String command;
            int a;
            int b;
            int result;
            String operation;

            while (!exit)
            {
                Console.WriteLine("Press s - start, q - quite\n");
                command = Console.ReadLine().ToLower();
                switch (command)
                {
                    case "s":
                        break;
                    case "q":
                        exit = true;
                        Console.WriteLine("\nBye, bye");
                        continue;
                    default: 
                        Console.WriteLine("Wrong input");
                        continue;
                }
                while (true)
                {
                    Console.Write("a = ");
                    try 
                    {
                        a = int.Parse(Console.ReadLine());
                        break;
                    } catch (Exception e)
                    {
                        Console.WriteLine("Input number");
                    }
                }
                while (true)
                {
                    Console.Write("b = ");
                    try
                    {
                        b = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Input number");
                    }
                }
                Console.WriteLine("Operation +,-,*,/ ");
                while (true)
                {
                    operation = Console.ReadLine();
                    if (operation == "+")
                    {
                        result = a + b;
                        break;
                    }
                    else if (operation == "-")
                    {
                        result = a - b;
                        break;
                    }
                    else if (operation == "*")
                    {
                        result = a * b;
                        break;

                    }
                    else if (operation == "/")
                    {
                        if (b == 0)
                        {
                            Console.WriteLine("Wrong input, can not devide by zero");
                        } else
                        {
                            result = a / b;
                            break;
                        }
                    } else
                    {
                        Console.WriteLine("Wrong input");
                    }

                }
                Console.WriteLine("Result = " + result + "\n");
            }
        }
    }
}
