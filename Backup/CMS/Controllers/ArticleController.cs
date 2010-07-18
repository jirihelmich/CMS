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
    public class ArticleController : CMS_Controller
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Article/Detail
        public ActionResult Detail()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                long id = 0;

                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch
                { }

                article a = this._app.articles().getById(id);

                if (a != null)
                {

                    ViewData["article"] = a;
                    ViewData["authors"] = this._app.articles().getAuthorsListById(id);
                    ViewData["categories"] = this._app.articles().getCategoriesListById(id);
                    ViewData["comments"] = this._app.articles().getCommentsById(id);
                    ViewData["tags"] = this._app.articles().getTagsById(id);

                    try
                    {
                        ViewData["chapter"] = int.Parse(Request.Params["chapter"]);
                    }
                    catch (Exception) {
                        ViewData["chapter"] = 0;
                    }

                    return View();
                }
                else
                {
                    this._messages.addError("Sorry, article not found");
                }
            }
            return RedirectToAction("Index","Home");
        }

    }
}
