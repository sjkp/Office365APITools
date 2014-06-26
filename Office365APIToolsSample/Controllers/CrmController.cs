using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Office365APIToolsSample.Models;

namespace Office365APIToolsSample.Controllers
{
    public class CrmController : Controller
    {
        // GET: Crm
        public async Task<ActionResult> Index()
        {
            var client = Office365.CrmClient;

            string accountsUrl = "https://sjkpdev06.crm4.dynamics.com/XRMServices/2011/OrganizationData.svc/AccountSet";


            var accounts = await client.Get<IEnumerable<Account>>(accountsUrl);
            ViewBag.Accounts = accounts;
            return View();
        }
    }
}