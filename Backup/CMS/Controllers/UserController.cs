using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.CMS.Ctrl;
using CMS.Forms;
using CMS.CMS.Login;
using System.Web.Security;
using CMS.Models;

namespace CMS.Controllers
{
    public class UserController : CMS_Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }


        //
        // GET+POST: /User/Login
        public ActionResult Login()
        {
            CMS_Login login = new CMS_Login();
            if (login.hasIdentity())
            {
                _messages.addMessage("You are already logged in.");
                return RedirectToAction("Index", "Home");
            }

            Form_LoginForm form = new Form_LoginForm();

            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    user user;
                    if (login.checkCredentials(form["username"].getValue(), form["password"].getValue(), out user))
                    {
                        login.SignIn(user);
                        return RedirectToAction("Index", "Home");
                    }
                    _messages.addError("Bad login or password");
                }
            }

            string html = form.render();

            ViewData["form"] = html;

            return View();
        }

        //
        // GET: /User/Logout
        public ActionResult Logout()
        {
            CMS_Login l = new CMS_Login();
            l.SignOut();
            return RedirectToAction("Login");
        }

    }
}
