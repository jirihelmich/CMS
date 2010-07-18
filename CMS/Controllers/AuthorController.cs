using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.CMS.Ctrl;
using CMS.Models;

namespace CMS.Controllers
{
    public class AuthorController : CMS_Controller
    {
        //
        // GET: /Author/List

        public ActionResult List()
        {
            long id = 0;
            try
            {
                id = long.Parse(Request.Params["id"]);
            }
            catch
            { }

            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["articlesCount"] = this._app.articles().getCountByAuthorId(id);

            return View(this._app.articles().getByAuthorId(id, this._app.ListLength, page * this._app.ListLength));
        }

    }
}
