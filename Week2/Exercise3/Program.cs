using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the first integer: ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Enter the second integer: ");
            int b = int.Parse(Console.ReadLine());

            Console.WriteLine($"Sum = {a + b}");
            Console.WriteLine($"Difference = {a - b}");
            Console.WriteLine($"Product = {a * b}");
        }
    }
}
