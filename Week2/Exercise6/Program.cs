using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Hours: ");
            int h = int.Parse(Console.ReadLine());

            Console.Write("Minutes: ");
            int m = int.Parse(Console.ReadLine());

            Console.Write("Seconds: ");
            int s = int.Parse(Console.ReadLine());

            int total = h * 3600 + m * 60 + s;
            Console.WriteLine($"Total seconds = {total}");
        }
    }
}
