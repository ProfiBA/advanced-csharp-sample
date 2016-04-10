using System;
using System.Collections.Generic;
using System.Reflection;
using ConsoleApp.code.model;

namespace ConsoleApp.code
{
    // Class that load DLL's
    public class Execute
    {
        private readonly ILocalStorage _localStorage;
        private readonly ILogger _log;

        public Execute(ILogger log, ILocalStorage localStorage)
        {
            _log = log;
            _localStorage = localStorage;
        }
        public void ExecuteAllDlls()
        {
            List<Plugin> plugins = _localStorage.GetAllPlugins();
            if (plugins.Count == 0)
            {
                _log.Print("No plugins found.. nothing to execute");
                return;
            }

            _log.Print("========= Executing DLL's print functions  =========");

            foreach (Plugin p in plugins)
            {
                _log.Print("Result for print fuction from :"+p.Name);
                var dll = Assembly.LoadFile(_localStorage.GetPluginsPath()+p.Name);
                foreach (Type type in dll.GetExportedTypes())
                {
                    try
                    {
                        dynamic c = Activator.CreateInstance(type);
                        c.Print();
                    }
                    catch (MissingMethodException)
                    {
                        _log.PrintError(p.Name+" does not contain print method.. skipping..");
                       
                    }
                    catch (AggregateException ex)
                    {
                        _log.PrintError("Error while loading "+p.Name + ex.Message);
                    }   
                }
            }
        }

    }
}
