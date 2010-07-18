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
            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            string ordering = "date";
            if (Request.Form.AllKeys.Contains("ordering"))
            {
                ordering = Request.Form["ordering"];
            }

            ViewData["articlesCount"] = this._app.articles().getFrontpageArticlesCount();

            return View(this._app.articles().getFrontpage(this._app.ListLength, page * this._app.ListLength, ordering));
        }
    }
}
