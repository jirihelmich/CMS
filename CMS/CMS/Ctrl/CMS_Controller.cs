using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.CMS.Services;
using CMS.CMS.App;

namespace CMS.CMS.Ctrl
{
    public class CMS_Controller : Controller
    {
        /// <summary>
        /// Application instance
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Messages instance
        /// </summary>
        protected CMS_Services_Message _messages;

        public static String[] langs = new String[] { "cz", "gb", "de", "ru", "fr", "pl" };

        /// <summary>
        /// Constructor
        /// </summary>
        public CMS_Controller()
        {
            this._app = new CMS_App();
            this._messages = CMS_Services_Message.getInstance();
            this.ViewData["langs"] = langs;
        }

        /// <summary>
        /// ACL trigger
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string action = filterContext.ActionDescriptor.ActionName;
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string resource = controller + ":" + action;

            try
            {
                this._app.checkACL(resource, this._app.getAcl());
            }
            catch (Acl.ACLResourceNotRegisteredException)
            {
                _messages.addError("You are about requesting a resource which is not registered");
                filterContext.HttpContext.Response.Redirect("/",true);
            }
            catch
            {
                _messages.addError("You aren't allowed to view the datasource!");
                filterContext.HttpContext.Response.Redirect("/user/login?backUrl=" + Request.Url, true);
            }

            this.ViewData["menu"] = this._app.categories().get(0, 0, 100000);

            bool logged = this._app.users().isLogged();
            this.ViewData["logged"] = logged;
            if (logged)
            {
                this.ViewData["user"] = this._app.users().getLogged().username;
            }

            ViewData["listLength"] = this._app.ListLength;
            ViewData["unregisteredComments"] = this._app.UnregistredComments;
        }
    }
}
