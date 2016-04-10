using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ConsoleApp.code.model;
using ConsoleApp.code.utils;
using Newtonsoft.Json;

namespace ConsoleApp.code
{
    class LocalStorage:ILocalStorage
    {
        private readonly ILogger _log;

        private List<Plugin> _localPlugins;
        private List<Plugin> _remotePlugins;

        // Get plugins to download
        public List<Plugin> GetNewPlugins()
        {
            return _toDownload;
        }

        // Return all plugins (included updated/new ones)
        public List<Plugin> GetAllPlugins()
        {
            return _localPlugins;
            
        }
        // Set new plugin from downloader component
        public void AddNewLocalPlugin(Plugin plugin )
        {
          _localPlugins.Add(plugin);
              
        } 
        private List<Plugin> _toDownload { get; set; } 

        // Constructor
        public LocalStorage(ILogger log)
        {
            _log = log;
        }
        // Get path for plugins storage
        public string GetPluginsPath()
        {
            var localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return localDirectory+"\\Plugins\\"; 
        }

        // Deserialize JSON from web server
        public void DeserializeAndCompare(string json)
        {
            dynamic results = JsonConvert.DeserializeObject<RootResponse>(json);
            if (results != null)
            {
                if (results.Success == false)
                {
                    _log.PrintError(results.ErrMessage);
                }
                else if (results.Success)
                {
                    if (results.Data.Count == 1)
                        _log.Print(results.Data.Count + " plugin found on remote web server");
                    else
                    _log.Print(results.Data.Count+ " plugins found on remote web server");
                    _remotePlugins = results.Data;

                    CompareResults();
                }
            }
            
        }
        // Compare local found plugins with remote ones
        private void CompareResults()
        {
            // No local plugins, need to download all
            if (_localPlugins.Count == 0)
            {
                _toDownload = _remotePlugins;
                _toDownload.ForEach(c => c.Status = Plugin.State.New);
                return;
            }
            List<Plugin> updates = new List<Plugin>();
            foreach (Plugin l in _localPlugins)
            {
                foreach (Plugin r in _remotePlugins)
                {
                    if (l.Name == r.Name)
                    {
                        if (l.DateCreated < r.DateCreated)
                            updates.Add(r);
                    }
                }   
            }

            updates.ForEach(c => c.Status = Plugin.State.Update);

            List<Plugin> newPlugins = _remotePlugins.Where(x => _localPlugins.All(y => y.Name != x.Name)).ToList();
            newPlugins.ForEach(c => c.Status = Plugin.State.New);

            // Has some updates
            if (updates.Count > 0 || newPlugins.Count > 0)
            {
                _toDownload = new List<Plugin>();

                _toDownload.AddRange(updates);
                _toDownload.AddRange(newPlugins);
            }
            else if (updates.Count == 0 && newPlugins.Count == 0)
            {
                _log.PrintSuccess("No updates found!");
            }

    }
        // Find all local plugins
        public void FindLocalPlugins()
        {
            _log.Print("Searching for local plugins");
            var localDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            List<Plugin> results = new List<Plugin>();
            if (localDirectory != null)
            {
                string[] files;
                try
                {
                    files = Directory.GetFiles(GetPluginsPath(), "*.dll");

                    results.AddRange(files.Select(file => new FileInfo(file)).Select(fi => new Plugin
                    {
                        Name = fi.Name,
                        DateCreated = fi.LastWriteTimeUtc,
                        Hash = Utils.GetMD5(GetPluginsPath() + fi.Name)
                    }));

                    if (results.Count == 0)
                    {
                        _log.Print("No local plugins found");
                    }
                    else
                    {
                        if (results.Count == 1)
                            _log.Print("Found " + results.Count + " local plugin");
                        else
                           _log.Print("Found " + results.Count + " local plugins");
                        
                      
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    _log.PrintError("No Plugins directory found, creating one");
                    Directory.CreateDirectory(GetPluginsPath());
                    
                }
                catch (AggregateException ex)
                {
                    _log.PrintError(ex.Message);
                }  
            }
             _localPlugins = results;
        }
    }
}
