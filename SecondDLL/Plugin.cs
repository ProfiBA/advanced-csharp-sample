using System;
using PluginsInterface;

namespace SecondDLL
{
    public class Plugin: ISimplePlugin
    {
        public void Print()
        {
            Console.WriteLine("Hello from Second plugin");
        }
    }
    public class PluginVanja : ISimplePlugin
    {
        public void Print()
        {
            Console.WriteLine("Hello from Second Vanja plugin!");
        }
    }
}
