using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using CMS.CMS.App;
using CMS.Models;
using System.Web.SessionState;

namespace CMS.CMS.Login
{
    public class CMS_Login
    {

        /// <summary>
        /// Tries to find a user with given credentials
        /// </summary>
        /// <param name="userame">Username</param>
        /// <param name="password">Password</param>
        /// <param name="user">found user</param>
        /// <returns></returns>
        public bool checkCredentials(string userame, string password, out user user)
        {
            CMS_App app = new CMS_App();
            user u = app.users().getByUsernameAndPassword(userame, password);
            user = u;
            return (u != null);
        }

        /// <summary>
        /// signs in the given user
        /// </summary>
        /// <param name="user">user to be signed in</param>
        public void SignIn(user user)
        {
            HttpSessionState session = HttpContext.Current.Session;
            session["loggedUserData"] = user;
        }

        /// <summary>
        /// signs out the currently logged user
        /// </summary>
        public void SignOut()
        {
            HttpSessionState session = HttpContext.Current.Session;
            session["loggedUserData"] = null;
        }

        /// <summary>
        /// unserialize the data stored in session
        /// and returns user object for the currently signed in user
        /// </summary>
        /// <returns></returns>
        public user getIdentity()
        {
            HttpSessionState session = HttpContext.Current.Session;
            return (user) session["loggedUserData"];
        }

        /// <summary>
        /// Is anybody logged in?
        /// </summary>
        /// <returns></returns>
        public Boolean hasIdentity()
        {
            HttpSessionState session = HttpContext.Current.Session;
            return (session["loggedUserData"] != null);
        }

    }
}
