using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CMS.Forms;
using CMS.CMS;
using CMS.Models;
using CMS.CMS.Ctrl;
using System.Drawing;
using System.IO;
using System.Text;
using CMS.CMS.InputModels;
using CMS.CMS.OutputModels;

namespace CMS.Controllers
{
    public class BackendController : CMS_Controller
    {

        //
        // GET+POST: /Backend/NewArticle
        [ValidateInput(false)] 
        public ActionResult NewArticle()
        {

            List<author> authors = this._app.users().getAuthors();
            List<role> roles = this._app.roles().get();
            List<category> categories = this._app.categories().getAll();

            Form_Article_New form = new Form_Article_New();
            form.setAuthors(authors);
            form.setRoles(roles);
            form.setCategories(categories);

            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    if (this._app.articles().newArticle(form, Request))
                    {
                        _messages.addMessage("The article has been saved.");
                        return RedirectToAction("listArticles", "backend");
                    }
                    else {
                        _messages.addError("An error occured while saving the article.");
                    }
                }
            }

            ViewData["form"] = form.render();

            return View();
        }

        //
        // GET: /Backend/ListArticles
        public ActionResult ListArticles()
        {
            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["articlesCount"] = this._app.articles().getCount();

            return View(this._app.articles().getAll());
        }


        //
        // GET: /Backend/DeleteArticle?id={id}
        public ActionResult DeleteArticle()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                try
                {
                    long id = long.Parse(Request.Params["id"]);
                    this._app.articles().deleteArticle(id);
                }
                catch (Exception) {
                    this._messages.addError("No article found.");
                }

                return RedirectToAction("ListArticles", "Backend");
            }
            else
            {
                _messages.addError("Undefined parameter ID");
                return RedirectToAction("listArticles", "backend");
            }
        }

        //
        // GET+POST: /Backend/EditArticle?id={id}
        [ValidateInput(false)] 
        public ActionResult EditArticle()
        {

            if (Request.Params.AllKeys.Contains("id"))
            {

                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                article a = this._app.articles().getById(id);

                if (a != null)
                {
                    List<articles_author> articlesAuthorsCross = this._app.articles().getAuthorsById(id);
                    List<articles_category> articlesCategoriesCross = this._app.articles().getCategoriesById(id);
                    List<tag> articlesTags = this._app.articles().getTagsById(id);

                    List<author> authors = this._app.users().getAuthors();
                    List<role> roles = this._app.roles().get();
                    List<category> categories = this._app.categories().getAll();

                    Form_Article_Edit form = new Form_Article_Edit(a);
                    form.setAuthors(authors, articlesAuthorsCross);
                    form.setRoles(roles, a.level.ToString());
                    form.setCategories(categories, articlesCategoriesCross);
                    form.setTags(articlesTags);

                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {

                            if (this._app.articles().saveArticle(form, Request))
                            {
                                _messages.addMessage("The article has been saved.");
                                return RedirectToAction("listArticles", "backend");
                            }
                            else
                            {
                                _messages.addError("An error occured while saving the article.");
                            }
                        }
                    }

                    ViewData["form"] = form.render();

                    return View();
                }
            }

            _messages.addError("Undefined or wrong parameter ID");
            return RedirectToAction("listArticles", "backend");
        }

        //
        // GET: /Backend/ListResources
        public ActionResult ListResources()
        {
            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["resourcesCount"] = this._app.resources().getCount();
            return View(this._app.resources().get(this._app.ListLength, page * this._app.ListLength));
        }

        //
        // GET+POST: /Backend/AddResource
        public ActionResult AddResource()
        {

            Form_Resource_Add form = new Form_Resource_Add();

            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    long id;
                    if (this._app.resources().add(form, out id))
                    {
                        _messages.addMessage("The resource has been successfully saved");
                        return Redirect("/backend/EditResource?id="+id.ToString());
                    }
                }
            }

            ViewData["form"] = form.render();

            return View();
        }

        //
        // GET+POST: /Backend/EditResource?id={id}
        public ActionResult EditResource()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                Form_Resource_Add form = new Form_Resource_Add();

                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                resource edited = this._app.resources().getById(id);

                if (edited != null)
                {
                    List<role> crossRoles = this._app.roles().getAll();
                    List<role> allowedRoles = this._app.resources().getRolesById(id);

                    form.setEditData(edited, crossRoles, allowedRoles);

                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            if (this._app.resources().save(form, edited))
                            {
                                _messages.addMessage("The resource has been successfully saved");
                                return RedirectToAction("ListResources");
                            }
                        }
                    }

                    ViewData["form"] = form.render();

                    return View();
                }
            }
            
            _messages.addError("Undefined or wrong parameter ID");
            return RedirectToAction("listResources", "backend");
        }

        //
        // GET: /Backend/ListCategories
        public ActionResult ListCategories()
        {
            long parent = 0;
            if (Request.Params.AllKeys.Contains("parent"))
            {
                if (Request.Params["parent"] != String.Empty)
                {
                    try
                    {
                        parent = long.Parse(Request.Params["parent"]);
                    }
                    catch { }
                }
                ViewData["current"] = parent;

                if (parent > 0)
                {
                    category c = this._app.categories().getById(parent);
                    if (c != null)
                    {
                        if (c.parentid.HasValue) ViewData["parent"] = c.parentid.Value;
                    }
                }
            }

            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["categoriesCount"] = this._app.categories().getCount(parent);

            return View(this._app.categories().get(parent, page * this._app.ListLength, this._app.ListLength));
        }

        //
        // GET+POST: /Backend/AddCategory[?parent={id}]
        public ActionResult AddCategory()
        {
            long parent = 0;
            if (Request.Params.AllKeys.Contains("parent"))
            {
                try
                {
                    parent = long.Parse(Request.Params["parent"]);
                }
                catch { }
            }

            string name = "Base imaginary category (root)";

            if (parent > 0)
            {
                category c = this._app.categories().getById(parent);
                if (c != null)
                {
                    //name = c.name;
                }
            }

            ViewData["name"] = name;

            Form_Category_Add form = new Form_Category_Add(parent);
            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    if (this._app.categories().add(parent, form))
                    {
                        _messages.addMessage("The category has been successfully added");
                        return Redirect("/backend/ListCategories?parent=" + parent.ToString());
                    }
                }
            }

            ViewData["form"] = form.render();

            return View();
        }

        //
        // GET+POST: /Backend/EditCategory?id={id}
        public ActionResult EditCategory()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {

                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                category c = this._app.categories().getById(id);

                if (c != null)
                {
                    Form_Category_Add form = new Form_Category_Add(0);
                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            if (this._app.categories().save(form, c))
                            {
                                _messages.addMessage("The category has been successfully saved");
                                if (c.parentid.HasValue)
                                {
                                    return Redirect("/backend/ListCategories?parent=" + c.parentid);
                                }
                                return Redirect("/backend/ListCategories?parent=" + c.id);
                            }
                        }
                    }

                    form.setEditData(c);

                    ViewData["form"] = form.render();

                    return View();
                }
            }
            _messages.addError("Undefined or wrong parameter ID");
            return RedirectToAction("listCategories", "backend");
        }

        //
        // GET: /Backend/ListRoles
        public ActionResult ListRoles()
        {
            long parent = 0;
            if (Request.Params.AllKeys.Contains("parent"))
            {
                if (Request.Params["parent"] != String.Empty)
                {
                    try
                    {
                        parent = long.Parse(Request.Params["parent"]);
                    }
                    catch { }
                }
                ViewData["current"] = parent;

                if (parent > 0)
                {
                    role r = this._app.roles().getById(parent);
                    if (r != null)
                    {
                        if (r.parentid.HasValue) ViewData["parent"] = r.parentid.Value;
                    }
                }
            }


            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["rolesCount"] = this._app.roles().getCount();
            return View(this._app.roles().get(parent, page * this._app.ListLength, this._app.ListLength));
        }

        //
        // GET+POST: /Backend/AddRole[?parent={id}]
        public ActionResult AddRole()
        {
            long parent = 0;
            if (Request.Params.AllKeys.Contains("parent"))
            {
                try
                {
                    parent = long.Parse(Request.Params["parent"]);
                }
                catch { }
            }

            string name = "root";

            if (parent > 0)
            {
                role r = this._app.roles().getById(parent);
                if (r != null)
                {
                    name = r.name;
                }
            }

            ViewData["name"] = name;

            Form_Role_Add form = new Form_Role_Add(parent);
            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    if (this._app.roles().add(parent, form))
                    {
                        _messages.addMessage("The role has been successfully added");
                        return Redirect("/backend/ListRoles?parent=" + parent.ToString());
                    }
                }
            }

            ViewData["form"] = form.render();

            return View();
        }

        //
        // GET: /Backend/EditRole?id={id}
        public ActionResult EditRole()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                role r = this._app.roles().getById(id);

                if (r != null)
                {
                    Form_Role_Add form = new Form_Role_Add(0);
                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            if (this._app.roles().save(form, r))
                            {
                                _messages.addMessage("The role has been successfully saved");
                                if (r.parentid.HasValue)
                                {
                                    return Redirect("/backend/ListRoles?parent=" + r.parentid);
                                }
                                return Redirect("/backend/ListRoles?parent=" + r.id);
                            }
                        }
                    }

                    form.setEditData(r);

                    ViewData["form"] = form.render();

                    return View();
                }
            }

            _messages.addError("Undefined or wrong parameter ID");
            return RedirectToAction("listRoles", "backend");
        }

        //
        // GET: /Backend/ListUsers
        public ActionResult ListUsers()
        {

            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["usersCount"] = this._app.users().getCount();

            ViewData["authors"] = this._app.users().getAuthors().Select(x => x.usersid).ToList();
            return View(this._app.users().get());
        }

        //
        // GET+POST: /Backend/AddUser
        public ActionResult AddUser()
        {

            Form_User_Add form = new Form_User_Add();
            form.setRoles(this._app.roles().get());

            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    if (this._app.users().add(form))
                    {
                        _messages.addMessage("The user has been successfully added");
                        return Redirect("/backend/ListUsers");
                    }
                }
            }

            ViewData["form"] = form.render();

            return View();
        }

        //
        // GET+POST: /Backend/EditUser?id={id}
        public ActionResult EditUser()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                Form_User_Add form = new Form_User_Add();
                form.setRoles(this._app.roles().getAll());

                user edited = this._app.users().getById(id);

                if (edited != null)
                {
                    form.setEditData(edited);

                    if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                    {
                        if (form.isValid(Request.Form))
                        {
                            if (this._app.users().save(form, edited))
                            {
                                _messages.addMessage("The user has been successfully saved");
                                return Redirect("/backend/ListUsers");
                            }
                            else
                            {
                                _messages.addError("The user hasn't been saved");   
                            }
                        }
                    }

                    ViewData["form"] = form.render();

                    return View();
                }
            }
            _messages.addError("Undefined or wrong parameter ID");
            return RedirectToAction("listUsers", "backend");
        }

        //
        // GET+POST: /Backend/PromoteUser?id={id}
        [ValidateInput(false)] 
        public ActionResult PromoteUser()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                Form_Author_Add form = new Form_Author_Add(id);

                if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                {
                    if (form.isValid(Request.Form))
                    {
                        if (this._app.users().promoteUser(form, id))
                        {
                            _messages.addMessage("The user has been successfully promoted");
                            return Redirect("/backend/ListUsers");
                        }
                        else
                        {
                            _messages.addMessage("There was an error while promoting the user");
                        }
                    }
                }

                ViewData["form"] = form.render();

                return View();
            }
            else
            {
                _messages.addError("Undefined parameter ID");
                return RedirectToAction("listUsers", "backend");
            }
        }

        //
        // GET: /Backend/DemoteUser?id={id}
        public ActionResult DemoteUser()
        {
            if (Request.Params.AllKeys.Contains("id"))
            {
                long id = 0;
                try
                {
                    id = long.Parse(Request.Params["id"]);
                }
                catch { }

                if (this._app.users().removeAuthorByUserId(id))
                {
                    _messages.addMessage("The author has been successfully demoted.");
                }
                else
                {
                    _messages.addError("There was an error while demoting the user because of binded data.");
                }

                return RedirectToAction("ListUsers");
            }
            else
            {
                _messages.addError("Undefined parameter ID");
                return RedirectToAction("listUsers", "backend");
            }
        }

        //
        // GET+POST: /Backend/EditAuthor?uid={uid}
        [ValidateInput(false)] 
        public ActionResult EditAuthor()
        {
            if (Request.Params.AllKeys.Contains("uid"))
            {
                long uid = 0;
                try
                {
                    uid = long.Parse(Request.Params["uid"]);
                }
                catch { }

                author edited = this._app.users().getAuthorByUserId(uid);

                Form_Author_Add form = new Form_Author_Add(uid);
                form.setEditData(edited);

                if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
                {
                    if (form.isValid(Request.Form))
                    {
                        if (this._app.users().saveAuthor(form, edited))
                        {
                            _messages.addMessage("The author has been successfully saved");
                            return Redirect("/backend/ListUsers");
                        }
                        else
                        {
                            _messages.addError("There was an error while saving the author");
                        }
                    }
                }

                ViewData["form"] = form.render();

                return View();
            }
            else
            {
                _messages.addError("Undefined parameter UID");
                return RedirectToAction("listUsers", "backend");
            }
        }

        //
        // GET: /Backend/DeleteUser?id={id}
        public ActionResult DeleteUser()
        {
            try
            {
                long id = long.Parse(Request.Params["id"]);
                if (this._app.users().delete(id))
                {
                    this._messages.addMessage("The user has been successfully deleted");
                }
                else
                {
                    this._messages.addError("The user cannot be deleted because of binded data");
                }
            }
            catch { }
            return RedirectToAction("ListUsers", "backend");
        }

        //
        // GET: /Backend/DeleteResource?id={id}
        public ActionResult DeleteResource()
        {
            try
            {
                long id = long.Parse(Request.Params["id"]);
                if (this._app.resources().delete(id))
                {
                    this._messages.addMessage("The resource has been successfully deleted");
                }
                else
                {
                    this._messages.addError("The resource cannot be deleted because of binded data");
                }
            }
            catch { }
            return RedirectToAction("ListResources", "backend");
        }

        //
        // GET: /Backend/DeleteRole?id={id}
        public ActionResult DeleteRole()
        {
            try
            {
                long id = long.Parse(Request.Params["id"]);
                if (this._app.roles().delete(id))
                {
                    this._messages.addMessage("The role has been successfully deleted");
                }
                else
                {
                    this._messages.addError("The role cannot be deleted because of binded data");
                }
            }
            catch { }
            return RedirectToAction("ListRoles", "backend");
        }

        //
        // GET: /Backend/DeleteCategory?id={id}
        public ActionResult DeleteCategory()
        {
            try
            {
                long id = long.Parse(Request.Params["id"]);
                if (this._app.categories().delete(id))
                {
                    this._messages.addMessage("The category has been successfully deleted");
                }
                else
                {
                    this._messages.addError("The category cannot be deleted because of binded data");
                }
            }
            catch { }
            return RedirectToAction("ListCategories", "backend");
        }

        //
        // GET: /Backend/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeletePageAjax(long id)
        {
            return Json(2);//this._app.pages().delete(id));
        }

        public ActionResult ListPages()
        {
            int page = 0;
            try
            {
                page = int.Parse(Request.Params["page"]);
            }
            catch { }

            ViewData["pagesCount"] = this._app.pages().getCount();

            return View(this._app.pages().getAll());
        }

        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPageAjax(PageInputModel p)
        {
            return Json(this._app.pages().add(p));
        }


        //
        // GET+POST: /Backend/Settings
        public ActionResult Settings()
        {
            Form_Settings form = new Form_Settings(this._app.ListLength, this._app.UnregistredComments);

            if (Request.HttpMethod.ToLower() == form.getMethod().ToString())
            {
                if (form.isValid(Request.Form))
                {
                    if (this._app.saveSettings(form))
                    {
                        _messages.addMessage("Settings have been successfully saved");
                        return Redirect("/backend");
                    }
                    else
                    {
                        _messages.addError("There was an error while saving settings");
                    }
                }
            }

            ViewData["form"] = form.render();
            return View();
        }

        public ActionResult Upload() {
            Object info = this._app.saveUploadedFile(Request);

            return new FileUploadJsonResult { Data = info };
        }

    }
}
