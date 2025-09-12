using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter radius r: ");
            double r = double.Parse(Console.ReadLine());

            double circumference = 2 * Math.PI * r;
            Console.WriteLine($"Circle circumference = {circumference}");
        }
    }
}
