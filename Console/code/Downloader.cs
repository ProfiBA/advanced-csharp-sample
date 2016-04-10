using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using ConsoleApp.code.model;
using ConsoleApp.code.utils;

namespace ConsoleApp.code
{
    public class MyDownload : IDownloader
    {
        private readonly ILogger _log;
        private readonly ILocalStorage _localStorage;

        public MyDownload(ILogger log, ILocalStorage ls)
        {
            _log = log;
            _localStorage = ls;
        }

        public void CheckForUpdate<T>(string serverUrl)
        {
            _localStorage.FindLocalPlugins();
            using (var w = new WebClient())
            {
                var jsonData = string.Empty;
                try
                {
                    _log.Print("Checking for updates..");
                    jsonData = w.DownloadString(serverUrl);
                }
                catch (Exception ex)
                {
                    _log.PrintError(ex.Message);
                }

                if (!string.IsNullOrEmpty(jsonData))
                {
                    _localStorage.DeserializeAndCompare(jsonData);
                }
            }
        }

        public void DownloadUpdates()
        {
            var result = _localStorage.GetNewPlugins();

            if (result == null)
                return;

            if (result.Count == 0)
            {
                _log.PrintSuccess("No new updates found!");
                return;
            }

            _log.Print("Found " + result.Count + " updates:");
            foreach (Plugin t in result)
            {
                if (t.Status == Plugin.State.Update)
                    _log.Print("Found new update for " + t.Name + " plugin");
                else if (t.Status == Plugin.State.New)
                    _log.Print("Found new plugin: " + t.Name);
            }

            DownloadFiles(result);
        }

        private void DownloadFiles(List<Plugin> res)
        {
            _log.Print("Starting download");

            foreach (Plugin p in res)
            {
                _log.Print("Downloading: " + p.Name);
                DownloadManager downloadManager = new DownloadManager();
                downloadManager.DownloadFile(Constants.RepoURL + p.Name, _localStorage.GetPluginsPath() + p.Name);
                if (p.Status == Plugin.State.New)
                _localStorage.AddNewLocalPlugin(p);         
            }
        }

        public class DownloadManager
        {
            public void DownloadFile(string sourceUrl, string targetFolder)
            {
                WebClient downloader = new WebClient();
                // fake as if you are a browser making the request.
                downloader.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");
                downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
                downloader.DownloadFileAsync(new Uri(sourceUrl), targetFolder);
                // wait for the current thread to complete, since the an async action will be on a new thread.
                while (downloader.IsBusy) { Thread.Sleep(200);}
            }

            private void Downloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                ILogger ad = new Logger();
                if (e.Error != null)
                    ad.PrintError(e.Error.Message);
                else
                    ad.PrintSuccess("Download Completed!");
            }
        }




    }
}
