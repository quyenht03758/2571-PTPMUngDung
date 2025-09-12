using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter smaller base a: ");
            double a = double.Parse(Console.ReadLine());

            Console.Write("Enter larger base b: ");
            double b = double.Parse(Console.ReadLine());

            Console.Write("Enter height h: ");
            double h = double.Parse(Console.ReadLine());

            double side = Math.Sqrt(Math.Pow((b - a) / 2, 2) + h * h);
            double perimeter = a + b + 2 * side;

            Console.WriteLine($"Isosceles trapezoid perimeter = {perimeter}");
        }
    }
}
