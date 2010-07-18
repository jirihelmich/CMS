using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CMS.Models;
using System.Collections.Generic;
using CMS.Forms;

namespace CMS.CMS.App.Categories
{
    public class CMS_App_Categories
    {
        /// <summary>
        /// CMS_App instance
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">CMS_App instance</param>
        public CMS_App_Categories(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// Returns category identified by the specified ID
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Category represented by the specified id</returns>
        public category getById(long id)
        {
            using (ArticleDataContext a = new ArticleDataContext())
            {
                try
                {
                    var data = a.categories
                        .Where(x => x.id == id)
                        .Single();

                    return data;
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Returns a list of categories
        /// </summary>
        /// <param name="id">Parent category id (0 for root)</param>
        /// <param name="a">Data context</param>
        /// <param name="start">How many categories should be skipped</param>
        /// <param name="count">Amount of returned categories</param>
        /// <returns>List of categories</returns>
        public List<category> get(long id, ArticleDataContext a, int start, int count)
        {
            using (a)
            {
                if (id > 0)
                {
                    var data = a.categories.Where(x => x.parentid == id).Skip(start).Take(count);
                    return data.ToList();
                }
                else if (id == 0)
                {
                    var data = a.categories.Where(x => x.parentid == null).Skip(start).Take(count);
                    return data.ToList();
                }
                else
                {
                    var data = a.categories.Skip(start).Take(count);
                    return data.ToList();
                }
            }
        }

        /// <summary>
        /// Returns first 10 categories (root children)
        /// </summary>
        /// <returns>List of categories</returns>
        public List<category> get()
        {
            return this.get(0,0,10);
        }

        /// <summary>
        /// Returns a list of categories
        /// </summary>
        /// <param name="id">Parent category id (0 for root)</param>
        /// <param name="start">How many categories should be skipped</param>
        /// <param name="count">Amount of returned categories</param>
        /// <returns>List of categories</returns>
        public List<category> get(long id, int start, int count)
        {
            return this.get(id, new ArticleDataContext(), start, count);
        }

        /// <summary>
        /// Adds a new child for the given parent category
        /// </summary>
        /// <param name="parent">parent category (0 for root)</param>
        /// <param name="form">form containig data</param>
        /// <returns></returns>
        public bool add(long parent, Form_Category_Add form)
        {
            category c = new category();
            c.date = DateTime.Now;
            c.parentid = parent;
            if (parent == 0)
            {
                c.parentid = null;
            }
            c.name = form["name"].getValue();
            c.alias = this._app.makeAlias(form["name"].getValue());

            using (ArticleDataContext a = new ArticleDataContext())
            {
                a.categories.InsertOnSubmit(c);
                try
                {
                    a.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Save changes for the given category
        /// </summary>
        /// <param name="form">form containing data</param>
        /// <param name="c">category</param>
        /// <returns>success</returns>
        public bool save(Form_Category_Add form, category c)
        {
            category newCat = new category();
            newCat.id = c.id;

            if (c.parentid.HasValue)
            {
                newCat.parentid = c.parentid;
            }

            newCat.name = form["name"].getValue();
            newCat.date = c.date;

            using (ArticleDataContext a = new ArticleDataContext())
            {
                a.categories.Attach(newCat, c);
                try
                {
                    a.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Get count of children for the given category (0 for root)
        /// </summary>
        /// <returns>count of children</returns>
        public int getCount(long parent)
        {
            long? p = parent;
            if (p == 0) p = null;
            using (ArticleDataContext a = new ArticleDataContext())
            {
                return a.categories.Where(x=>x.parentid == p).Count();
            }
        }

        /// <summary>
        /// Deletes a category specified by the given id
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>success</returns>
        public bool delete(long id)
        {
            using (ArticleDataContext u = new ArticleDataContext())
            {
                try
                {
                    u.categories.DeleteAllOnSubmit(u.categories.Where(x => x.id == id));
                    u.SubmitChanges();
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns></returns>
        public List<category> getAll()
        {
            using (ArticleDataContext a = new ArticleDataContext())
            {
                return a.categories.ToList();
            }
        }
    }
}