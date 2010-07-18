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
using System.Security.Cryptography;
using System.Text;
using CMS.CMS.Login;
using System.Collections.Generic;
using CMS.Forms;
using CMS.CMS.Form.Element;

namespace CMS.CMS.App.Resources
{
    public class CMS_App_Resources
    {
        /// <summary>
        /// CMS_App instance
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Application</param>
        public CMS_App_Resources(CMS_App app)
        {
            _app = app;
        }

        /// <summary>
        /// Returns resource identified by the specified ID
        /// </summary>
        /// <param name="id">resource id</param>
        /// <returns></returns>
        public resource getById(long id)
        {

            using (ACLDataContext a = new ACLDataContext())
            {
                try
                {
                    var data = a.resources
                        .Where(x => x.id == id)
                        .Single();

                    return data;
                }catch(Exception)
                {
                    return null;
                }
            }
        
        }

        /// <summary>
        /// Returns a list of resources
        /// </summary>
        /// <param name="a">DataContext</param>
        /// <returns>List of resources</returns>
        public List<resource> get(ACLDataContext a)
        {
            using (a)
            {
                var data = a.resources;

                return data.ToList();
            }
        }

        /// <summary>
        /// Returns a list of resources
        /// </summary>
        /// <returns>List of resources</returns>
        public List<resource> get()
        {
            return this.get(new ACLDataContext());
        }

        /// <summary>
        /// Adds a new resource
        /// </summary>
        /// <param name="form">Resource data</param>
        /// <param name="id">Resource id</param>
        /// <returns>success</returns>
        public bool add(Form_Resource_Add form, out long id)
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                resource newRes = new resource();
                newRes.action = form["action"].getValue();
                newRes.controller = form["controller"].getValue();
                newRes.name = form["name"].getValue();
                newRes.date = DateTime.Now;

                a.resources.InsertOnSubmit(newRes);

                try
                {
                    a.SubmitChanges();
                }
                catch (Exception)
                {
                    id = 0;
                    return false;
                }

                id = newRes.id;
            }

            return true;
        }

        /// <summary>
        /// Save changes to the given resource
        /// </summary>
        /// <param name="form">Resource data</param>
        /// <param name="edited">Edited resource</param>
        /// <returns>success</returns>
        public bool save(Form_Resource_Add form, resource edited)
        {
            resource newRes = new resource();
            newRes.action = form["action"].getValue();
            newRes.controller = form["controller"].getValue();
            newRes.name = form["name"].getValue();
            newRes.date = DateTime.Now;
            newRes.id = edited.id;

            List<role_resource> insert = new List<role_resource>();

            foreach (CMS_Form_Element e in form.getElements())
            {
                if (e.getName().StartsWith("role_"))
                {
                    if (e.getValue() == "1")
                    {
                        string name = e.getName();
                        name = name.Replace("role_", "");

                        long roleId = long.Parse(name);

                        role_resource row = new role_resource();
                        row.resourcesid = edited.id;
                        row.rolesid = roleId;
                        row.date = DateTime.Now;

                        insert.Add(row);
                    }
                }
            }

            ACLDataContext a = new ACLDataContext();
            a.role_resources.DeleteAllOnSubmit(a.role_resources.Where(x => x.resourcesid == edited.id));
            a.role_resources.InsertAllOnSubmit(insert);
            a.resources.Attach(newRes, edited);

            try
            {
                a.SubmitChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets allowed roles for the given resource id
        /// </summary>
        /// <param name="id">resource id</param>
        /// <returns>List of allowed roles</returns>
        public List<role> getRolesById(long id)
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                return a.role_resources.Where(x => x.resourcesid == id).Select(x => x.role).ToList();
            }
        }

        /// <summary>
        /// Gets count of resources
        /// </summary>
        /// <returns>Count of resources</returns>
        public int getCount()
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                return a.resources.Count();
            }
        }

        /// <summary>
        /// Gets list of resources
        /// </summary>
        /// <param name="start">How many resources have to be skipped</param>
        /// <param name="count">Count of returned resources (max)</param>
        /// <returns>List of resources</returns>
        public List<resource> get(int count, int start)
        {
            using (ACLDataContext a = new ACLDataContext())
            {
                return a.resources.Skip(start).Take(count).ToList();
            }
        }

        /// <summary>
        /// Deletes a resource with the given id
        /// </summary>
        /// <param name="id">resource id</param>
        /// <returns>success</returns>
        public bool delete(long id)
        {
            using (ACLDataContext u = new ACLDataContext())
            {
                try
                {
                    u.resources.DeleteAllOnSubmit(u.resources.Where(x => x.id == id));
                    u.SubmitChanges();
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}