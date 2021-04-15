using Opg62;
using System;

namespace NewSubScriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Opret ny Subscriber ";
            Console.WriteLine("Skriv navne på bil forsikring");
            var bil = Console.ReadLine();
            var newSub = new SubScriber("Bil", bil, 1020);
            newSub.Run();
            Console.ReadKey();
        }
    }
}
