using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.code.model;

namespace ConsoleApp.code
{
    public interface ILocalStorage
    {
        void DeserializeAndCompare(string json);
        void FindLocalPlugins();
        List<Plugin> GetNewPlugins();
        string GetPluginsPath();
        void AddNewLocalPlugin(Plugin plugin);
        List<Plugin> GetAllPlugins();
    }
}
