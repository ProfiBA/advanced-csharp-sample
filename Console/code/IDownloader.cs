using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.code
{
    interface IDownloader
    {
       void CheckForUpdate<T>(string serverUrl);
    }
}
