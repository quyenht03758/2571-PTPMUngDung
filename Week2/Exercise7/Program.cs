using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter radius r: ");
            double r = double.Parse(Console.ReadLine());

            double area = Math.PI * r * r;
            Console.WriteLine($"Circle area = {area}");

        }
    }
}
