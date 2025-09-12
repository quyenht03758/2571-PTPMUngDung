# WEEK 2
---

## Exercises

### Exercise 1
Create a sample Console Application (.NET Framework) in Visual Studio that enters your name and displays:  
**Hello world! <your name>**

```csharp
using System;

namespace Exercise1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine($"Hello world! {name}");
        }
    }
}
