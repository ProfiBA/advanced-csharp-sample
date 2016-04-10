using System;

namespace ConsoleApp.code
{
    class Logger : ILogger
    {
        private readonly DateTime _now = DateTime.Now;
        public void Print(string value)
        {
            Console.WriteLine("[" + _now + "] " + value);
        }

        public void PrintError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("["+_now + "] "+ value);
            Console.ResetColor();

        }

        public void PrintSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[" + _now + "] " + value);
            Console.ResetColor();
        }
    }
}
