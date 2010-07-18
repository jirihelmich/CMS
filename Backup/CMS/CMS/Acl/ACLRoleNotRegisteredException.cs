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

namespace CMS.CMS.Acl
{
    public class ACLRoleNotRegisteredException : Exception
    {

        /// <summary>
        /// Role name
        /// </summary>
        protected string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Role name</param>
        public ACLRoleNotRegisteredException(String name)
        {
            _name = name;
        }

        /// <summary>
        /// Property for getting Error message
        /// </summary>
        public override string Message
        {
            get
            {
                return "Role " + this._name + " není registrována";
            }
        }

    }
}
