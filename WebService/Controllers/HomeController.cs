using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebService.Models;

namespace WebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // FOR DEBUG PURPOSE
        public ActionResult Log()
        {
            ViewBag.Message = "Developer log";

            var data = GetPluginsData();

            if (data.Count == 0)
                ViewBag.NoData = "Sorry no files found, please try again";

            return View(data);
        }

        // GET ALL PLUGINS IN JSON RESPONSE
        [HttpGet]
        public ActionResult GetPlugins()
        {
            try
            {
                // Get all plugins data
                var data = GetPluginsData();
                // If there is no plugins on server, return folowing message
                if (data.Count == 0)
                {
                    return Json(new { Success = false, ErrMessage = "No plugins found on server, please try again later" }, JsonRequestBehavior.AllowGet);
                }
                    // Return plugins as JSON 
                    return Json(new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ErrMessage = "A error occur: "+ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        private List<Plugin> GetPluginsData()
        {
             // Get path for Repository folder
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repository");

            // Get directory info for provided path
            DirectoryInfo di = new DirectoryInfo(path);

            // Get all .dll files (plugins) in provided folder except interface 
            FileInfo[] files = di.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            List<Plugin> returnResults = (from fis in files where fis.Name != "PluginsInterface.dll" select new Plugin { Name = fis.Name, DateCreated = fis.LastWriteTimeUtc, Hash = Utils.Utils.GetMD5(path+"\\"+fis.Name)}).ToList();
            return returnResults;

           
        }
    }
}