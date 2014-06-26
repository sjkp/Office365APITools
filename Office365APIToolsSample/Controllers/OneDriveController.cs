using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Office365APIToolsSample.Controllers
{
    public class OneDriveController : Controller
    {
        // GET: OneDrive
        public async Task<ActionResult> Index()
        {

            ViewBag.Files = await MyFilesApiSample.GetMyFiles();
            return View();
        }

        public async Task<ActionResult> MyFiles()
        {
            ViewBag.Files = await MyFilesApiSample.GetFiles();
            return View("Index");
        }

    }
}