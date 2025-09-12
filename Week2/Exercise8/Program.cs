using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter side a: ");
            double a = double.Parse(Console.ReadLine());

            double area = a * a;
            Console.WriteLine($"Square area = {area}");
        }
    }
}
