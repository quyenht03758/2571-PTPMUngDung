using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the first positive integer: ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Enter the second positive integer: ");
            int b = int.Parse(Console.ReadLine());

            int max = (a > b) ? a : b;
            Console.WriteLine($"The larger number is: {max}");
        }
    }
}
