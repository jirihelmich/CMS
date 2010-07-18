using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.CMS.Ctrl;

namespace CMS.Controllers
{
    public class FeedController : CMS_Controller
    {

        //
        // GET: /Feed/Rss
        public ActionResult RSS()
        {

            string xml = this._app.makeRss(this._app.articles().getFrontpage(this._app.ListLength));

            this.Response.Output.Write(xml);
            this.Response.End();

            return View();
        }

    }
}
