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

namespace CMS.CMS.App.Roles
{
    public class CMS_App_Roles
    {
        /// <summary>
        /// Application instance
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Application</param>
        public CMS_App_Roles(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// Returns category identified by the specified ID
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Role represented by the given id</returns>
        public role getById(long id)
        {

            using (ACLDataContext a = new ACLDataContext())
            {
                try
                {
                    var data = a.roles
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
        /// List of roles
        /// </summary>
        /// <param name="id">parent role (0 for root)</param>
        /// <param name="a">DataContext</param>
        /// <param name="start">How many roles have to be skipped</param>
        /// <param name="count">The maximum amount of returned roles</param>
        /// <returns>List of roles</returns>
        public List<role> get(long id, ACLDataContext a, int start, int count)
        {
            using (a)
            {
                if (id > 0)
                {
                    var data = a.roles.Where(x => x.parentid == id).Skip(start).Take(count);
                    return data.ToList();
                }
                else if (id == 0)
                {
                    var data = a.roles.Where(x => x.parentid == null).Skip(start).Take(count);
                    return data.ToList();
                }
                else
                {
                    var data = a.roles.Skip(start).Take(count);
                    return data.ToList();
                }
            }
        }

        /// <summary>
        /// Returns first ten roles in a list
        /// </summary>
        /// <returns>List of roles</returns>
        public List<role> get()
        {
            return this.get(0, 0, 10);
        }

        /// <summary>
        /// List of roles
        /// </summary>
        /// <param name="id">parent role (0 for root)</param>
        /// <param name="start">How many roles have to be skipped</param>
        /// <param name="count">The maximum amount of returned roles</param>
        /// <returns>List of roles</returns>
        public List<role> get(long id, int start, int count)
        {
            return this.get(id, new ACLDataContext(), start, count);
        }

        /// <summary>
        /// Adds a new role
        /// </summary>
        /// <param name="parent">Parent role</param>
        /// <param name="form">Role data</param>
        /// <returns>success</returns>
        public bool add(long parent, Form_Role_Add form)
        {
            role r = new role();
            r.date = DateTime.Now;
            r.parentid = parent;
            if (parent == 0)
            {
                r.parentid = null;
            }
            r.name = form["name"].getValue();

            using (ACLDataContext a = new ACLDataContext())
            {
                a.roles.InsertOnSubmit(r);
                a.SubmitChanges();
            }

            return true;
        }

        /// <summary>
        /// Save changes to the given role
        /// </summary>
        /// <param name="form">Role data</param>
        /// <param name="r">Role</param>
        /// <returns>success</returns>
        public bool save(Form_Role_Add form, role r)
        {
            role newRole = new role();
            newRole.id = r.id;

            if (r.parentid.HasValue)
            {
                newRole.parentid = r.parentid;
            }

            newRole.name = form["name"].getValue();
            newRole.date = r.date;

            using (ACLDataContext a = new ACLDataContext())
            {
                a.roles.Attach(newRole, r);
                a.SubmitChanges();
            }

            return true;

        }

        /// <summary>
        /// Gets role by role name
        /// </summary>
        /// <param name="name">role name</param>
        /// <returns>Role</returns>
        public role getByName(string name)
        {
            using (ACLDataContext c = new ACLDataContext())
            {
                try
                {
                    return c.roles.Where(x => x.name == name).Single();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets count of roles
        /// </summary>
        /// <returns>Count of roles</returns>
        public int getCount()
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                return a.roles.Count();
            }
        }


        /// <summary>
        /// Deletes a role with the specified id
        /// </summary>
        /// <param name="id">role id</param>
        /// <returns>success</returns>
        public bool delete(long id)
        {
            using (ACLDataContext u = new ACLDataContext())
            {
                try
                {
                    u.roles.DeleteAllOnSubmit(u.roles.Where(x => x.id == id));
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
        /// Gets all roles
        /// </summary>
        /// <returns></returns>
        public List<role> getAll()
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                return a.roles.ToList();
            }
        }
    }
}
