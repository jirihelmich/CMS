using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.CMS.Ctrl;

namespace CMS.Controllers
{
    [HandleError]
    public class HomeController : CMS_Controller
    {
        //
        // GET: /
        public ActionResult Index()
        {
            return View(this._app.news().getAll().Take(4));
        }
    }
}
