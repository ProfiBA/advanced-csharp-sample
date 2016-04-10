using System;
using PluginsInterface;

namespace FirstDLL
{
    public class Plugin : ISimplePlugin
    {
        public void Print()
        {
            Console.WriteLine("Hello from First plugin");
        }
    }

    public class VanjaPlugin : ISimplePlugin
    {
        public void Print()
        {
            Console.WriteLine("Hello from Vanja First plugin :)");
        }
    }
}
