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
    public class ACLResourceNotRegisteredException : Exception
    {
        
        /// <summary>
        /// Resource name
        /// </summary>
        protected string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Resource name</param>
        public ACLResourceNotRegisteredException(String name)
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
                return "Resource " + this._name + " is not registered";
            }
        }
    }
}
