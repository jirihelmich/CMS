using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.CMS.Ctrl;
using CMS.Forms;

namespace CMS.Controllers
{
    public class CommentController : CMS_Controller
    {
        //
        // GET+POST: /Comment/Add

        public ActionResult Add()
        {
            try
            {
                if (Request.Params.AllKeys.Contains("aid"))
                {
                    long id = long.Parse(Request.Params["aid"]);
                    Form_Comment_New form = new Form_Comment_New(id,"aid");

                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            if (this._app.comments().add(id, form, Request.UserHostAddress))
                            {
                                _messages.addMessage("The comment has been successfully saved");
                                return Redirect("/article/detail?chapter=0&id=" + id);
                            }
                        }
                    }

                    ViewData["form"] = form.render();

                    return View();
                }
                else if (Request.Params.AllKeys.Contains("id"))
                {
                    long id = long.Parse(Request.Params["id"]);
                    Form_Comment_New form = new Form_Comment_New(id, "id");

                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            long aid;
                            if (this._app.comments().reply(id, form, Request.UserHostAddress, out aid))
                            {
                                _messages.addMessage("The comment has been successfully saved");
                                return Redirect("/article/detail?chapter=0&id="+aid);
                            }
                        }
                    }

                    ViewData["form"] = form.render();

                    return View();
                }
                
                
            }catch{
            
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
