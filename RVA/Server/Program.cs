using System;
using Tim11.Travel;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Pokretanje test primera
            TestExample.RunTest();
            
            Console.WriteLine("\nPritisnite bilo koji taster za izlaz...");
            Console.ReadKey();
        }
    }
}
