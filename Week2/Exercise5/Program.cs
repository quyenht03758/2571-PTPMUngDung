using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a two-digit positive integer: ");
            int n = int.Parse(Console.ReadLine());

            int tens = n / 10;
            int ones = n % 10;
            int reversed = ones * 10 + tens;

            Console.WriteLine($"Reversed number: {reversed}");
        }
    }
}
