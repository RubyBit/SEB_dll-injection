using System;
using System.Threading;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            for (int i = 0; i < 10; i++)
            {
                printer();
                Thread.Sleep(2000);
            }
            Console.WriteLine("Finished");
            Thread.Sleep(2000);
        }

        static void printer()
        {
            Console.WriteLine("Hello");
        }
    }
}
