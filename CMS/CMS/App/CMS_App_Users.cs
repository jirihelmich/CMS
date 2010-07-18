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

namespace CMS.CMS.App.Users
{
    public class CMS_App_Users
    {
        /// <summary>
        /// Application
        /// </summary>
        protected CMS_App _app;

        /// <summary>
        /// Login
        /// </summary>
        protected CMS_Login _login;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Application</param>
        public CMS_App_Users(CMS_App app)
        {
            _app = app;
            _login = new CMS_Login();
        }

        /// <summary>
        /// Returns user identified by the specified ID
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        public user getById(long id)
        {

            using (UserDataContext u = new UserDataContext())
            {
                try
                {
                    var data = u.users
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
        /// Returns count of users matching given username and password
        /// expected count should be 0 or 1
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Pasword</param>
        /// <returns>Amount of users matching given credentials</returns>
        public int getCountByUsernameAndPassword(string username, string password)
        {
            SHA1Managed sha = new SHA1Managed();
            string hash = BitConverter.ToString(sha.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", String.Empty).ToLower();

            using (UserDataContext users = new UserDataContext())
            {
                int authuser = users.users
                    .Where(u => (u.password == hash && u.username == username))
                    .Select(u => u).Count();

                if (authuser < 0 || authuser > 1)
                {
                    throw new Exception("Unexpected count of users matching given credentials");
                }

                return authuser;
            }
        }
        
        /// <summary>
        /// Gets a user by the specified username and password
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>user</returns>
        public user getByUsernameAndPassword(string username, string password)
        {
            SHA1Managed sha = new SHA1Managed();
            string hash = BitConverter.ToString(sha.ComputeHash(Encoding.Default.GetBytes(password))).Replace("-", String.Empty).ToLower();

            using (UserDataContext users = new UserDataContext())
            {
                try
                {
                    return users.users
                        .Where(u => (u.password == hash && u.username == username))
                        .Single();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Determines if the user is logged
        /// </summary>
        /// <returns>boolean</returns>
        public Boolean isLogged()
        {
            return _login.hasIdentity();
        }

        /// <summary>
        /// Gets logged user instance
        /// </summary>
        /// <returns>Logged user</returns>
        public user getLogged()
        {
            if (_login.hasIdentity())
            {
                return _login.getIdentity();
            }

            throw new Exception("User not logged in, cannot return his identity");
        }

        /// <summary>
        /// Gets a list of all authors
        /// </summary>
        /// <param name="a">DataContext</param>
        /// <returns>List of authors</returns>
        public List<author> getAuthors(LangDataContext a)
        {
            using (a)
            {
                var data = a.authors.OrderBy(x => x.lastname + " " + x.name);

                return data.ToList();
            }
        }

        /// <summary>
        /// Gets a list of all authors
        /// </summary>
        /// <returns>List of authors</returns>
        public List<author> getAuthors()
        {
            return this.getAuthors(new LangDataContext());
        }

        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>List of users</returns>
        public List<user> get()
        {
            using (UserDataContext u = new UserDataContext())
            {
                return u.users.ToList();
            }
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="form">User data</param>
        /// <returns>success</returns>
        public bool add(Form_User_Add form)
        {
            user newUser = new user();
            newUser.username = form["username"].getValue();
            newUser.rolesid = long.Parse(form["role"].getValue());
            newUser.email = form["email"].getValue();
            
            SHA1Managed sha = new SHA1Managed();
            string hash = BitConverter.ToString(sha.ComputeHash(Encoding.Default.GetBytes(form["password"].getValue()))).Replace("-", String.Empty).ToLower();
                
            newUser.password = hash;
            newUser.date = DateTime.Now;

            using (UserDataContext u = new UserDataContext())
            {
                try
                {
                    u.users.InsertOnSubmit(newUser);
                    u.SubmitChanges();
                }
                catch (Exception e)
                {
                    CMS.Services.CMS_Services_Message.getInstance().addError(e.Message);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Save changes to the given user
        /// </summary>
        /// <param name="form">User data</param>
        /// <param name="edited">Edited user</param>
        /// <returns>success</returns>
        public bool save(Form_User_Add form, user edited)
        {
            user toSave = new user();
            toSave.id = edited.id;
            toSave.password = edited.password;
            if (form["password"].getValue() != String.Empty)
            {
                SHA1Managed sha = new SHA1Managed();
                string hash = BitConverter.ToString(sha.ComputeHash(Encoding.Default.GetBytes(form["password"].getValue()))).Replace("-", String.Empty).ToLower();
                toSave.password = hash;
            }
            toSave.rolesid = long.Parse(form["role"].getValue());
            toSave.email = form["email"].getValue();
            toSave.username = form["username"].getValue();
            toSave.date = edited.date;

            using (UserDataContext u = new UserDataContext())
            {
                u.users.Attach(toSave, edited);
                try
                {
                    u.SubmitChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes an author from the system (if there is no article written by him - DB FK)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>success</returns>
        public bool removeAuthorByUserId(long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                a.authors.DeleteAllOnSubmit(a.authors.Where(x => x.usersid == id));
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
        /// Adds an author to the system
        /// </summary>
        /// <param name="form">Author data</param>
        /// <param name="id">User id</param>
        /// <returns>success</returns>
        public bool promoteUser(Form_Author_Add form, long id)
        {
            using (LangDataContext a = new LangDataContext())
            {
                author toSave = new author();
                toSave.date = DateTime.Now;
                toSave.description = form["description"].getValue();
                toSave.lastname = form["lastname"].getValue();
                toSave.name = form["name"].getValue();
                toSave.usersid = id;

                a.authors.InsertOnSubmit(toSave);
                try
                {
                    a.SubmitChanges();
                }
                catch(Exception e)
                {
                    CMS.Services.CMS_Services_Message.getInstance().addError(e.Message);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets an author for a user specified by the given id
        /// </summary>
        /// <param name="uid">User id</param>
        /// <returns>author</returns>
        public author getAuthorByUserId(long uid)
        {
            using (LangDataContext a = new LangDataContext())
            {
                try
                {
                    return a.authors.Where(x => x.usersid == uid).Single();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Save changes to the given author
        /// </summary>
        /// <param name="form">Author data</param>
        /// <param name="edited">Edited author</param>
        /// <returns>success</returns>
        public bool saveAuthor(Form_Author_Add form, author edited)
        {
            author toSave = new author();
            toSave.id = edited.id;
            toSave.lastname = form["lastname"].getValue();
            toSave.description = form["description"].getValue();
            toSave.name = form["name"].getValue();
            toSave.usersid = edited.usersid;
            toSave.date = edited.date;

            using (LangDataContext a = new LangDataContext())
            {
                a.authors.Attach(toSave, edited);
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
        }

        /// <summary>
        /// Gets count of users
        /// </summary>
        /// <returns>Count of users</returns>
        public int getCount()
        {
            using (UserDataContext u = new UserDataContext())
            {
                return u.users.Count();
            }
        }

        /// <summary>
        /// Deletes a user with the given id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>success</returns>
        public bool delete(long id)
        {
            using (UserDataContext u = new UserDataContext())
            {
                try
                {
                    u.users.DeleteAllOnSubmit(u.users.Where(x => x.id == id));
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
