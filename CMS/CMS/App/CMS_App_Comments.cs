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
using CMS.Forms;

namespace CMS.CMS.App
{
    public class CMS_App_Comments
    {

        /// <summary>
        /// CMS_App instance
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Application</param>
        public CMS_App_Comments(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// Returns all commments related to the article indentified by the
        /// given id
        /// </summary>
        /// <param name="articleid">ID of article</param>
        //public void getByArticleId(long articleid)
        //{

        //    using (LangDataContext a = new LangDataContext())
        //    {
        //        var data = a.comments.Where(x => x.articleid == articleid).ToList();

        //        return data;
        //    }
        
        //}

        /// <summary>
        /// Returns a comment identified by the given id
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <returns></returns>
        public comment getById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                try
                {
                    var data = a.comments.Where(x => x.id == id).Single();

                    return data;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Removes the comment with the given id from data source
        /// </summary>
        /// <param name="id"></param>
        public void deleteById(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                try
                {
                    a.comments.DeleteOnSubmit(a.comments.Where(x => x.id == id).Single());
                }
                catch (Exception)
                {
                    CMS.Services.CMS_Services_Message.getInstance().addError("The comment cannot be deleted.");
                }
            }
        }

        /// <summary>
        /// Adds a new comment to the article specifed with given id
        /// </summary>
        /// <param name="id">comment id</param>
        /// <param name="form">form containing data</param>
        /// <param name="ip">Client IP address</param>
        /// <returns></returns>
        public bool add(long id, Form_Comment_New form, string ip)
        {
            comment c = new comment();
            c.useralias = form["name"].getValue();
            c.title = form["title"].getValue();
            c.text = form["text"].getValue();
            c.parentid = null;
            c.ip = ip;
            c.email = form["email"].getValue();
            c.articlesid = id;

            if (this._app.users().isLogged())
            {
                c.usersid = this._app.users().getLogged().id;
            }

            try
            {
                using (LangDataContext a = new LangDataContext())
                {
                    a.comments.InsertOnSubmit(c);
                    a.SubmitChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves a reply to a comment with the given id
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <param name="form">Reply data</param>
        /// <param name="aid">Article id</param>
        /// <param name="ip">Client IP address</param>
        /// <returns></returns>
        public bool reply(long id, Form_Comment_New form, string ip, out long aid)
        {
            comment c = new comment();
            c.useralias = form["name"].getValue();
            c.title = form["title"].getValue();
            c.text = form["text"].getValue();
            c.parentid = id;
            c.ip = ip;
            c.email = form["email"].getValue();

            if (this._app.users().isLogged())
            {
                c.usersid = this._app.users().getLogged().id;
            }

            try
            {
                using (LangDataContext a = new LangDataContext())
                {
                    comment replied = a.comments.Where(x => x.id == id).Single();
                    aid = c.articlesid = replied.articlesid;
                    a.comments.InsertOnSubmit(c);
                    a.SubmitChanges();
                }
            }
            catch
            {
                aid = 0;
                return false;
            }

            return true;
        }
    }
}
