using System;
namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome7715();
            Welcome2329();
            Console.ReadKey();
        }
        static partial void Welcome2329();
        private static void Welcome7715()
        {
            Console.WriteLine("Enter your name");
            string? userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console aplication", userName);
        }
    }
}
