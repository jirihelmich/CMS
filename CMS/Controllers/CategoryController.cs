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
    public class CategoryController : CMS_Controller
    {
        //
        // GET: /Category/List

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

            ViewData["articlesCount"] = this._app.articles().getCountByCategoryId(id);
            ViewData["children"] = this._app.categories().get(id, 0, 100000);
            return View(this._app.articles().getByCategoryId(id, this._app.ListLength, page * this._app.ListLength));
        }

    }
}
