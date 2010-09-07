using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ImageController : Controller
    {
        

        public ActionResult Thumbnail()
        {
            try
            {
                int width = Int32.Parse(Request.Params["w"]);
                int height = Int32.Parse(Request.Params["h"]);
                string path = Request.MapPath("~/files/"+Request.Params["path"]);

                ImageResize.Models.ImageEditing resizer = new ImageResize.Models.ImageEditing(path);

                return new ImageResize.ActionResults.ImageActionResult(resizer.Resize(width, height));
            }
            catch (Exception e)
            {
                return View();
            }
        }

    }
}
